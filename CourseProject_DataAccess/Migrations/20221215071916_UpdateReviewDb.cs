using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseProject_DataAccess.Migrations
{
    public partial class UpdateReviewDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReviewId",
                table: "Comment",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ReviewId",
                table: "Comment",
                column: "ReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Review_ReviewId",
                table: "Comment",
                column: "ReviewId",
                principalTable: "Review",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Review_ReviewId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_ReviewId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "ReviewId",
                table: "Comment");
        }
    }
}
