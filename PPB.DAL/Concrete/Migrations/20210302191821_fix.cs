using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace PPB.DAL.Migrations
{
    public partial class fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMusics",
                schema: "PPB",
                table: "UserMusics");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                schema: "PPB",
                table: "UserMusics",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "_id",
                schema: "PPB",
                table: "UserMusics",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMusics",
                schema: "PPB",
                table: "UserMusics",
                column: "_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMusics",
                schema: "PPB",
                table: "UserMusics");

            migrationBuilder.DropColumn(
                name: "_id",
                schema: "PPB",
                table: "UserMusics");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                schema: "PPB",
                table: "UserMusics",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMusics",
                schema: "PPB",
                table: "UserMusics",
                column: "UserID");
        }
    }
}
