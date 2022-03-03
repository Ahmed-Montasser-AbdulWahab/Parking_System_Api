using Microsoft.EntityFrameworkCore.Migrations;

namespace Parking_System_API.Migrations
{
    public partial class EditingDataTypeForHardwareType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingTransactions_Hardware_HardwareId",
                table: "ParkingTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Hardware",
                table: "Hardware");

            migrationBuilder.RenameTable(
                name: "Hardware",
                newName: "Hardwares");

            migrationBuilder.RenameIndex(
                name: "IX_Hardware_ConnectionString",
                table: "Hardwares",
                newName: "IX_Hardwares_ConnectionString");

            migrationBuilder.AlterColumn<string>(
                name: "HardwareType",
                table: "Hardwares",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Hardwares",
                table: "Hardwares",
                column: "HardwareId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingTransactions_Hardwares_HardwareId",
                table: "ParkingTransactions",
                column: "HardwareId",
                principalTable: "Hardwares",
                principalColumn: "HardwareId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingTransactions_Hardwares_HardwareId",
                table: "ParkingTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Hardwares",
                table: "Hardwares");

            migrationBuilder.RenameTable(
                name: "Hardwares",
                newName: "Hardware");

            migrationBuilder.RenameIndex(
                name: "IX_Hardwares_ConnectionString",
                table: "Hardware",
                newName: "IX_Hardware_ConnectionString");

            migrationBuilder.AlterColumn<int>(
                name: "HardwareType",
                table: "Hardware",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Hardware",
                table: "Hardware",
                column: "HardwareId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingTransactions_Hardware_HardwareId",
                table: "ParkingTransactions",
                column: "HardwareId",
                principalTable: "Hardware",
                principalColumn: "HardwareId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
