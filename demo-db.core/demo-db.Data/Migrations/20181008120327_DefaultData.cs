using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace demo_db.Data.Migrations
{
    public partial class DefaultData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Admin" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Student" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Teacher" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Deleted", "FullName", "MentorId", "Password", "RegisteredOn", "RoleId", "UserName" },
                values: new object[] { 2, false, "Pesho Goshev", null, "abc123", new DateTime(2016, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "pesho04" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Deleted", "FullName", "MentorId", "Password", "RegisteredOn", "RoleId", "UserName" },
                values: new object[] { 3, false, "Penka Tosheva", null, "toniStoraro22", new DateTime(2016, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "gosho007" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Deleted", "FullName", "MentorId", "Password", "RegisteredOn", "RoleId", "UserName" },
                values: new object[] { 1, false, "Uchitelq Yoda", null, "parola", new DateTime(2016, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Yo666" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
