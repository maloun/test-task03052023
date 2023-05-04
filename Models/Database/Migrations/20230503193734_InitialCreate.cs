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
                name: "HouseConsumersTables",
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
                    table.PrimaryKey("PK_HouseConsumersTables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlantsConsumersTables",
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
                    table.PrimaryKey("PK_PlantsConsumersTables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HouseConsumptionsTables",
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
                    table.PrimaryKey("PK_HouseConsumptionsTables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HouseConsumptionsTables_HouseConsumersTables_ConsumerId",
                        column: x => x.ConsumerId,
                        principalTable: "HouseConsumersTables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlantsConsumptionsTables",
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
                    table.PrimaryKey("PK_PlantsConsumptionsTables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlantsConsumptionsTables_PlantsConsumersTables_ConsumerId",
                        column: x => x.ConsumerId,
                        principalTable: "PlantsConsumersTables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HouseConsumptionsTables_ConsumerId",
                table: "HouseConsumptionsTables",
                column: "ConsumerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantsConsumptionsTables_ConsumerId",
                table: "PlantsConsumptionsTables",
                column: "ConsumerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HouseConsumptionsTables");

            migrationBuilder.DropTable(
                name: "PlantsConsumptionsTables");

            migrationBuilder.DropTable(
                name: "HouseConsumersTables");

            migrationBuilder.DropTable(
                name: "PlantsConsumersTables");
        }
    }
}
