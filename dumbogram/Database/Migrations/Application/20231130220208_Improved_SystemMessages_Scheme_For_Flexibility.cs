using Dumbogram.Models.Messages.SystemMessages;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dumbogram.Database.Migrations.Application
{
    /// <inheritdoc />
    public partial class Improved_SystemMessages_Scheme_For_Flexibility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "new_description",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "new_title",
                table: "messages");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:system_message_type", "user_joined,user_left,chat_description_edited,chat_title_edited");

            migrationBuilder.AlterColumn<string>(
                name: "discriminator",
                table: "messages",
                type: "character varying(21)",
                maxLength: 21,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "SystemMessageDetails",
                table: "messages",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<SystemMessageType>(
                name: "system_message_type",
                table: "messages",
                type: "system_message_type",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SystemMessageDetails",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "system_message_type",
                table: "messages");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:system_message_type", "user_joined,user_left,chat_description_edited,chat_title_edited");

            migrationBuilder.AlterColumn<string>(
                name: "discriminator",
                table: "messages",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(21)",
                oldMaxLength: 21);

            migrationBuilder.AddColumn<string>(
                name: "new_description",
                table: "messages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "new_title",
                table: "messages",
                type: "text",
                nullable: true);
        }
    }
}
