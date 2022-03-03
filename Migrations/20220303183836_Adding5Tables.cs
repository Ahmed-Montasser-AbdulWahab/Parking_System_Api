using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Parking_System_API.Migrations
{
    public partial class Adding5Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SystemUsers",
                keyColumn: "Email",
                keyValue: "admin@admin.com");

            migrationBuilder.DeleteData(
                table: "SystemUsers",
                keyColumn: "Email",
                keyValue: "operator@operator.com");

            migrationBuilder.CreateTable(
                name: "Hardware",
                columns: table => new
                {
                    HardwareId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HardwareType = table.Column<int>(type: "int", nullable: false),
                    ConnectionString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Service = table.Column<bool>(type: "bit", nullable: false),
                    Direction = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hardware", x => x.HardwareId);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    ParticipantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoProvidePhoto = table.Column<bool>(type: "bit", nullable: false),
                    DoDetected = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.ParticipantId);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    PlateNumberId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartSubscription = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndSubscription = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPresent = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.PlateNumberId);
                });

            migrationBuilder.CreateTable(
                name: "ParkingTransactions",
                columns: table => new
                {
                    ParticipantId = table.Column<int>(type: "int", nullable: false),
                    PlateNumberId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HardwareId = table.Column<int>(type: "int", nullable: false),
                    DateTimeTransaction = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Result = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingTransactions", x => new { x.ParticipantId, x.PlateNumberId, x.HardwareId, x.DateTimeTransaction });
                    table.ForeignKey(
                        name: "FK_ParkingTransactions_Hardware_HardwareId",
                        column: x => x.HardwareId,
                        principalTable: "Hardware",
                        principalColumn: "HardwareId",
                        onDelete: ReferentialAction.Cascade,
                        onUpdate: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParkingTransactions_Participants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "ParticipantId",
                        onDelete: ReferentialAction.Cascade,
                        onUpdate: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParkingTransactions_Vehicles_PlateNumberId",
                        column: x => x.PlateNumberId,
                        principalTable: "Vehicles",
                        principalColumn: "PlateNumberId",
                        onDelete: ReferentialAction.Cascade,
                        onUpdate: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Participant_Vehicle",
                columns: table => new
                {
                    ParticipantsParticipantId = table.Column<int>(type: "int", nullable: false),
                    VehiclesPlateNumberId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participant_Vehicle", x => new { x.ParticipantsParticipantId, x.VehiclesPlateNumberId });
                    table.ForeignKey(
                        name: "FK_Participant_Vehicle_Participants_ParticipantsParticipantId",
                        column: x => x.ParticipantsParticipantId,
                        principalTable: "Participants",
                        principalColumn: "ParticipantId",
                        onDelete: ReferentialAction.Cascade,
                        onUpdate: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participant_Vehicle_Vehicles_VehiclesPlateNumberId",
                        column: x => x.VehiclesPlateNumberId,
                        principalTable: "Vehicles",
                        principalColumn: "PlateNumberId",
                        onDelete: ReferentialAction.Cascade,
                        onUpdate: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParkingTransactions_HardwareId",
                table: "ParkingTransactions",
                column: "HardwareId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingTransactions_PlateNumberId",
                table: "ParkingTransactions",
                column: "PlateNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_Participant_Vehicle_VehiclesPlateNumberId",
                table: "Participant_Vehicle",
                column: "VehiclesPlateNumberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParkingTransactions");

            migrationBuilder.DropTable(
                name: "Participant_Vehicle");

            migrationBuilder.DropTable(
                name: "Hardware");

            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.InsertData(
                table: "SystemUsers",
                columns: new[] { "Email", "Name", "Password", "Salt", "Type" },
                values: new object[] { "admin@admin.com", "admin", "admin", null, true });

            migrationBuilder.InsertData(
                table: "SystemUsers",
                columns: new[] { "Email", "Name", "Password", "Salt", "Type" },
                values: new object[] { "operator@operator.com", "operator", "operator", null, false });
        }
    }
}
