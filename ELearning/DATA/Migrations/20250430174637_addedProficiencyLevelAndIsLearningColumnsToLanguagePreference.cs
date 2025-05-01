using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATA.Migrations
{
    /// <inheritdoc />
    public partial class addedProficiencyLevelAndIsLearningColumnsToLanguagePreference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLearning",
                table: "LanguagePreferences",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ProficiencyLevel",
                table: "LanguagePreferences",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLearning",
                table: "LanguagePreferences");

            migrationBuilder.DropColumn(
                name: "ProficiencyLevel",
                table: "LanguagePreferences");
        }
    }
}
