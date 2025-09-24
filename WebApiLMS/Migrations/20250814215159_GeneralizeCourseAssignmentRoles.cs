using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiLMS.Migrations
{
    /// <inheritdoc />
    public partial class GeneralizeCourseAssignmentRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseLecturerAssignments_Users_LecturerId",
                table: "CourseLecturerAssignments");

            migrationBuilder.DropIndex(
                name: "IX_CourseLecturerAssignments_CourseId",
                table: "CourseLecturerAssignments");

            migrationBuilder.RenameColumn(
                name: "LecturerId",
                table: "CourseLecturerAssignments",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseLecturerAssignments_LecturerId",
                table: "CourseLecturerAssignments",
                newName: "IX_CourseLecturerAssignments_UserId");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "CourseLecturerAssignments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLecturerAssignments_CourseId_UserId_Role",
                table: "CourseLecturerAssignments",
                columns: new[] { "CourseId", "UserId", "Role" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseLecturerAssignments_Users_UserId",
                table: "CourseLecturerAssignments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseLecturerAssignments_Users_UserId",
                table: "CourseLecturerAssignments");

            migrationBuilder.DropIndex(
                name: "IX_CourseLecturerAssignments_CourseId_UserId_Role",
                table: "CourseLecturerAssignments");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "CourseLecturerAssignments");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "CourseLecturerAssignments",
                newName: "LecturerId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseLecturerAssignments_UserId",
                table: "CourseLecturerAssignments",
                newName: "IX_CourseLecturerAssignments_LecturerId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLecturerAssignments_CourseId",
                table: "CourseLecturerAssignments",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseLecturerAssignments_Users_LecturerId",
                table: "CourseLecturerAssignments",
                column: "LecturerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
