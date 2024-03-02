using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Chat_AspNetCore7.Migrations
{
    /// <inheritdoc />
    public partial class userOnlineSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "16e5743d-974c-4ec9-86c1-b68a73646865");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "98376069-0188-461f-92cb-33a877ad2054");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e65c1ec3-fb31-40e1-8254-b6b32666cd5d");

            migrationBuilder.AddColumn<bool>(
                name: "IsOnline",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "00af30ff-f882-458b-a342-376d4561703d", null, "Admin", "ADMIN" },
                    { "9bd52e3f-5a56-4df6-9a90-c9fee3560f5d", null, "Editor", "EDITOR" },
                    { "edce8ea2-1cb4-43b2-8568-8d529fcbab4c", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "00af30ff-f882-458b-a342-376d4561703d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9bd52e3f-5a56-4df6-9a90-c9fee3560f5d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "edce8ea2-1cb4-43b2-8568-8d529fcbab4c");

            migrationBuilder.DropColumn(
                name: "IsOnline",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "16e5743d-974c-4ec9-86c1-b68a73646865", null, "Editor", "EDITOR" },
                    { "98376069-0188-461f-92cb-33a877ad2054", null, "Admin", "ADMIN" },
                    { "e65c1ec3-fb31-40e1-8254-b6b32666cd5d", null, "User", "USER" }
                });
        }
    }
}
