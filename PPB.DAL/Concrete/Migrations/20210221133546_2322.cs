using Microsoft.EntityFrameworkCore.Migrations;

namespace PPB.DAL.Migrations
{
    public partial class _2322 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "roundScore",
                schema: "PPB",
                table: "BattleLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "roundScore",
                schema: "PPB",
                table: "BattleLogs");
        }
    }
}
