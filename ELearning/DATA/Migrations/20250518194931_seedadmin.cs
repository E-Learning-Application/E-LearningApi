using DATA.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATA.Migrations
{
    /// <inheritdoc />
    public partial class seedadmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var hasher = new PasswordHasher<AppUser>();
            var admin = new AppUser
            {
                Id = 1,
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = Guid.NewGuid().ToString("D"),
            };

            admin.PasswordHash = hasher.HashPassword(admin, "Admin#2002");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[]
                {
            "Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail",
            "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp",
            "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnabled",
            "AccessFailedCount", "IsDeleted"
                },
                values: new object[]
                {
            admin.Id, admin.UserName, admin.NormalizedUserName, admin.Email, admin.NormalizedEmail,
            admin.EmailConfirmed, admin.PasswordHash, admin.SecurityStamp, admin.ConcurrencyStamp,
            false, false, false, 0, false
                }
            );

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { admin.Id, 2 }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
