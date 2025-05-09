using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsAggregatorData.Migrations
{
    /// <inheritdoc />
    public partial class AddReactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReactionName = table.Column<string>(type: "nvarchar", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactions", x => x.Id);
                    table.UniqueConstraint("AK_Reactions_ReactionName", x => x.ReactionName);
                });

            migrationBuilder.CreateTable(
                name: "UserCommentReactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCommentReactions", x => x.Id);
                    table.UniqueConstraint("AK_UserCommentReactions_UserId_CommentId", x => new { x.UserId, x.CommentId });
                    table.ForeignKey(
                        name: "FK_UserCommentReactions_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserCommentReactions_Reactions_ReactionId",
                        column: x => x.ReactionId,
                        principalTable: "Reactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserCommentReactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCommentReactions_CommentId",
                table: "UserCommentReactions",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCommentReactions_ReactionId",
                table: "UserCommentReactions",
                column: "ReactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCommentReactions");

            migrationBuilder.DropTable(
                name: "Reactions");
        }
    }
}
