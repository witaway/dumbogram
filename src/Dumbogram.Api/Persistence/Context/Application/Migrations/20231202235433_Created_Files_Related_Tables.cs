#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Dumbogram.Api.Database.Migrations.Application
{
    /// <inheritdoc />
    public partial class Created_Files_Related_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "files_groups",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    deleted_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_files_groups", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "file",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    files_group_id = table.Column<Guid>(type: "uuid", nullable: true),
                    original_filename = table.Column<string>(type: "text", nullable: true),
                    stored_filename = table.Column<string>(type: "text", nullable: false),
                    mime_type = table.Column<string>(type: "text", nullable: false),
                    file_size = table.Column<int>(type: "integer", nullable: false),
                    file_type = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    width = table.Column<int>(type: "integer", nullable: true),
                    height = table.Column<int>(type: "integer", nullable: true),
                    duration = table.Column<int>(type: "integer", nullable: true),
                    deleted_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_file", x => x.id);
                    table.ForeignKey(
                        name: "fk_file_files_groups_files_group_id",
                        column: x => x.files_group_id,
                        principalTable: "files_groups",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_file_files_group_id",
                table: "file",
                column: "files_group_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "file");

            migrationBuilder.DropTable(
                name: "files_groups");
        }
    }
}
