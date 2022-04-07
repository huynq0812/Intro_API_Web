using Microsoft.EntityFrameworkCore.Migrations;

namespace Intro_API_Web.Migrations
{
    public partial class RolesDefault : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "93c146ce-ec00-4697-a10b-9fda7c58397d", "820061b6-a267-4874-946e-6aba3a506ace", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8ebe7653-4f4b-478f-98c3-76471724f15b", "e81cd296-638f-46ac-a8ad-621ca1ea71e2", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8ebe7653-4f4b-478f-98c3-76471724f15b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "93c146ce-ec00-4697-a10b-9fda7c58397d");
        }
    }
}
