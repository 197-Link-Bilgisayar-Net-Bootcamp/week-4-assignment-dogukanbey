using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace week4.Data.Migrations
{
    public partial class iniittt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Expiration",
                table: "UserRefreshTokens",
                newName: "Expiration");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "UserRefreshTokens",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Expiration",
                table: "UserRefreshTokens",
                newName: "Expriration");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "UserRefreshTokens",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);
        }
    }
}
