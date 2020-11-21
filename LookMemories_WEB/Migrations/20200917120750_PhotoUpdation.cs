using Microsoft.EntityFrameworkCore.Migrations;

namespace LookMemories_WEB.Migrations
{
    public partial class PhotoUpdation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFav",
                table: "Photos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Photos",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5eba9226-5f58-4a2d-8c04-3126aefd3698",
                column: "ConcurrencyStamp",
                value: "5d1dd43b-c881-4a94-a9a1-cb3e6ce5e902");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "690f679c-4431-41ad-b098-82fc4f8c4dea",
                column: "ConcurrencyStamp",
                value: "21a3c3a2-4adf-46e7-9457-54aae7ae45b2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "86c1922f-ffda-46f3-aa29-eb575a85b0ff", "AQAAAAEAACcQAAAAEHTYaBbevwUXis+KpmsYHAt3uVkCw3RLiTHHf2XIde0PjBKKz4Edf/ZfNT9vH8itZg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFav",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Photos");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5eba9226-5f58-4a2d-8c04-3126aefd3698",
                column: "ConcurrencyStamp",
                value: "6c15d54d-1301-446c-9c23-030e4ce33b0d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "690f679c-4431-41ad-b098-82fc4f8c4dea",
                column: "ConcurrencyStamp",
                value: "3fbcd94e-36c4-4848-851d-6b2925aeb234");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "8fcaa3b4-967f-4015-b04d-b4663fd8acea", "AQAAAAEAACcQAAAAEO40VSX9z6h/+ReK2vzUgqDXH+j5E55KxwQO0s2gZmF5CvguELIuA69g/VtpNZ8vFw==" });
        }
    }
}
