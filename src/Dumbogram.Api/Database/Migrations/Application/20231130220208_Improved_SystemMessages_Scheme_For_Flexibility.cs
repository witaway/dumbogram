#nullable disable

using Dumbogram.Api.Models.Messages;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dumbogram.Api.Database.Migrations.Application;

/// <inheritdoc />
public partial class Improved_SystemMessages_Scheme_For_Flexibility : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            "new_description",
            "messages");

        migrationBuilder.DropColumn(
            "new_title",
            "messages");

        migrationBuilder.AlterDatabase()
            .Annotation("Npgsql:Enum:system_message_type",
                "user_joined,user_left,chat_description_edited,chat_title_edited");

        migrationBuilder.AlterColumn<string>(
            "discriminator",
            "messages",
            "character varying(21)",
            maxLength: 21,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text");

        migrationBuilder.AddColumn<string>(
            "SystemMessageDetails",
            "messages",
            "jsonb",
            nullable: true);

        migrationBuilder.AddColumn<SystemMessageType>(
            "system_message_type",
            "messages",
            "system_message_type",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            "SystemMessageDetails",
            "messages");

        migrationBuilder.DropColumn(
            "system_message_type",
            "messages");

        migrationBuilder.AlterDatabase()
            .OldAnnotation("Npgsql:Enum:system_message_type",
                "user_joined,user_left,chat_description_edited,chat_title_edited");

        migrationBuilder.AlterColumn<string>(
            "discriminator",
            "messages",
            "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(21)",
            oldMaxLength: 21);

        migrationBuilder.AddColumn<string>(
            "new_description",
            "messages",
            "text",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            "new_title",
            "messages",
            "text",
            nullable: true);
    }
}