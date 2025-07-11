using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameClientToPerson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_service_orders_people_client_id",
                table: "service_orders");

            migrationBuilder.DropForeignKey(
                name: "FK_vehicles_people_client_id",
                table: "vehicles");

            migrationBuilder.RenameColumn(
                name: "client_id",
                table: "vehicles",
                newName: "person_id");

            migrationBuilder.RenameIndex(
                name: "IX_vehicles_client_id",
                table: "vehicles",
                newName: "IX_vehicles_person_id");

            migrationBuilder.RenameColumn(
                name: "client_id",
                table: "service_orders",
                newName: "person_id");

            migrationBuilder.RenameIndex(
                name: "IX_service_orders_client_id",
                table: "service_orders",
                newName: "IX_service_orders_person_id");

            migrationBuilder.AddForeignKey(
                name: "FK_service_orders_people_person_id",
                table: "service_orders",
                column: "person_id",
                principalTable: "people",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_service_orders_people_person_id",
                table: "service_orders");

            migrationBuilder.DropForeignKey(
                name: "FK_vehicles_people_person_id",
                table: "vehicles");

            migrationBuilder.RenameColumn(
                name: "person_id",
                table: "vehicles",
                newName: "client_id");

            migrationBuilder.RenameIndex(
                name: "IX_vehicles_person_id",
                table: "vehicles",
                newName: "IX_vehicles_client_id");

            migrationBuilder.RenameColumn(
                name: "person_id",
                table: "service_orders",
                newName: "client_id");

            migrationBuilder.RenameIndex(
                name: "IX_service_orders_person_id",
                table: "service_orders",
                newName: "IX_service_orders_client_id");

            migrationBuilder.AddForeignKey(
                name: "FK_service_orders_people_client_id",
                table: "service_orders",
                column: "client_id",
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
