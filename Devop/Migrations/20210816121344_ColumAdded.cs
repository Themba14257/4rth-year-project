using Microsoft.EntityFrameworkCore.Migrations;

namespace Devop.Migrations
{
    public partial class ColumAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "EditUserModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "EditUserModel");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "AspNetUsers");
        }
    }
}
