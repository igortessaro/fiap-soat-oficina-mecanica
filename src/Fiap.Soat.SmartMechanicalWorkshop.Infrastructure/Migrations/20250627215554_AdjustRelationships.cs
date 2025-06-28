using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdjustRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Supplies",
                table: "Supplies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AvailableServices",
                table: "AvailableServices");

            migrationBuilder.DropColumn(
                name: "addressCity",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "addressState",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "addressStreet",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "addressZipCode",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "phoneAreaCode",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "phoneCountryCode",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "phoneNumber",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "phoneType",
                table: "clients");

            migrationBuilder.RenameTable(
                name: "Vehicles",
                newName: "vehicles");

            migrationBuilder.RenameTable(
                name: "Supplies",
                newName: "supplies");

            migrationBuilder.RenameTable(
                name: "AvailableServices",
                newName: "available_services");

            migrationBuilder.RenameColumn(
                name: "Model",
                table: "vehicles",
                newName: "model");

            migrationBuilder.RenameColumn(
                name: "Brand",
                table: "vehicles",
                newName: "brand");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "vehicles",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "vehicles",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "ManufactureYear",
                table: "vehicles",
                newName: "manufacture_year");

            migrationBuilder.RenameColumn(
                name: "LicensePlate",
                table: "vehicles",
                newName: "license_plate");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "vehicles",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "vehicles",
                newName: "client_id");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "supplies",
                newName: "quantity");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "supplies",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "supplies",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "supplies",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "supplies",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "supplies",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "available_services",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "available_services",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "available_services",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "available_services",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "available_services",
                newName: "created_at");

            migrationBuilder.AlterColumn<string>(
                name: "model",
                table: "vehicles",
                type: "VARCHAR(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "brand",
                table: "vehicles",
                type: "VARCHAR(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "license_plate",
                table: "vehicles",
                type: "VARCHAR(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "supplies",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "supplies",
                type: "VARCHAR(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "clients",
                keyColumn: "email",
                keyValue: null,
                column: "email",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "clients",
                type: "VARCHAR(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "address_id",
                table: "clients",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "phone",
                table: "clients",
                type: "VARCHAR(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "available_services",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "available_services",
                type: "VARCHAR(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_vehicles",
                table: "vehicles",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_supplies",
                table: "supplies",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_available_services",
                table: "available_services",
                column: "id");

            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    street = table.Column<string>(type: "VARCHAR(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    city = table.Column<string>(type: "VARCHAR(60)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<string>(type: "VARCHAR(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    zipCode = table.Column<string>(type: "VARCHAR(15)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addresses", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_vehicles_client_id",
                table: "vehicles",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "IX_clients_address_id",
                table: "clients",
                column: "address_id");

            migrationBuilder.AddForeignKey(
                name: "FK_clients_addresses_address_id",
                table: "clients",
                column: "address_id",
                principalTable: "addresses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vehicles_clients_client_id",
                table: "vehicles",
                column: "client_id",
                principalTable: "clients",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_clients_addresses_address_id",
                table: "clients");

            migrationBuilder.DropForeignKey(
                name: "FK_vehicles_clients_client_id",
                table: "vehicles");

            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vehicles",
                table: "vehicles");

            migrationBuilder.DropIndex(
                name: "IX_vehicles_client_id",
                table: "vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_supplies",
                table: "supplies");

            migrationBuilder.DropIndex(
                name: "IX_clients_address_id",
                table: "clients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_available_services",
                table: "available_services");

            migrationBuilder.DropColumn(
                name: "address_id",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "phone",
                table: "clients");

            migrationBuilder.RenameTable(
                name: "vehicles",
                newName: "Vehicles");

            migrationBuilder.RenameTable(
                name: "supplies",
                newName: "Supplies");

            migrationBuilder.RenameTable(
                name: "available_services",
                newName: "AvailableServices");

            migrationBuilder.RenameColumn(
                name: "model",
                table: "Vehicles",
                newName: "Model");

            migrationBuilder.RenameColumn(
                name: "brand",
                table: "Vehicles",
                newName: "Brand");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Vehicles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Vehicles",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "manufacture_year",
                table: "Vehicles",
                newName: "ManufactureYear");

            migrationBuilder.RenameColumn(
                name: "license_plate",
                table: "Vehicles",
                newName: "LicensePlate");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Vehicles",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "client_id",
                table: "Vehicles",
                newName: "ClientId");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "Supplies",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "Supplies",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Supplies",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Supplies",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Supplies",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Supplies",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "AvailableServices",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "AvailableServices",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AvailableServices",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "AvailableServices",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "AvailableServices",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "Vehicles",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Brand",
                table: "Vehicles",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LicensePlate",
                table: "Vehicles",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldMaxLength: 20)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Supplies",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Supplies",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "clients",
                type: "VARCHAR(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "addressCity",
                table: "clients",
                type: "VARCHAR(60)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "addressState",
                table: "clients",
                type: "VARCHAR(30)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "addressStreet",
                table: "clients",
                type: "VARCHAR(100)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "addressZipCode",
                table: "clients",
                type: "VARCHAR(15)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "phoneAreaCode",
                table: "clients",
                type: "VARCHAR(5)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "phoneCountryCode",
                table: "clients",
                type: "VARCHAR(5)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "phoneNumber",
                table: "clients",
                type: "VARCHAR(15)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "phoneType",
                table: "clients",
                type: "VARCHAR(10)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "AvailableServices",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AvailableServices",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Supplies",
                table: "Supplies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AvailableServices",
                table: "AvailableServices",
                column: "Id");
        }
    }
}
