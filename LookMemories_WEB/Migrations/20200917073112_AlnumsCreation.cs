using Microsoft.EntityFrameworkCore.Migrations;

namespace LookMemories_WEB.Migrations
{
    public partial class AlnumsCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlbumName = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<string>(nullable: true),
                    AccountUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Albums_AspNetUsers_AccountUserId",
                        column: x => x.AccountUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhotoPath = table.Column<string>(nullable: true),
                    UplodedDate = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    AlbumId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Albums_AccountUserId",
                table: "Albums",
                column: "AccountUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_AlbumId",
                table: "Photos",
                column: "AlbumId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5eba9226-5f58-4a2d-8c04-3126aefd3698",
                column: "ConcurrencyStamp",
                value: "d3efb83a-f2ab-4eb7-8118-29c42d144eab");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "690f679c-4431-41ad-b098-82fc4f8c4dea",
                column: "ConcurrencyStamp",
                value: "bb4f28a7-7f0c-4c53-83b9-a9ec3726ac8a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3b447e22-efca-4b5e-9e81-6f0713e7c2cc", "AQAAAAEAACcQAAAAEAstIOagYeZ4Qx4mOVo/t9WC0YuWYmsj4I7UqezBUzp7lcXbmMmIloGTbBmAwk2zLA==" });
        }
    }
}
