using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsAggregatorData.Migrations
{
    /// <inheritdoc />
    public partial class AddNewsImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Sources_Name_Url",
                table: "Sources");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Sources",
                newName: "RSSUrl");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "News",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Sources_Name_RSSUrl",
                table: "Sources",
                columns: new[] { "Name", "RSSUrl" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_News_ImageUrl",
                table: "News",
                column: "ImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Sources_Name_RSSUrl",
                table: "Sources");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_News_ImageUrl",
                table: "News");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "News");

            migrationBuilder.RenameColumn(
                name: "RSSUrl",
                table: "Sources",
                newName: "Url");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Sources_Name_Url",
                table: "Sources",
                columns: new[] { "Name", "Url" });
        }
    }
}
