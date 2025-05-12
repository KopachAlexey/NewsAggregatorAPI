using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsAggregatorData.Migrations
{
    /// <inheritdoc />
    public partial class RenameReactionNameToName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Reactions_ReactionName",
                table: "Reactions");

            migrationBuilder.RenameColumn(
                name: "ReactionName",
                table: "Reactions",
                newName: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Reactions_Name",
                table: "Reactions",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Reactions_Name",
                table: "Reactions");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Reactions",
                newName: "ReactionName");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Reactions_ReactionName",
                table: "Reactions",
                column: "ReactionName");
        }
    }
}
