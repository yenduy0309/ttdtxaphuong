using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ttxaphuong.Migrations
{
    /// <inheritdoc />
    public partial class lan2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id_account",
                keyValue: 1,
                column: "Create_at",
                value: new DateTime(2025, 4, 7, 15, 47, 32, 996, DateTimeKind.Utc).AddTicks(7139));

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id_account",
                keyValue: 2,
                column: "Create_at",
                value: new DateTime(2025, 4, 7, 15, 47, 32, 996, DateTimeKind.Utc).AddTicks(7148));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id_account",
                keyValue: 1,
                column: "Create_at",
                value: new DateTime(2025, 4, 7, 15, 42, 3, 49, DateTimeKind.Utc).AddTicks(1857));

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id_account",
                keyValue: 2,
                column: "Create_at",
                value: new DateTime(2025, 4, 7, 15, 42, 3, 49, DateTimeKind.Utc).AddTicks(1866));
        }
    }
}
