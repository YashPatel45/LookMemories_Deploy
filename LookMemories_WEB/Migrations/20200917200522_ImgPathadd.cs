using Microsoft.EntityFrameworkCore.Migrations;

namespace LookMemories_WEB.Migrations
{
    public partial class ImgPathadd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgName",
                table: "Photos",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5eba9226-5f58-4a2d-8c04-3126aefd3698",
                column: "ConcurrencyStamp",
                value: "0182da08-a26d-4414-9e5a-69796c2b5b34");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "690f679c-4431-41ad-b098-82fc4f8c4dea",
                column: "ConcurrencyStamp",
                value: "071a1f79-803a-4d14-9240-dd4755ae5bc9");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "821d8df1-5f80-455a-99be-0b2178deabf8", "AQAAAAEAACcQAAAAEJ6pHxNS4ZNxw2WLOFSsZ+KTc4sMSW2mPx8v84f/uSgtysewEfA+JzEz54CYZazevg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgName",
                table: "Photos");

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
    }
}
