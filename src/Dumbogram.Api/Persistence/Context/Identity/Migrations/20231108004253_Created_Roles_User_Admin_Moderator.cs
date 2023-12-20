#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dumbogram.Api.Database.Migrations.Identity
{
    /// <inheritdoc />
    public partial class Created_Roles_User_Admin_Moderator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "id", "concurrency_stamp", "name", "normalized_name" },
                values: new object[,]
                {
                    { "7bc57da2-d3e4-40d9-bee4-b61ad4ed9914", null, "Admin", null },
                    { "7cf999e8-b8c2-4c4e-8bfd-8741b6cc6430", null, "User", null },
                    { "8dfc16f6-2e30-4b18-8f07-0f9bcae0b401", null, "Moderator", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "7bc57da2-d3e4-40d9-bee4-b61ad4ed9914");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "7cf999e8-b8c2-4c4e-8bfd-8741b6cc6430");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "8dfc16f6-2e30-4b18-8f07-0f9bcae0b401");
        }
    }
}
