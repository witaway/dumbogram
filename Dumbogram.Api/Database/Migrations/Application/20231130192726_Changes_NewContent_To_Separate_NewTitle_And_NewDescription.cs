#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Dumbogram.Api.Database.Migrations.Application
{
    /// <inheritdoc />
    public partial class Changes_NewContent_To_Separate_NewTitle_And_NewDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "new_content",
                table: "messages",
                newName: "new_title");

            migrationBuilder.RenameColumn(
                name: "edited_title_system_message_new_content",
                table: "messages",
                newName: "new_description");

            migrationBuilder.AddColumn<Guid>(
                name: "subject_profile_user_id",
                table: "regular_user_messages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_regular_user_messages_subject_profile_user_id",
                table: "regular_user_messages",
                column: "subject_profile_user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_regular_user_messages_chats_chat_id",
                table: "regular_user_messages",
                column: "chat_id",
                principalTable: "chats",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_regular_user_messages_user_profiles_subject_profile_user_id",
                table: "regular_user_messages",
                column: "subject_profile_user_id",
                principalTable: "user_profiles",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_regular_user_messages_chats_chat_id",
                table: "regular_user_messages");

            migrationBuilder.DropForeignKey(
                name: "fk_regular_user_messages_user_profiles_subject_profile_user_id",
                table: "regular_user_messages");

            migrationBuilder.DropIndex(
                name: "ix_regular_user_messages_subject_profile_user_id",
                table: "regular_user_messages");

            migrationBuilder.DropColumn(
                name: "subject_profile_user_id",
                table: "regular_user_messages");

            migrationBuilder.RenameColumn(
                name: "new_title",
                table: "messages",
                newName: "new_content");

            migrationBuilder.RenameColumn(
                name: "new_description",
                table: "messages",
                newName: "edited_title_system_message_new_content");
        }
    }
}
