using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dumbogram.Database.Migrations.Application
{
    /// <inheritdoc />
    public partial class Added_ChatVisibility_to_Chat_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "chat_visibility",
                table: "chats",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "chat_visibility",
                table: "chats");
        }
    }
}
