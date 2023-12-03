using System;
using Dumbogram.Models.Files;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dumbogram.Database.Migrations.Application
{
    /// <inheritdoc />
    public partial class AddedmetainformationtoFilesGroupandaddedmetadataforeachfiletype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "duration",
                table: "file");

            migrationBuilder.DropColumn(
                name: "height",
                table: "file");

            migrationBuilder.DropColumn(
                name: "width",
                table: "file");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:files_group_type", "avatars,attached_photos,attached_videos,attached_documents")
                .Annotation("Npgsql:Enum:system_message_type", "user_joined,user_left,chat_created,chat_description_edited,chat_title_edited")
                .OldAnnotation("Npgsql:Enum:system_message_type", "user_joined,user_left,chat_created,chat_description_edited,chat_title_edited");

            migrationBuilder.AddColumn<FilesGroupType>(
                name: "group_type",
                table: "files_groups",
                type: "files_group_type",
                nullable: false,
                defaultValue: FilesGroupType.Avatars);

            migrationBuilder.AddColumn<Guid>(
                name: "owner_id",
                table: "files_groups",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<long>(
                name: "file_size",
                table: "file",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

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
                name: "photo_metadata",
                table: "file",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "video_metadata",
                table: "file",
                type: "jsonb",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_files_groups_owner_id",
                table: "files_groups",
                column: "owner_id");

            migrationBuilder.AddForeignKey(
                name: "fk_files_groups_user_profiles_owner_id",
                table: "files_groups",
                column: "owner_id",
                principalTable: "user_profiles",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_files_groups_user_profiles_owner_id",
                table: "files_groups");

            migrationBuilder.DropIndex(
                name: "ix_files_groups_owner_id",
                table: "files_groups");

            migrationBuilder.DropColumn(
                name: "group_type",
                table: "files_groups");

            migrationBuilder.DropColumn(
                name: "owner_id",
                table: "files_groups");

            migrationBuilder.DropColumn(
                name: "animation_metadata",
                table: "file");

            migrationBuilder.DropColumn(
                name: "document_metadata",
                table: "file");

            migrationBuilder.DropColumn(
                name: "photo_metadata",
                table: "file");

            migrationBuilder.DropColumn(
                name: "video_metadata",
                table: "file");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:system_message_type", "user_joined,user_left,chat_created,chat_description_edited,chat_title_edited")
                .OldAnnotation("Npgsql:Enum:files_group_type", "avatars,attached_photos,attached_videos,attached_documents")
                .OldAnnotation("Npgsql:Enum:system_message_type", "user_joined,user_left,chat_created,chat_description_edited,chat_title_edited");

            migrationBuilder.AlterColumn<int>(
                name: "file_size",
                table: "file",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "duration",
                table: "file",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "height",
                table: "file",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "width",
                table: "file",
                type: "integer",
                nullable: true);
        }
    }
}
