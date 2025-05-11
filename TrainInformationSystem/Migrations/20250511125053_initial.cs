using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TrainInfoSystem.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    ClassId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.ClassId);
                });

            migrationBuilder.CreateTable(
                name: "Trains",
                columns: table => new
                {
                    TrainId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrainNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trains", x => x.TrainId);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PassengerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrainId = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    JourneyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Bookings_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Trains_TrainId",
                        column: x => x.TrainId,
                        principalTable: "Trains",
                        principalColumn: "TrainId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fares",
                columns: table => new
                {
                    TrainId = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    FareAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalSeats = table.Column<int>(type: "int", nullable: false),
                    AvailableSeats = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fares", x => new { x.TrainId, x.ClassId });
                    table.ForeignKey(
                        name: "FK_Fares_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fares_Trains_TrainId",
                        column: x => x.TrainId,
                        principalTable: "Trains",
                        principalColumn: "TrainId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PNRs",
                columns: table => new
                {
                    PNRId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PNRNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Coach = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BerthNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeatNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PNRs", x => x.PNRId);
                    table.ForeignKey(
                        name: "FK_PNRs_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "ClassId", "ClassName" },
                values: new object[,]
                {
                    { 1, "First AC" },
                    { 2, "AC 2 Tier" },
                    { 3, "AC 3 Tier" },
                    { 4, "Sleeper" }
                });

            migrationBuilder.InsertData(
                table: "Trains",
                columns: new[] { "TrainId", "TrainName", "TrainNumber" },
                values: new object[,]
                {
                    { 1, "Rajdhani Express", "12951" },
                    { 2, "Shatabdi Express", "12009" },
                    { 3, "Duronto Express", "12245" }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "BookingId", "ClassId", "Email", "JourneyDate", "PassengerName", "TrainId" },
                values: new object[,]
                {
                    { 1, 1, "amit.sharma@example.com", new DateTime(2025, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Amit Sharma", 1 },
                    { 2, 2, "priya.patel@example.com", new DateTime(2025, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Priya Patel", 1 },
                    { 3, 3, "rahul.gupta@example.com", new DateTime(2025, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rahul Gupta", 2 },
                    { 4, 2, "sneha.verma@example.com", new DateTime(2025, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sneha Verma", 3 },
                    { 5, 4, "vikram.singh@example.com", new DateTime(2025, 6, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vikram Singh", 3 }
                });

            migrationBuilder.InsertData(
                table: "Fares",
                columns: new[] { "ClassId", "TrainId", "AvailableSeats", "FareAmount", "TotalSeats" },
                values: new object[,]
                {
                    { 1, 1, 15, 4500.00m, 20 },
                    { 2, 1, 30, 3000.00m, 40 },
                    { 3, 1, 50, 2000.00m, 60 },
                    { 4, 1, 70, 600.00m, 80 },
                    { 1, 2, 10, 4000.00m, 15 },
                    { 2, 2, 25, 2500.00m, 30 },
                    { 3, 2, 40, 1800.00m, 50 },
                    { 1, 3, 12, 4200.00m, 18 },
                    { 2, 3, 28, 2800.00m, 35 },
                    { 3, 3, 45, 1900.00m, 55 }
                });

            migrationBuilder.InsertData(
                table: "PNRs",
                columns: new[] { "PNRId", "BerthNumber", "BookingId", "Coach", "PNRNumber", "SeatNo", "Status" },
                values: new object[,]
                {
                    { 1, "10", 1, "H1", "PNR123456789", "10A", "Confirmed" },
                    { 2, "15", 2, "A1", "PNR234567890", "15B", "Confirmed" },
                    { 3, "20", 3, "B2", "PNR345678901", "20C", "Waiting" },
                    { 4, "25", 4, "A2", "PNR456789012", "25A", "Confirmed" },
                    { 5, "30", 5, "S1", "PNR567890123", "30D", "RAC" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ClassId",
                table: "Bookings",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TrainId",
                table: "Bookings",
                column: "TrainId");

            migrationBuilder.CreateIndex(
                name: "IX_Fares_ClassId",
                table: "Fares",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_PNRs_BookingId",
                table: "PNRs",
                column: "BookingId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fares");

            migrationBuilder.DropTable(
                name: "PNRs");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Trains");
        }
    }
}
