using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaContable.API.Migrations
{
    /// <inheritdoc />
    public partial class variosTeaks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_hidden",
                schema: "dbo",
                table: "journal_entries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_blocked",
                schema: "dbo",
                table: "accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_hidden",
                schema: "dbo",
                table: "journal_entries");

            migrationBuilder.DropColumn(
                name: "is_blocked",
                schema: "dbo",
                table: "accounts");
        }
    }
}
