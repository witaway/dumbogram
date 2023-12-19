#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Dumbogram.Api.Database.Migrations.Application
{
    /// <inheritdoc />
    public partial class Returned_Missing_SystemMessageDetailsField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SystemMessageDetails",
                table: "messages",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SystemMessageDetails",
                table: "messages");
        }
    }
}
