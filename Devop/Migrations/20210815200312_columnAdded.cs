using Microsoft.EntityFrameworkCore.Migrations;

namespace Devop.Migrations
{
    public partial class columnAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SuburbId",
                table: "EditUserModel",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SuburbId",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SuburbId",
                table: "AspNetUsers",
                column: "SuburbId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_tblSuburb_SuburbId",
                table: "AspNetUsers",
                column: "SuburbId",
                principalTable: "tblSuburb",
                principalColumn: "SuburbId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_tblSuburb_SuburbId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SuburbId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SuburbId",
                table: "EditUserModel");

            migrationBuilder.DropColumn(
                name: "SuburbId",
                table: "AspNetUsers");
        }
    }
}
