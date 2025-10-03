using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<int>(type: "int", nullable: false),
                    OrderTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveredInd = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                });

            migrationBuilder.CreateTable(
                name: "Occurrence",
                columns: table => new
                {
                    OccurrenceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OccurrenceType = table.Column<int>(type: "int", nullable: false),
                    OccurrenceTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinisherInd = table.Column<bool>(type: "bit", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Occurrence", x => x.OccurrenceId);
                    table.ForeignKey(
                        name: "FK_Occurrence_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Occurrence_OrderId",
                table: "Occurrence",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Occurrence");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
