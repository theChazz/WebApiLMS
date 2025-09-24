using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiLMS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserProgramEnrollments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserProgramEnrollments_ProgramId",
                table: "UserProgramEnrollments",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProgramEnrollments_UserId",
                table: "UserProgramEnrollments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgramEnrollments_Programs_ProgramId",
                table: "UserProgramEnrollments",
                column: "ProgramId",
                principalTable: "Programs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgramEnrollments_Users_UserId",
                table: "UserProgramEnrollments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProgramEnrollments_Programs_ProgramId",
                table: "UserProgramEnrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProgramEnrollments_Users_UserId",
                table: "UserProgramEnrollments");

            migrationBuilder.DropIndex(
                name: "IX_UserProgramEnrollments_ProgramId",
                table: "UserProgramEnrollments");

            migrationBuilder.DropIndex(
                name: "IX_UserProgramEnrollments_UserId",
                table: "UserProgramEnrollments");
        }
    }
}
