using Microsoft.EntityFrameworkCore.Migrations;

namespace Parking_System_API.Migrations
{
    public partial class AddingSystemUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemUsers",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemUsers", x => x.Email);
                });

            migrationBuilder.InsertData(
                table: "SystemUsers",
                columns: new[] { "Email", "Name", "Password", "Salt", "Type" },
                values: new object[] { "admin@admin.com", "admin", "admin", null, true });

            migrationBuilder.InsertData(
                table: "SystemUsers",
                columns: new[] { "Email", "Name", "Password", "Salt", "Type" },
                values: new object[] { "operator@operator.com", "operator", "operator", null, false });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemUsers");
        }
    }
}
