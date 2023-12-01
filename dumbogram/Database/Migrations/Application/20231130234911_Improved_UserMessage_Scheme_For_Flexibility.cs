using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Dumbogram.Database.Migrations.Application
{
    /// <inheritdoc />
    public partial class Improved_UserMessage_Scheme_For_Flexibility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_messages_messages_forwarded_chat_id_forwarded_message_id",
                table: "messages");

            migrationBuilder.DropForeignKey(
                name: "fk_messages_regular_user_messages_regular_user_message_id",
                table: "messages");

            migrationBuilder.DropTable(
                name: "regular_user_messages");

            migrationBuilder.DropIndex(
                name: "ix_messages_forwarded_chat_id_forwarded_message_id",
                table: "messages");

            migrationBuilder.DropIndex(
                name: "ix_messages_regular_user_message_id",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "forwarded_chat_id",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "forwarded_message_id",
                table: "messages");

            migrationBuilder.RenameColumn(
                name: "regular_user_message_id",
                table: "messages",
                newName: "replied_message_id");

            migrationBuilder.RenameColumn(
                name: "discriminator",
                table: "messages",
                newName: "message_type");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "messages",
                type: "jsonb",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_messages_chat_id_replied_message_id",
                table: "messages",
                columns: new[] { "chat_id", "replied_message_id" });

            migrationBuilder.AddForeignKey(
                name: "fk_messages_messages_chat_id_replied_message_id",
                table: "messages",
                columns: new[] { "chat_id", "replied_message_id" },
                principalTable: "messages",
                principalColumns: new[] { "chat_id", "id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_messages_messages_chat_id_replied_message_id",
                table: "messages");

            migrationBuilder.DropIndex(
                name: "ix_messages_chat_id_replied_message_id",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "messages");

            migrationBuilder.RenameColumn(
                name: "replied_message_id",
                table: "messages",
                newName: "regular_user_message_id");

            migrationBuilder.RenameColumn(
                name: "message_type",
                table: "messages",
                newName: "discriminator");

            migrationBuilder.AddColumn<Guid>(
                name: "forwarded_chat_id",
                table: "messages",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "forwarded_message_id",
                table: "messages",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "regular_user_messages",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    chat_id = table.Column<Guid>(type: "uuid", nullable: false),
                    subject_profile_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    replied_message_id = table.Column<int>(type: "integer", nullable: true),
                    content = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    regular_user_message_id = table.Column<int>(type: "integer", nullable: true),
                    subject_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_regular_user_messages", x => x.id);
                    table.ForeignKey(
                        name: "fk_regular_user_messages_chats_chat_id",
                        column: x => x.chat_id,
                        principalTable: "chats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_regular_user_messages_messages_chat_id_replied_message_id",
                        columns: x => new { x.chat_id, x.replied_message_id },
                        principalTable: "messages",
                        principalColumns: new[] { "chat_id", "id" });
                    table.ForeignKey(
                        name: "fk_regular_user_messages_regular_user_messages_regular_user_me",
                        column: x => x.regular_user_message_id,
                        principalTable: "regular_user_messages",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_regular_user_messages_user_profiles_subject_profile_user_id",
                        column: x => x.subject_profile_user_id,
                        principalTable: "user_profiles",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_messages_forwarded_chat_id_forwarded_message_id",
                table: "messages",
                columns: new[] { "forwarded_chat_id", "forwarded_message_id" });

            migrationBuilder.CreateIndex(
                name: "ix_messages_regular_user_message_id",
                table: "messages",
                column: "regular_user_message_id");

            migrationBuilder.CreateIndex(
                name: "ix_regular_user_messages_chat_id_replied_message_id",
                table: "regular_user_messages",
                columns: new[] { "chat_id", "replied_message_id" });

            migrationBuilder.CreateIndex(
                name: "ix_regular_user_messages_regular_user_message_id",
                table: "regular_user_messages",
                column: "regular_user_message_id");

            migrationBuilder.CreateIndex(
                name: "ix_regular_user_messages_subject_profile_user_id",
                table: "regular_user_messages",
                column: "subject_profile_user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_messages_messages_forwarded_chat_id_forwarded_message_id",
                table: "messages",
                columns: new[] { "forwarded_chat_id", "forwarded_message_id" },
                principalTable: "messages",
                principalColumns: new[] { "chat_id", "id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_messages_regular_user_messages_regular_user_message_id",
                table: "messages",
                column: "regular_user_message_id",
                principalTable: "regular_user_messages",
                principalColumn: "id");
        }
    }
}
