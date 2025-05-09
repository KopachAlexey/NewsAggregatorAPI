using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsAggregatorData.Migrations
{
    /// <inheritdoc />
    public partial class NewsRatingMayBeNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "PositivityRate",
                table: "News",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldDefaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "PositivityRate",
                table: "News",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
        }
    }
}
