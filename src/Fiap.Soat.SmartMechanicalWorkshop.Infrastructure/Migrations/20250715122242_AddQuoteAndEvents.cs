using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddQuoteAndEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_service_orders_people_EmployeeId",
                table: "service_orders");

            migrationBuilder.DropForeignKey(
                name: "FK_vehicles_people_client_id",
                table: "vehicles");

            migrationBuilder.DropIndex(
                name: "IX_service_orders_EmployeeId",
                table: "service_orders");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "service_orders");

            migrationBuilder.RenameColumn(
                name: "client_id",
                table: "vehicles",
                newName: "person_id");

            migrationBuilder.RenameIndex(
                name: "IX_vehicles_client_id",
                table: "vehicles",
                newName: "IX_vehicles_person_id");

            migrationBuilder.CreateTable(
                name: "quotes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    service_order_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    status = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quotes", x => x.id);
                    table.ForeignKey(
                        name: "FK_quotes_service_orders_service_order_id",
                        column: x => x.service_order_id,
                        principalTable: "service_orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "service_order_events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    status = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    service_order_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_order_events", x => x.id);
                    table.ForeignKey(
                        name: "FK_service_order_events_service_orders_service_order_id",
                        column: x => x.service_order_id,
                        principalTable: "service_orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "quote_services",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    quote_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    service_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quote_services", x => x.id);
                    table.ForeignKey(
                        name: "FK_quote_services_available_services_service_id",
                        column: x => x.service_id,
                        principalTable: "available_services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_quote_services_quotes_quote_id",
                        column: x => x.quote_id,
                        principalTable: "quotes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "quote_supplies",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    quote_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    supply_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quote_supplies", x => x.id);
                    table.ForeignKey(
                        name: "FK_quote_supplies_quotes_quote_id",
                        column: x => x.quote_id,
                        principalTable: "quotes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_quote_supplies_supplies_supply_id",
                        column: x => x.supply_id,
                        principalTable: "supplies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_quote_services_quote_id",
                table: "quote_services",
                column: "quote_id");

            migrationBuilder.CreateIndex(
                name: "IX_quote_services_service_id",
                table: "quote_services",
                column: "service_id");

            migrationBuilder.CreateIndex(
                name: "IX_quote_supplies_quote_id",
                table: "quote_supplies",
                column: "quote_id");

            migrationBuilder.CreateIndex(
                name: "IX_quote_supplies_supply_id",
                table: "quote_supplies",
                column: "supply_id");

            migrationBuilder.CreateIndex(
                name: "IX_quotes_service_order_id",
                table: "quotes",
                column: "service_order_id");

            migrationBuilder.CreateIndex(
                name: "IX_service_order_events_service_order_id",
                table: "service_order_events",
                column: "service_order_id");

            migrationBuilder.AddForeignKey(
                name: "FK_vehicles_people_person_id",
                table: "vehicles",
                column: "person_id",
                principalTable: "people",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vehicles_people_person_id",
                table: "vehicles");

            migrationBuilder.DropTable(
                name: "quote_services");

            migrationBuilder.DropTable(
                name: "quote_supplies");

            migrationBuilder.DropTable(
                name: "service_order_events");

            migrationBuilder.DropTable(
                name: "quotes");

            migrationBuilder.RenameColumn(
                name: "person_id",
                table: "vehicles",
                newName: "client_id");

            migrationBuilder.RenameIndex(
                name: "IX_vehicles_person_id",
                table: "vehicles",
                newName: "IX_vehicles_client_id");

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "service_orders",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_service_orders_EmployeeId",
                table: "service_orders",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_service_orders_people_EmployeeId",
                table: "service_orders",
                column: "EmployeeId",
                principalTable: "people",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_vehicles_people_client_id",
                table: "vehicles",
                column: "client_id",
                principalTable: "people",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
