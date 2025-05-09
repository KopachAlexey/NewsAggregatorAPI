using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsAggregatorData.Migrations
{
    /// <inheritdoc />
    public partial class ReactionNameLenth30 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ReactionName",
                table: "Reactions",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ReactionName",
                table: "Reactions",
                type: "nvarchar",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);
        }
    }
}
