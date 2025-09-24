using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiLMS.Migrations
{
    /// <inheritdoc />
    public partial class AddProgramTypeEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProgramTypeEnum",
                table: "Programs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProgramTypeEnum",
                table: "Programs");
        }
    }
}
