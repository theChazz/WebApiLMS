using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiLMS.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseResourceLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExcelUrl",
                table: "Courses",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PdfUrl",
                table: "Courses",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PowerPointUrl",
                table: "Courses",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TeamsJoinUrl",
                table: "Courses",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WordUrl",
                table: "Courses",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ZipUrl",
                table: "Courses",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExcelUrl",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "PdfUrl",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "PowerPointUrl",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "TeamsJoinUrl",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "WordUrl",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ZipUrl",
                table: "Courses");
        }
    }
}
