using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEmployeeFromServiceOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_service_orders_people_EmployeeId",
                table: "service_orders");

            migrationBuilder.DropForeignKey(
                name: "FK_service_orders_people_person_id",
                table: "service_orders");

            migrationBuilder.DropIndex(
                name: "IX_service_orders_EmployeeId",
                table: "service_orders");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_service_orders_people_client_id",
                table: "service_orders");

            migrationBuilder.RenameColumn(
                name: "client_id",
                table: "service_orders",
                newName: "person_id");

            migrationBuilder.RenameIndex(
                name: "IX_service_orders_client_id",
                table: "service_orders",
                newName: "IX_service_orders_person_id");

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
                name: "FK_service_orders_people_person_id",
                table: "service_orders",
                column: "person_id",
                principalTable: "people",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
