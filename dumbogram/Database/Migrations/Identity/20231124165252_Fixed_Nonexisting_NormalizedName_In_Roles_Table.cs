using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dumbogram.Database.Migrations.Identity
{
    /// <inheritdoc />
    public partial class Fixed_Nonexisting_NormalizedName_In_Roles_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { "865fa393-ba0d-4dc7-8e1b-0bba7d40457c", null, "Admin", "ADMIN" },
                    { "bc4956d6-4a89-4a5a-a31e-84c5ec9f423e", null, "Moderator", "MODERATOR" },
                    { "f09b3d19-bbcf-4264-8ee7-c0ed1c10e1dc", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "865fa393-ba0d-4dc7-8e1b-0bba7d40457c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "bc4956d6-4a89-4a5a-a31e-84c5ec9f423e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "f09b3d19-bbcf-4264-8ee7-c0ed1c10e1dc");

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
    }
}
