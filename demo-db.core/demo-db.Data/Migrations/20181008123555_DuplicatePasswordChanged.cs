using Microsoft.EntityFrameworkCore.Migrations;

namespace demo_db.Data.Migrations
{
    public partial class DuplicatePasswordChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "Password",
                value: "toniStoraro23");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "Password",
                value: "toniStoraro22");
        }
    }
}
