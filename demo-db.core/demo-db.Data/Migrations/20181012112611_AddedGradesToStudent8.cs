using Microsoft.EntityFrameworkCore.Migrations;

namespace demo_db.Data.Migrations
{
    public partial class AddedGradesToStudent8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "AssaignmentId", "StudentId", "ReceivedGrade" },
                values: new object[] { 2, 8, 28.0 });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "AssaignmentId", "StudentId", "ReceivedGrade" },
                values: new object[] { 3, 8, 74.0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumns: new[] { "AssaignmentId", "StudentId" },
                keyValues: new object[] { 2, 8 });

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumns: new[] { "AssaignmentId", "StudentId" },
                keyValues: new object[] { 3, 8 });
        }
    }
}
