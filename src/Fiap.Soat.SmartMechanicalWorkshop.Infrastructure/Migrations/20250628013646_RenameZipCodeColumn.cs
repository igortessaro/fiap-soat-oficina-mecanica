using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameZipCodeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "zipCode",
                table: "addresses",
                newName: "zip_code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "zip_code",
                table: "addresses",
                newName: "zipCode");
        }
    }
}
