#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Dumbogram.Api.Database.Migrations.Application
{
    /// <inheritdoc />
    public partial class Made_SenderId_Optional_In_General_But_Required_For_UserMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_messages_user_profiles_subject_id",
                table: "messages");

            migrationBuilder.DropIndex(
                name: "ix_messages_subject_id",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "subject_id",
                table: "messages");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:system_message_type", "user_joined,user_left,chat_created,chat_description_edited,chat_title_edited")
                .OldAnnotation("Npgsql:Enum:system_message_type", "user_joined,user_left,chat_description_edited,chat_title_edited");

            migrationBuilder.AddColumn<Guid>(
                name: "sender_id",
                table: "messages",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_messages_sender_id",
                table: "messages",
                column: "sender_id");

            migrationBuilder.AddForeignKey(
                name: "fk_messages_user_profiles_sender_id",
                table: "messages",
                column: "sender_id",
                principalTable: "user_profiles",
                principalColumn: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_messages_user_profiles_sender_id",
                table: "messages");

            migrationBuilder.DropIndex(
                name: "ix_messages_sender_id",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "sender_id",
                table: "messages");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:system_message_type", "user_joined,user_left,chat_description_edited,chat_title_edited")
                .OldAnnotation("Npgsql:Enum:system_message_type", "user_joined,user_left,chat_created,chat_description_edited,chat_title_edited");

            migrationBuilder.AddColumn<Guid>(
                name: "subject_id",
                table: "messages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_messages_subject_id",
                table: "messages",
                column: "subject_id");

            migrationBuilder.AddForeignKey(
                name: "fk_messages_user_profiles_subject_id",
                table: "messages",
                column: "subject_id",
                principalTable: "user_profiles",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
