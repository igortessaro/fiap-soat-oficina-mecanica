using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmailContraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_people_email",
                table: "people",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_people_email",
                table: "people");
        }
    }
}
