using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseProject_DataAccess.Migrations
{
    public partial class UpdateDbReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TagsData",
                table: "Review");

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Review",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Review");

            migrationBuilder.AddColumn<string>(
                name: "TagsData",
                table: "Review",
                type: "text",
                nullable: true);
        }
    }
}
