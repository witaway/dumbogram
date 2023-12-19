#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Dumbogram.Api.Database.Migrations.Application;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "user_profiles",
            table => new
            {
                user_id = table.Column<Guid>("uuid", nullable: false),
                username = table.Column<string>("character varying(32)", maxLength: 32, nullable: false),
                description = table.Column<string>("character varying(256)", maxLength: 256, nullable: false),
                avatar_media_id = table.Column<Guid>("uuid", nullable: false),
                created_date = table.Column<DateTime>("timestamp with time zone", nullable: false),
                updated_date = table.Column<DateTime>("timestamp with time zone", nullable: false)
            },
            constraints: table => { table.PrimaryKey("pk_user_profiles", x => x.user_id); });

        migrationBuilder.CreateIndex(
            "ix_user_profiles_username",
            "user_profiles",
            "username",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "user_profiles");
    }
}