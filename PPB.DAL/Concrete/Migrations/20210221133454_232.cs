using Microsoft.EntityFrameworkCore.Migrations;

namespace PPB.DAL.Migrations
{
    public partial class _232 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                schema: "PPB",
                table: "PlayerLobby",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PlayerMove",
                schema: "PPB",
                table: "BattleLogs",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "PPB",
                table: "PlayerLobby");

            migrationBuilder.DropColumn(
                name: "PlayerMove",
                schema: "PPB",
                table: "BattleLogs");
        }
    }
}
