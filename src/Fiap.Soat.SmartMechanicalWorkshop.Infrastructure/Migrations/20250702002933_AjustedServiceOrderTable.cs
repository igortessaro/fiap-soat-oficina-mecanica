using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjustedServiceOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_service_orders_clients_client_id",
                table: "service_orders");

            migrationBuilder.DropForeignKey(
                name: "FK_service_orders_vehicles_vehicle_id",
                table: "service_orders");

            migrationBuilder.RenameColumn(
                name: "VehicleCheckOutDate",
                table: "service_orders",
                newName: "vehicle_check_out_date");

            migrationBuilder.RenameColumn(
                name: "VehicleCheckInDate",
                table: "service_orders",
                newName: "vehicle_check_in_date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "vehicle_check_out_date",
                table: "service_orders",
                type: "DATETIME",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "vehicle_check_in_date",
                table: "service_orders",
                type: "DATETIME",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddForeignKey(
                name: "FK_service_orders_clients_client_id",
                table: "service_orders",
                column: "client_id",
                principalTable: "clients",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_service_orders_vehicles_vehicle_id",
                table: "service_orders",
                column: "vehicle_id",
                principalTable: "vehicles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_service_orders_clients_client_id",
                table: "service_orders");

            migrationBuilder.DropForeignKey(
                name: "FK_service_orders_vehicles_vehicle_id",
                table: "service_orders");

            migrationBuilder.RenameColumn(
                name: "vehicle_check_out_date",
                table: "service_orders",
                newName: "VehicleCheckOutDate");

            migrationBuilder.RenameColumn(
                name: "vehicle_check_in_date",
                table: "service_orders",
                newName: "VehicleCheckInDate");

            migrationBuilder.AlterColumn<DateTime>(
                name: "VehicleCheckOutDate",
                table: "service_orders",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "VehicleCheckInDate",
                table: "service_orders",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME");

            migrationBuilder.AddForeignKey(
                name: "FK_service_orders_clients_client_id",
                table: "service_orders",
                column: "client_id",
                principalTable: "clients",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_service_orders_vehicles_vehicle_id",
                table: "service_orders",
                column: "vehicle_id",
                principalTable: "vehicles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
