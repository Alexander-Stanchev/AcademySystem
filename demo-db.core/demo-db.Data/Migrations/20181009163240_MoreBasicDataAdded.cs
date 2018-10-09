using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace demo_db.Data.Migrations
{
    public partial class MoreBasicDataAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "CourseId", "End", "Name", "Start", "TeacherId" },
                values: new object[] { 1, new DateTime(2018, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alpha .NET", new DateTime(2017, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "CourseId", "End", "Name", "Start", "TeacherId" },
                values: new object[] { 2, new DateTime(2018, 4, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alpha Java", new DateTime(2017, 10, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 });

            migrationBuilder.InsertData(
                table: "Assaignments",
                columns: new[] { "Id", "CourseId", "DateTime", "MaxPoints", "Name" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2017, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 100, "OOP Assignment 1" },
                    { 2, 1, new DateTime(2018, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 100, "OOP Assignment 2" },
                    { 3, 1, new DateTime(2018, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 100, "OOP Assignment 3" },
                    { 4, 2, new DateTime(2018, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 100, "DSA Assignment 1" },
                    { 5, 2, new DateTime(2018, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 100, "DSA Assignment 2" },
                    { 6, 2, new DateTime(2018, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, "DSA Assignment 3" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Assaignments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Assaignments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Assaignments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Assaignments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Assaignments",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Assaignments",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 2);
        }
    }
}
