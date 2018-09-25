using Microsoft.EntityFrameworkCore.Migrations;

namespace Test.Domain.Migrations
{
    public partial class AlterUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MailBox",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mobile",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SaltValue",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MailBox",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "User");

            migrationBuilder.DropColumn(
                name: "SaltValue",
                table: "User");
        }
    }
}
