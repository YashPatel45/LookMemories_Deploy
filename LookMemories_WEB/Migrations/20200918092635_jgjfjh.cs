using Microsoft.EntityFrameworkCore.Migrations;

namespace LookMemories_WEB.Migrations
{
    public partial class jgjfjh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5eba9226-5f58-4a2d-8c04-3126aefd3698",
                column: "ConcurrencyStamp",
                value: "177f25a4-5b22-4204-bc25-33cc2da57f60");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "690f679c-4431-41ad-b098-82fc4f8c4dea",
                column: "ConcurrencyStamp",
                value: "f47e2544-31b5-4fd5-a9b2-ea5e50d0a9e2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "30513e5f-a4c3-4aae-a411-a9b41e65f2b7", "AQAAAAEAACcQAAAAEFNOOruVHC8iiQzK4agmn1/ieD4SbkNwRVTSvdX7CycTSGvXcFPBUEreLLlknRahJw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
