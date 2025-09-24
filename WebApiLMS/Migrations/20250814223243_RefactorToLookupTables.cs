using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiLMS.Migrations
{
    /// <inheritdoc />
    public partial class RefactorToLookupTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_UserId_Role",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_CourseLecturerAssignments_CourseId_UserId_Role",
                table: "CourseLecturerAssignments");

            // Defer dropping Users.Role until after we map UserRoleId

            migrationBuilder.DropColumn(
                name: "AssignedAt",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "ProgramTypeEnum",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "CourseLecturerAssignments");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "UserRoles",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "AssignedBy",
                table: "UserRoles",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "UserRoleId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProgramTypeId",
                table: "Programs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProgramTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramTypes", x => x.Id);
                });

            // Seed lookup rows for ProgramTypes
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM ProgramTypes WHERE Code = 'Degree')
                    INSERT INTO ProgramTypes (Code, Name) VALUES ('Degree', 'Degree');
                IF NOT EXISTS (SELECT 1 FROM ProgramTypes WHERE Code = 'Learnership')
                    INSERT INTO ProgramTypes (Code, Name) VALUES ('Learnership', 'Learnership');
                IF NOT EXISTS (SELECT 1 FROM ProgramTypes WHERE Code = 'SkillsProgramme')
                    INSERT INTO ProgramTypes (Code, Name) VALUES ('SkillsProgramme', 'Skills Programme');
                IF NOT EXISTS (SELECT 1 FROM ProgramTypes WHERE Code = 'Apprenticeship')
                    INSERT INTO ProgramTypes (Code, Name) VALUES ('Apprenticeship', 'Apprenticeship');
            ");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserRoleId",
                table: "Users",
                column: "UserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_Code",
                table: "UserRoles",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Programs_ProgramTypeId",
                table: "Programs",
                column: "ProgramTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLecturerAssignments_CourseId_UserId",
                table: "CourseLecturerAssignments",
                columns: new[] { "CourseId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProgramTypes_Code",
                table: "ProgramTypes",
                column: "Code",
                unique: true);

            // Seed lookup rows for UserRoles (as reference table)
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM UserRoles WHERE Code = 'Student')
                    INSERT INTO UserRoles (Code, Name) VALUES ('Student', 'Student');
                IF NOT EXISTS (SELECT 1 FROM UserRoles WHERE Code = 'Lecturer')
                    INSERT INTO UserRoles (Code, Name) VALUES ('Lecturer', 'Lecturer');
                IF NOT EXISTS (SELECT 1 FROM UserRoles WHERE Code = 'Facilitator')
                    INSERT INTO UserRoles (Code, Name) VALUES ('Facilitator', 'Facilitator');
                IF NOT EXISTS (SELECT 1 FROM UserRoles WHERE Code = 'Assessor')
                    INSERT INTO UserRoles (Code, Name) VALUES ('Assessor', 'Assessor');
                IF NOT EXISTS (SELECT 1 FROM UserRoles WHERE Code = 'Moderator')
                    INSERT INTO UserRoles (Code, Name) VALUES ('Moderator', 'Moderator');
                IF NOT EXISTS (SELECT 1 FROM UserRoles WHERE Code = 'Admin')
                    INSERT INTO UserRoles (Code, Name) VALUES ('Admin', 'Administrator');
            ");

            // Map existing textual Program.Type values to ProgramTypeId
            migrationBuilder.Sql(@"
                UPDATE p
                SET ProgramTypeId = pt.Id
                FROM Programs p
                INNER JOIN ProgramTypes pt ON pt.Code = p.Type;
                -- Fallback to 'Degree' if still zero
                UPDATE Programs SET ProgramTypeId = (SELECT TOP 1 Id FROM ProgramTypes WHERE Code = 'Degree')
                WHERE ProgramTypeId = 0;
            ");

            // Map existing Users.Role values to Users.UserRoleId before dropping Role column
            migrationBuilder.Sql(@"
                UPDATE u
                SET UserRoleId = ur.Id
                FROM Users u
                INNER JOIN UserRoles ur ON ur.Code = u.Role;
                -- Fallback to 'Student' if still zero
                UPDATE Users SET UserRoleId = (SELECT TOP 1 Id FROM UserRoles WHERE Code = 'Student')
                WHERE UserRoleId = 0;
            ");

            // Now safe to drop the legacy Users.Role column
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Programs_ProgramTypes_ProgramTypeId",
                table: "Programs",
                column: "ProgramTypeId",
                principalTable: "ProgramTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserRoles_UserRoleId",
                table: "Users",
                column: "UserRoleId",
                principalTable: "UserRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Programs_ProgramTypes_ProgramTypeId",
                table: "Programs");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserRoles_UserRoleId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "ProgramTypes");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserRoleId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_Code",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_Programs_ProgramTypeId",
                table: "Programs");

            migrationBuilder.DropIndex(
                name: "IX_CourseLecturerAssignments_CourseId_UserId",
                table: "CourseLecturerAssignments");

            migrationBuilder.DropColumn(
                name: "UserRoleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProgramTypeId",
                table: "Programs");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "UserRoles",
                newName: "AssignedBy");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "UserRoles",
                newName: "Role");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "AssignedAt",
                table: "UserRoles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UserRoles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProgramTypeEnum",
                table: "Programs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "CourseLecturerAssignments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId_Role",
                table: "UserRoles",
                columns: new[] { "UserId", "Role" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseLecturerAssignments_CourseId_UserId_Role",
                table: "CourseLecturerAssignments",
                columns: new[] { "CourseId", "UserId", "Role" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
