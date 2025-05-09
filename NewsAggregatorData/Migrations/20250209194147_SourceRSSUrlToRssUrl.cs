using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsAggregatorData.Migrations
{
    /// <inheritdoc />
    public partial class SourceRSSUrlToRssUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Sources_Name_RSSUrl",
                table: "Sources");

            migrationBuilder.RenameColumn(
                name: "RSSUrl",
                table: "Sources",
                newName: "RssUrl");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Sources_Name_RssUrl",
                table: "Sources",
                columns: new[] { "Name", "RssUrl" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Sources_Name_RssUrl",
                table: "Sources");

            migrationBuilder.RenameColumn(
                name: "RssUrl",
                table: "Sources",
                newName: "RSSUrl");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Sources_Name_RSSUrl",
                table: "Sources",
                columns: new[] { "Name", "RSSUrl" });
        }
    }
}
