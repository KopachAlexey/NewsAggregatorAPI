using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsAggregatorData.Migrations
{
    /// <inheritdoc />
    public partial class RoleNoDefaultValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RoleName",
                table: "Roles",
                type: "int",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 50,
                oldDefaultValue: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RoleName",
                table: "Roles",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 3,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 50);
        }
    }
}
