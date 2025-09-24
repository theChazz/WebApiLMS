using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiLMS.Migrations
{
    /// <inheritdoc />
    public partial class AddSetaCredentials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SetaBodies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetaBodies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserCredentials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CredentialType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SetaBodyId = table.Column<int>(type: "int", nullable: true),
                    Scope = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EvidenceUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCredentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCredentials_SetaBodies_SetaBodyId",
                        column: x => x.SetaBodyId,
                        principalTable: "SetaBodies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserCredentials_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SetaBodies_Code",
                table: "SetaBodies",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCredentials_SetaBodyId",
                table: "UserCredentials",
                column: "SetaBodyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCredentials_UserId_CredentialType_RegistrationNumber",
                table: "UserCredentials",
                columns: new[] { "UserId", "CredentialType", "RegistrationNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCredentials");

            migrationBuilder.DropTable(
                name: "SetaBodies");
        }
    }
}
