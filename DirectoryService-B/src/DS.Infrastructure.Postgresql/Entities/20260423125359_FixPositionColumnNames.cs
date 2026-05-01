using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DS.Infrastructure.Postgresql.Entities
{
    /// <inheritdoc />
    public partial class FixPositionColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "positions",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "positions",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "positions",
                newName: "created_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "positions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "positions",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "positions",
                newName: "CreatedAt");
        }
    }
}
