using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CurrencyRateApp.Context.Migrations
{
    public partial class Added_Authorization_key_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthorizationKeys",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApiKeyHash = table.Column<string>(maxLength: 255, nullable: false),
                    Salt = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizationKeys", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizationKeys_ApiKeyHash",
                table: "AuthorizationKeys",
                column: "ApiKeyHash",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorizationKeys");
        }
    }
}
