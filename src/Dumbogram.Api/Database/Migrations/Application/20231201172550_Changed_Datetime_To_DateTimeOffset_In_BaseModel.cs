#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Dumbogram.Api.Database.Migrations.Application
{
    /// <inheritdoc />
    public partial class Changed_Datetime_To_DateTimeOffset_In_BaseModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_date",
                table: "user_profiles",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_date",
                table: "messages",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_date",
                table: "chats",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_date",
                table: "chat_memberships",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_date",
                table: "chat_member_permissions",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deleted_date",
                table: "user_profiles");

            migrationBuilder.DropColumn(
                name: "deleted_date",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "deleted_date",
                table: "chats");

            migrationBuilder.DropColumn(
                name: "deleted_date",
                table: "chat_memberships");

            migrationBuilder.DropColumn(
                name: "deleted_date",
                table: "chat_member_permissions");
        }
    }
}
