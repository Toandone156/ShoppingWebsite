using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingWebsite.Migrations
{
    public partial class AddDeleteAttr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Categories");
        }
    }
}
