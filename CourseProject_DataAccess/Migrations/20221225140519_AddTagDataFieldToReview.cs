using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseProject_DataAccess.Migrations
{
    public partial class AddTagDataFieldToReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TagsData",
                table: "Review",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TagsData",
                table: "Review");
        }
    }
}
