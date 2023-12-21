using Dumbogram.Api.Persistence.Context.Application.Enumerations;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dumbogram.Api.Database.Migrations.Application
{
    /// <inheritdoc />
    public partial class Mergedmultiplefilemetadatafieldsintoone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "animation_metadata",
                table: "file");

            migrationBuilder.DropColumn(
                name: "document_metadata",
                table: "file");

            migrationBuilder.DropColumn(
                name: "file_type",
                table: "file");

            migrationBuilder.DropColumn(
                name: "photo_metadata",
                table: "file");

            migrationBuilder.RenameColumn(
                name: "video_metadata",
                table: "file",
                newName: "metadata");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:file_type", "unknown,photo,video,animation,document")
                .Annotation("Npgsql:Enum:files_group_type", "avatars,attached_photos,attached_videos,attached_documents")
                .Annotation("Npgsql:Enum:system_message_type", "user_joined,user_left,chat_created,chat_description_edited,chat_title_edited")
                .OldAnnotation("Npgsql:Enum:files_group_type", "avatars,attached_photos,attached_videos,attached_documents")
                .OldAnnotation("Npgsql:Enum:system_message_type", "user_joined,user_left,chat_created,chat_description_edited,chat_title_edited");

            migrationBuilder.AddColumn<FileType>(
                name: "type",
                table: "file",
                type: "file_type",
                nullable: false,
                defaultValue: FileType.Unknown);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "file");

            migrationBuilder.RenameColumn(
                name: "metadata",
                table: "file",
                newName: "video_metadata");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:files_group_type", "avatars,attached_photos,attached_videos,attached_documents")
                .Annotation("Npgsql:Enum:system_message_type", "user_joined,user_left,chat_created,chat_description_edited,chat_title_edited")
                .OldAnnotation("Npgsql:Enum:file_type", "unknown,photo,video,animation,document")
                .OldAnnotation("Npgsql:Enum:files_group_type", "avatars,attached_photos,attached_videos,attached_documents")
                .OldAnnotation("Npgsql:Enum:system_message_type", "user_joined,user_left,chat_created,chat_description_edited,chat_title_edited");

            migrationBuilder.AddColumn<string>(
                name: "animation_metadata",
                table: "file",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "document_metadata",
                table: "file",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "file_type",
                table: "file",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "photo_metadata",
                table: "file",
                type: "jsonb",
                nullable: true);
        }
    }
}
