using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DS.Infrastructure.Postgresql.Entities
{
    /// <inheritdoc />
    public partial class RenameRelationPrimaryKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "department_positions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "department_locations",
                newName: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "department_positions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "department_locations",
                newName: "Id");
        }
    }
}
