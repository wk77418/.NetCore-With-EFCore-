using Microsoft.EntityFrameworkCore.Migrations;

namespace StudyTestDemo.Migrations
{
    public partial class AddSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Age",
                table: "Student",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Student",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "Id", "Age", "ClassName", "Email", "Name", "PhotoPath" },
                values: new object[] { 1, 10, 1, "dasfaf@qq.com", "张三", null });

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "Id", "Age", "ClassName", "Email", "Name", "PhotoPath" },
                values: new object[] { 2, 11, 2, "dafadf@qeq.com", "李四", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Student",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Student",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Student");

            migrationBuilder.AlterColumn<int>(
                name: "Age",
                table: "Student",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
