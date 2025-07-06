using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATA.Migrations
{
    /// <inheritdoc />
    public partial class feedbackrelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_AspNetUsers_UserId",
                table: "Feedbacks");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Feedbacks",
                newName: "FeedbackerId");

            migrationBuilder.RenameIndex(
                name: "IX_Feedbacks_UserId",
                table: "Feedbacks",
                newName: "IX_Feedbacks_FeedbackerId");

            migrationBuilder.AddColumn<int>(
                name: "FeedbackedId",
                table: "Feedbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_FeedbackedId",
                table: "Feedbacks",
                column: "FeedbackedId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Feedback_NoSelfFeedback",
                table: "Feedbacks",
                sql: "FeedbackerId != FeedbackedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_AspNetUsers_FeedbackedId",
                table: "Feedbacks",
                column: "FeedbackedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_AspNetUsers_FeedbackerId",
                table: "Feedbacks",
                column: "FeedbackerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_AspNetUsers_FeedbackedId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_AspNetUsers_FeedbackerId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_FeedbackedId",
                table: "Feedbacks");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Feedback_NoSelfFeedback",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "FeedbackedId",
                table: "Feedbacks");

            migrationBuilder.RenameColumn(
                name: "FeedbackerId",
                table: "Feedbacks",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Feedbacks_FeedbackerId",
                table: "Feedbacks",
                newName: "IX_Feedbacks_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_AspNetUsers_UserId",
                table: "Feedbacks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
