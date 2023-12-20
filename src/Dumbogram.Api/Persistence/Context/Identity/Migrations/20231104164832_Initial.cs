﻿#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Dumbogram.Api.Database.Migrations.Identity;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "AspNetRoles",
            table => new
            {
                id = table.Column<string>("text", nullable: false),
                name = table.Column<string>("character varying(256)", maxLength: 256, nullable: true),
                normalized_name = table.Column<string>("character varying(256)", maxLength: 256, nullable: true),
                concurrency_stamp = table.Column<string>("text", nullable: true)
            },
            constraints: table => { table.PrimaryKey("pk_asp_net_roles", x => x.id); });

        migrationBuilder.CreateTable(
            "AspNetUsers",
            table => new
            {
                id = table.Column<string>("text", nullable: false),
                user_name = table.Column<string>("character varying(256)", maxLength: 256, nullable: true),
                normalized_user_name = table.Column<string>("character varying(256)", maxLength: 256, nullable: true),
                email = table.Column<string>("character varying(256)", maxLength: 256, nullable: true),
                normalized_email = table.Column<string>("character varying(256)", maxLength: 256, nullable: true),
                email_confirmed = table.Column<bool>("boolean", nullable: false),
                password_hash = table.Column<string>("text", nullable: true),
                security_stamp = table.Column<string>("text", nullable: true),
                concurrency_stamp = table.Column<string>("text", nullable: true),
                phone_number = table.Column<string>("text", nullable: true),
                phone_number_confirmed = table.Column<bool>("boolean", nullable: false),
                two_factor_enabled = table.Column<bool>("boolean", nullable: false),
                lockout_end = table.Column<DateTimeOffset>("timestamp with time zone", nullable: true),
                lockout_enabled = table.Column<bool>("boolean", nullable: false),
                access_failed_count = table.Column<int>("integer", nullable: false)
            },
            constraints: table => { table.PrimaryKey("pk_asp_net_users", x => x.id); });

        migrationBuilder.CreateTable(
            "AspNetRoleClaims",
            table => new
            {
                id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                role_id = table.Column<string>("text", nullable: false),
                claim_type = table.Column<string>("text", nullable: true),
                claim_value = table.Column<string>("text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_asp_net_role_claims", x => x.id);
                table.ForeignKey(
                    "fk_asp_net_role_claims_asp_net_roles_role_id",
                    x => x.role_id,
                    "AspNetRoles",
                    "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "AspNetUserClaims",
            table => new
            {
                id = table.Column<int>("integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy",
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                user_id = table.Column<string>("text", nullable: false),
                claim_type = table.Column<string>("text", nullable: true),
                claim_value = table.Column<string>("text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_asp_net_user_claims", x => x.id);
                table.ForeignKey(
                    "fk_asp_net_user_claims_asp_net_users_user_id",
                    x => x.user_id,
                    "AspNetUsers",
                    "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "AspNetUserLogins",
            table => new
            {
                login_provider = table.Column<string>("text", nullable: false),
                provider_key = table.Column<string>("text", nullable: false),
                provider_display_name = table.Column<string>("text", nullable: true),
                user_id = table.Column<string>("text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_asp_net_user_logins", x => new { x.login_provider, x.provider_key });
                table.ForeignKey(
                    "fk_asp_net_user_logins_asp_net_users_user_id",
                    x => x.user_id,
                    "AspNetUsers",
                    "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "AspNetUserRoles",
            table => new
            {
                user_id = table.Column<string>("text", nullable: false),
                role_id = table.Column<string>("text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_asp_net_user_roles", x => new { x.user_id, x.role_id });
                table.ForeignKey(
                    "fk_asp_net_user_roles_asp_net_roles_role_id",
                    x => x.role_id,
                    "AspNetRoles",
                    "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    "fk_asp_net_user_roles_asp_net_users_user_id",
                    x => x.user_id,
                    "AspNetUsers",
                    "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            "AspNetUserTokens",
            table => new
            {
                user_id = table.Column<string>("text", nullable: false),
                login_provider = table.Column<string>("text", nullable: false),
                name = table.Column<string>("text", nullable: false),
                value = table.Column<string>("text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_asp_net_user_tokens", x => new { x.user_id, x.login_provider, x.name });
                table.ForeignKey(
                    "fk_asp_net_user_tokens_asp_net_users_user_id",
                    x => x.user_id,
                    "AspNetUsers",
                    "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            "ix_asp_net_role_claims_role_id",
            "AspNetRoleClaims",
            "role_id");

        migrationBuilder.CreateIndex(
            "RoleNameIndex",
            "AspNetRoles",
            "normalized_name",
            unique: true);

        migrationBuilder.CreateIndex(
            "ix_asp_net_user_claims_user_id",
            "AspNetUserClaims",
            "user_id");

        migrationBuilder.CreateIndex(
            "ix_asp_net_user_logins_user_id",
            "AspNetUserLogins",
            "user_id");

        migrationBuilder.CreateIndex(
            "ix_asp_net_user_roles_role_id",
            "AspNetUserRoles",
            "role_id");

        migrationBuilder.CreateIndex(
            "EmailIndex",
            "AspNetUsers",
            "normalized_email");

        migrationBuilder.CreateIndex(
            "UserNameIndex",
            "AspNetUsers",
            "normalized_user_name",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "AspNetRoleClaims");

        migrationBuilder.DropTable(
            "AspNetUserClaims");

        migrationBuilder.DropTable(
            "AspNetUserLogins");

        migrationBuilder.DropTable(
            "AspNetUserRoles");

        migrationBuilder.DropTable(
            "AspNetUserTokens");

        migrationBuilder.DropTable(
            "AspNetRoles");

        migrationBuilder.DropTable(
            "AspNetUsers");
    }
}