using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsAggregatorData.Migrations
{
    /// <inheritdoc />
    public partial class AddNewsRateForUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ValidRate",
                table: "News");

            migrationBuilder.AddColumn<double>(
                name: "NewsMinRate",
                table: "Users",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddCheckConstraint(
                name: "ValidNewsMinRate",
                table: "Users",
                sql: "NewsMinRate >= -5 AND NewsMinRate <= 5");

            migrationBuilder.AddCheckConstraint(
                name: "ValidRate",
                table: "News",
                sql: "PositivityRate >= -5 AND PositivityRate <= 5");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ValidNewsMinRate",
                table: "Users");

            migrationBuilder.DropCheckConstraint(
                name: "ValidRate",
                table: "News");

            migrationBuilder.DropColumn(
                name: "NewsMinRate",
                table: "Users");

            migrationBuilder.AddCheckConstraint(
                name: "ValidRate",
                table: "News",
                sql: "PositivityRate >= -10 AND PositivityRate <= 10");
        }
    }
}
