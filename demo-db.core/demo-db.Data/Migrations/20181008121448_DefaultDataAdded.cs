using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace demo_db.Data.Migrations
{
    public partial class DefaultDataAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Deleted", "FullName", "MentorId", "Password", "RegisteredOn", "RoleId", "UserName" },
                values: new object[] { 4, false, "Stenley Rois", null, "toniStoraro22", new DateTime(2016, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "shefanarelefa" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
