using CORE.Constants;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATA.Migrations
{
    /// <inheritdoc />
    public partial class roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[,]
                {
                                { Roles.User.ToString(), Roles.User.ToString().ToUpper(), Guid.NewGuid().ToString() },
                                { Roles.Admin.ToString(), Roles.Admin.ToString().ToUpper(), Guid.NewGuid().ToString() },
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [AspNetRoles]");
        }
    }
}
