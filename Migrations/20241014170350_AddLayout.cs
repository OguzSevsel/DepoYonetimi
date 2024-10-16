using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Balya_Yerleştirme.Migrations
{
    /// <inheritdoc />
    public partial class AddLayout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Layout",
                schema: "Layout",
                newName: "Layout");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Layout");

            migrationBuilder.RenameTable(
                name: "Layout",
                newName: "Layout",
                newSchema: "Layout");
        }
    }
}
