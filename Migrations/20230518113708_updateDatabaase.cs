using Microsoft.EntityFrameworkCore.Migrations;

namespace WebOS.Migrations
{
    public partial class updateDatabaase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Scholar",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Indx",
                table: "Scholar",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Scholar");

            migrationBuilder.DropColumn(
                name: "Indx",
                table: "Scholar");
        }
    }
}
