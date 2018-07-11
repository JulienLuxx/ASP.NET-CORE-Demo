using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Test.Domain.Migrations
{
    public partial class AddTimestamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Timestamp",
                table: "Comment",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Timestamp",
                table: "Article",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Article");
        }
    }
}
