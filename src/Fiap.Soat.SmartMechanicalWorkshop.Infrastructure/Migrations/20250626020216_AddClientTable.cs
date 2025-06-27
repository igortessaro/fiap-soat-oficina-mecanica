using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClientTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    document = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fullname = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phoneCountryCode = table.Column<string>(type: "VARCHAR(5)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phoneAreaCode = table.Column<string>(type: "VARCHAR(5)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phoneNumber = table.Column<string>(type: "VARCHAR(15)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phoneType = table.Column<string>(type: "VARCHAR(10)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "VARCHAR(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    addressStreet = table.Column<string>(type: "VARCHAR(100)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    addressCity = table.Column<string>(type: "VARCHAR(60)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    addressState = table.Column<string>(type: "VARCHAR(30)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    addressZipCode = table.Column<string>(type: "VARCHAR(15)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clients", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "clients");
        }
    }
}
