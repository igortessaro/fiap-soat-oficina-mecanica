using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class HotfixServiceOrdersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "vehicle_check_in_date",
                table: "service_orders");

            migrationBuilder.DropColumn(
                name: "vehicle_check_out_date",
                table: "service_orders");

            migrationBuilder.CreateTable(
                name: "AvailableServiceSupply",
                columns: table => new
                {
                    AvailableServicesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SuppliesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailableServiceSupply", x => new { x.AvailableServicesId, x.SuppliesId });
                    table.ForeignKey(
                        name: "FK_AvailableServiceSupply_available_services_AvailableServicesId",
                        column: x => x.AvailableServicesId,
                        principalTable: "available_services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AvailableServiceSupply_supplies_SuppliesId",
                        column: x => x.SuppliesId,
                        principalTable: "supplies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AvailableServiceSupply_SuppliesId",
                table: "AvailableServiceSupply",
                column: "SuppliesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvailableServiceSupply");

            migrationBuilder.AddColumn<DateTime>(
                name: "vehicle_check_in_date",
                table: "service_orders",
                type: "DATETIME",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "vehicle_check_out_date",
                table: "service_orders",
                type: "DATETIME",
                nullable: true);
        }
    }
}
