using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WDM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessLevel", "CreatedBy", "CreatedDate", "Email", "IsActive", "Password", "UserName" },
                values: new object[] { new Guid("8c647159-9a27-43c9-aa21-115e9dddee9e"), "Read-Write", new Guid("8c647159-9a27-43c9-aa21-115e9dddee9e"), new DateTime(2025, 8, 17, 19, 31, 26, 0, DateTimeKind.Utc), "irfan@gmail.com", true, "admin12345", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8c647159-9a27-43c9-aa21-115e9dddee9e"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");
        }
    }
}
