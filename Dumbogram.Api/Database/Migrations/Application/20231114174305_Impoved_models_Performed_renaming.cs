#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Dumbogram.Api.Database.Migrations.Application
{
    /// <inheritdoc />
    public partial class Impoved_models_Performed_renaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_chat_user_profiles_owner_id",
                table: "chat");

            migrationBuilder.DropForeignKey(
                name: "fk_chat_member_permission_chat_chat_id",
                table: "chat_member_permission");

            migrationBuilder.DropForeignKey(
                name: "fk_chat_member_permission_user_profiles_member_id",
                table: "chat_member_permission");

            migrationBuilder.DropForeignKey(
                name: "fk_chat_membership_chat_chat_id",
                table: "chat_membership");

            migrationBuilder.DropForeignKey(
                name: "fk_chat_membership_user_profiles_member_id",
                table: "chat_membership");

            migrationBuilder.DropForeignKey(
                name: "fk_chat_message_chat_chat_id",
                table: "chat_message");

            migrationBuilder.DropForeignKey(
                name: "fk_chat_message_user_profiles_sender_id",
                table: "chat_message");

            migrationBuilder.DropPrimaryKey(
                name: "pk_chat_message",
                table: "chat_message");

            migrationBuilder.DropPrimaryKey(
                name: "pk_chat_membership",
                table: "chat_membership");

            migrationBuilder.DropPrimaryKey(
                name: "pk_chat_member_permission",
                table: "chat_member_permission");

            migrationBuilder.DropPrimaryKey(
                name: "pk_chat",
                table: "chat");

            migrationBuilder.DropColumn(
                name: "avatar_media_id",
                table: "user_profiles");

            migrationBuilder.DropColumn(
                name: "created_date",
                table: "user_profiles");

            migrationBuilder.DropColumn(
                name: "updated_date",
                table: "user_profiles");

            migrationBuilder.DropColumn(
                name: "updated_date",
                table: "chat_message");

            migrationBuilder.DropColumn(
                name: "created_date",
                table: "chat_membership");

            migrationBuilder.DropColumn(
                name: "updated_date",
                table: "chat_membership");

            migrationBuilder.DropColumn(
                name: "created_date",
                table: "chat_member_permission");

            migrationBuilder.DropColumn(
                name: "updated_date",
                table: "chat_member_permission");

            migrationBuilder.DropColumn(
                name: "created_date",
                table: "chat");

            migrationBuilder.DropColumn(
                name: "updated_date",
                table: "chat");

            migrationBuilder.RenameTable(
                name: "chat_message",
                newName: "chat_messages");

            migrationBuilder.RenameTable(
                name: "chat_membership",
                newName: "chat_memberships");

            migrationBuilder.RenameTable(
                name: "chat_member_permission",
                newName: "chat_member_permissions");

            migrationBuilder.RenameTable(
                name: "chat",
                newName: "chats");

            migrationBuilder.RenameIndex(
                name: "ix_chat_message_sender_id",
                table: "chat_messages",
                newName: "ix_chat_messages_sender_id");

            migrationBuilder.RenameIndex(
                name: "ix_chat_message_created_date",
                table: "chat_messages",
                newName: "ix_chat_messages_created_date");

            migrationBuilder.RenameIndex(
                name: "ix_chat_message_chat_id",
                table: "chat_messages",
                newName: "ix_chat_messages_chat_id");

            migrationBuilder.RenameIndex(
                name: "ix_chat_membership_member_id",
                table: "chat_memberships",
                newName: "ix_chat_memberships_member_id");

            migrationBuilder.RenameIndex(
                name: "ix_chat_member_permission_member_id",
                table: "chat_member_permissions",
                newName: "ix_chat_member_permissions_member_id");

            migrationBuilder.RenameIndex(
                name: "ix_chat_owner_id",
                table: "chats",
                newName: "ix_chats_owner_id");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "user_profiles",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            migrationBuilder.AddPrimaryKey(
                name: "pk_chat_messages",
                table: "chat_messages",
                columns: new[] { "chat_id", "id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_chat_memberships",
                table: "chat_memberships",
                columns: new[] { "chat_id", "member_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_chat_member_permissions",
                table: "chat_member_permissions",
                columns: new[] { "chat_id", "member_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_chats",
                table: "chats",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_chat_member_permissions_chats_chat_id",
                table: "chat_member_permissions",
                column: "chat_id",
                principalTable: "chats",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_chat_member_permissions_user_profiles_member_id",
                table: "chat_member_permissions",
                column: "member_id",
                principalTable: "user_profiles",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_chat_memberships_chats_chat_id",
                table: "chat_memberships",
                column: "chat_id",
                principalTable: "chats",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_chat_memberships_user_profiles_member_id",
                table: "chat_memberships",
                column: "member_id",
                principalTable: "user_profiles",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_chat_messages_chats_chat_id",
                table: "chat_messages",
                column: "chat_id",
                principalTable: "chats",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_chat_messages_user_profiles_sender_id",
                table: "chat_messages",
                column: "sender_id",
                principalTable: "user_profiles",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_chats_user_profiles_owner_id",
                table: "chats",
                column: "owner_id",
                principalTable: "user_profiles",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_chat_member_permissions_chats_chat_id",
                table: "chat_member_permissions");

            migrationBuilder.DropForeignKey(
                name: "fk_chat_member_permissions_user_profiles_member_id",
                table: "chat_member_permissions");

            migrationBuilder.DropForeignKey(
                name: "fk_chat_memberships_chats_chat_id",
                table: "chat_memberships");

            migrationBuilder.DropForeignKey(
                name: "fk_chat_memberships_user_profiles_member_id",
                table: "chat_memberships");

            migrationBuilder.DropForeignKey(
                name: "fk_chat_messages_chats_chat_id",
                table: "chat_messages");

            migrationBuilder.DropForeignKey(
                name: "fk_chat_messages_user_profiles_sender_id",
                table: "chat_messages");

            migrationBuilder.DropForeignKey(
                name: "fk_chats_user_profiles_owner_id",
                table: "chats");

            migrationBuilder.DropPrimaryKey(
                name: "pk_chats",
                table: "chats");

            migrationBuilder.DropPrimaryKey(
                name: "pk_chat_messages",
                table: "chat_messages");

            migrationBuilder.DropPrimaryKey(
                name: "pk_chat_memberships",
                table: "chat_memberships");

            migrationBuilder.DropPrimaryKey(
                name: "pk_chat_member_permissions",
                table: "chat_member_permissions");

            migrationBuilder.RenameTable(
                name: "chats",
                newName: "chat");

            migrationBuilder.RenameTable(
                name: "chat_messages",
                newName: "chat_message");

            migrationBuilder.RenameTable(
                name: "chat_memberships",
                newName: "chat_membership");

            migrationBuilder.RenameTable(
                name: "chat_member_permissions",
                newName: "chat_member_permission");

            migrationBuilder.RenameIndex(
                name: "ix_chats_owner_id",
                table: "chat",
                newName: "ix_chat_owner_id");

            migrationBuilder.RenameIndex(
                name: "ix_chat_messages_sender_id",
                table: "chat_message",
                newName: "ix_chat_message_sender_id");

            migrationBuilder.RenameIndex(
                name: "ix_chat_messages_created_date",
                table: "chat_message",
                newName: "ix_chat_message_created_date");

            migrationBuilder.RenameIndex(
                name: "ix_chat_messages_chat_id",
                table: "chat_message",
                newName: "ix_chat_message_chat_id");

            migrationBuilder.RenameIndex(
                name: "ix_chat_memberships_member_id",
                table: "chat_membership",
                newName: "ix_chat_membership_member_id");

            migrationBuilder.RenameIndex(
                name: "ix_chat_member_permissions_member_id",
                table: "chat_member_permission",
                newName: "ix_chat_member_permission_member_id");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "user_profiles",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "avatar_media_id",
                table: "user_profiles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                table: "user_profiles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_date",
                table: "user_profiles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                table: "chat",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_date",
                table: "chat",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_date",
                table: "chat_message",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                table: "chat_membership",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_date",
                table: "chat_membership",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                table: "chat_member_permission",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_date",
                table: "chat_member_permission",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "pk_chat",
                table: "chat",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_chat_message",
                table: "chat_message",
                columns: new[] { "chat_id", "id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_chat_membership",
                table: "chat_membership",
                columns: new[] { "chat_id", "member_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_chat_member_permission",
                table: "chat_member_permission",
                columns: new[] { "chat_id", "member_id" });

            migrationBuilder.AddForeignKey(
                name: "fk_chat_user_profiles_owner_id",
                table: "chat",
                column: "owner_id",
                principalTable: "user_profiles",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_chat_member_permission_chat_chat_id",
                table: "chat_member_permission",
                column: "chat_id",
                principalTable: "chat",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_chat_member_permission_user_profiles_member_id",
                table: "chat_member_permission",
                column: "member_id",
                principalTable: "user_profiles",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_chat_membership_chat_chat_id",
                table: "chat_membership",
                column: "chat_id",
                principalTable: "chat",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_chat_membership_user_profiles_member_id",
                table: "chat_membership",
                column: "member_id",
                principalTable: "user_profiles",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_chat_message_chat_chat_id",
                table: "chat_message",
                column: "chat_id",
                principalTable: "chat",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_chat_message_user_profiles_sender_id",
                table: "chat_message",
                column: "sender_id",
                principalTable: "user_profiles",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
