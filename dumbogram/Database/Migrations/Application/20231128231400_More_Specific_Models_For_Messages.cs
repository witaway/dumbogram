using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Dumbogram.Database.Migrations.Application
{
    /// <inheritdoc />
    public partial class More_Specific_Models_For_Messages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chat_messages");

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    chat_id = table.Column<Guid>(type: "uuid", nullable: false),
                    subject_id = table.Column<Guid>(type: "uuid", nullable: false),
                    discriminator = table.Column<string>(type: "text", nullable: false),
                    new_content = table.Column<string>(type: "text", nullable: true),
                    edited_title_system_message_new_content = table.Column<string>(type: "text", nullable: true),
                    forwarded_chat_id = table.Column<Guid>(type: "uuid", nullable: true),
                    forwarded_message_id = table.Column<int>(type: "integer", nullable: true),
                    regular_user_message_id = table.Column<int>(type: "integer", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_messages", x => new { x.chat_id, x.id });
                    table.ForeignKey(
                        name: "fk_messages_chats_chat_id",
                        column: x => x.chat_id,
                        principalTable: "chats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_messages_messages_forwarded_chat_id_forwarded_message_id",
                        columns: x => new { x.forwarded_chat_id, x.forwarded_message_id },
                        principalTable: "messages",
                        principalColumns: new[] { "chat_id", "id" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_messages_user_profiles_subject_id",
                        column: x => x.subject_id,
                        principalTable: "user_profiles",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "regular_user_messages",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    content = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    replied_message_id = table.Column<int>(type: "integer", nullable: true),
                    regular_user_message_id = table.Column<int>(type: "integer", nullable: true),
                    chat_id = table.Column<Guid>(type: "uuid", nullable: false),
                    subject_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_regular_user_messages", x => x.id);
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
                });

            migrationBuilder.CreateIndex(
                name: "ix_messages_chat_id",
                table: "messages",
                column: "chat_id");

            migrationBuilder.CreateIndex(
                name: "ix_messages_created_date",
                table: "messages",
                column: "created_date");

            migrationBuilder.CreateIndex(
                name: "ix_messages_forwarded_chat_id_forwarded_message_id",
                table: "messages",
                columns: new[] { "forwarded_chat_id", "forwarded_message_id" });

            migrationBuilder.CreateIndex(
                name: "ix_messages_regular_user_message_id",
                table: "messages",
                column: "regular_user_message_id");

            migrationBuilder.CreateIndex(
                name: "ix_messages_subject_id",
                table: "messages",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "ix_regular_user_messages_chat_id_replied_message_id",
                table: "regular_user_messages",
                columns: new[] { "chat_id", "replied_message_id" });

            migrationBuilder.CreateIndex(
                name: "ix_regular_user_messages_regular_user_message_id",
                table: "regular_user_messages",
                column: "regular_user_message_id");

            migrationBuilder.AddForeignKey(
                name: "fk_messages_regular_user_messages_regular_user_message_id",
                table: "messages",
                column: "regular_user_message_id",
                principalTable: "regular_user_messages",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_messages_regular_user_messages_regular_user_message_id",
                table: "messages");

            migrationBuilder.DropTable(
                name: "regular_user_messages");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.CreateTable(
                name: "chat_messages",
                columns: table => new
                {
                    chat_id = table.Column<Guid>(type: "uuid", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false),
                    sender_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    message = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    message_type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_chat_messages", x => new { x.chat_id, x.id });
                    table.ForeignKey(
                        name: "fk_chat_messages_chats_chat_id",
                        column: x => x.chat_id,
                        principalTable: "chats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_chat_messages_user_profiles_sender_id",
                        column: x => x.sender_id,
                        principalTable: "user_profiles",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_chat_messages_chat_id",
                table: "chat_messages",
                column: "chat_id");

            migrationBuilder.CreateIndex(
                name: "ix_chat_messages_created_date",
                table: "chat_messages",
                column: "created_date");

            migrationBuilder.CreateIndex(
                name: "ix_chat_messages_sender_id",
                table: "chat_messages",
                column: "sender_id");
        }
    }
}
