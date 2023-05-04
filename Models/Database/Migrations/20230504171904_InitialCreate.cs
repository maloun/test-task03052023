using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace demo.Models.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HouseConsumers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConsumerId = table.Column<int>(type: "int", nullable: false),
                    UploadDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseConsumers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlantsConsumers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConsumerId = table.Column<int>(type: "int", nullable: false),
                    UploadDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantsConsumers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HouseConsumptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Weather = table.Column<double>(type: "float", nullable: false),
                    Consumption = table.Column<double>(type: "float", nullable: false),
                    ConsumerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseConsumptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HouseConsumptions_HouseConsumers_ConsumerId",
                        column: x => x.ConsumerId,
                        principalTable: "HouseConsumers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlantsConsumptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Consumption = table.Column<double>(type: "float", nullable: false),
                    ConsumerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantsConsumptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlantsConsumptions_PlantsConsumers_ConsumerId",
                        column: x => x.ConsumerId,
                        principalTable: "PlantsConsumers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HouseConsumptions_ConsumerId",
                table: "HouseConsumptions",
                column: "ConsumerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantsConsumptions_ConsumerId",
                table: "PlantsConsumptions",
                column: "ConsumerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HouseConsumptions");

            migrationBuilder.DropTable(
                name: "PlantsConsumptions");

            migrationBuilder.DropTable(
                name: "HouseConsumers");

            migrationBuilder.DropTable(
                name: "PlantsConsumers");
        }
    }
}
