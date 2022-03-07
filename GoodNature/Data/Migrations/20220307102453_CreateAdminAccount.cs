using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Text;

namespace GoodNature.Data.Migrations
{
    public partial class CreateAdminAccount : Migration
    {
        readonly string AdminUserGUID = "<insert here>";
        readonly string AdminRoleGUID = "<insert here>";
        const string AdminEmail = "<insert here>";

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            var passwordHash = hasher.HashPassword(null, "<insert here>");

            string insertUser = $@"INSERT INTO AspNetUsers (Id,
                                                            UserName,
                                                            NormalizedUserName,
                                                            Email,
                                                            EmailConfirmed,
                                                            PhoneNumberConfirmed,
                                                            TwoFactorEnabled,
                                                            LockoutEnabled,
                                                            AccessFailedCount,
                                                            NormalizedEmail,
                                                            PasswordHash,
                                                            SecurityStamp,
                                                            FirstName)

                                                    VALUES ('{AdminUserGUID}',
                                                            '{AdminEmail}',
                                                            '{AdminEmail.ToUpper()}',
                                                            '{AdminEmail}',
                                                            0,
                                                            0,
                                                            0,
                                                            0,
                                                            0,
                                                            '{AdminEmail.ToUpper()}',
                                                            '{passwordHash}',
                                                            '',
                                                            'Admin')";

            migrationBuilder.Sql(insertUser);

            migrationBuilder.Sql($"INSERT INTO AspNetRoles (Id, Name, NormalizedName) VALUES ('{AdminRoleGUID}', 'Admin', 'ADMIN')");

            migrationBuilder.Sql($"INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES ('{AdminUserGUID}', '{AdminRoleGUID}')");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM AspNetUserRoles WHERE UserId = '{AdminUserGUID}' AND RoleId = '{AdminRoleGUID}'");

            migrationBuilder.Sql($"DELETE FROM AspNetUsers WHERE Id = '{AdminUserGUID}'");

            migrationBuilder.Sql($"DELETE FROM AspNetRoles WHERE Id = '{AdminRoleGUID}'");
        }
    }
}
