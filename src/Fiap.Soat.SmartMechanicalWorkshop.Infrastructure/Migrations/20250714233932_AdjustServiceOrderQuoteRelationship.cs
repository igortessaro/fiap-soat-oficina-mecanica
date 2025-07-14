using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdjustServiceOrderQuoteRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_quotes_service_order_id",
                table: "quotes");

            migrationBuilder.CreateIndex(
                name: "IX_quotes_service_order_id",
                table: "quotes",
                column: "service_order_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_quotes_service_order_id",
                table: "quotes");

            migrationBuilder.CreateIndex(
                name: "IX_quotes_service_order_id",
                table: "quotes",
                column: "service_order_id",
                unique: true);
        }
    }
}
