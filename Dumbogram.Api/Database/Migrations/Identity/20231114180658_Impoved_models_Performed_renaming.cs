#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dumbogram.Api.Database.Migrations.Identity
{
    /// <inheritdoc />
    public partial class Impoved_models_Performed_renaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "id", "concurrency_stamp", "name", "normalized_name" },
                values: new object[,]
                {
                    { "007906fb-d4b4-4ab7-9048-e0716d7444e5", null, "Admin", null },
                    { "250fe6d6-697e-4a66-bdcb-5f472b93a342", null, "Moderator", null },
                    { "df5b45bf-7d84-416a-9aaa-6c907c001a60", null, "User", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "007906fb-d4b4-4ab7-9048-e0716d7444e5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "250fe6d6-697e-4a66-bdcb-5f472b93a342");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "df5b45bf-7d84-416a-9aaa-6c907c001a60");

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
    }
}
