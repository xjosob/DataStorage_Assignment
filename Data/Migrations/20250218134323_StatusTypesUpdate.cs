using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class StatusTypesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "StatusTypes",
                columns: new[] { "Id", "StatusName" },
                values: new object[,]
                {
                    { 1, "Not started" },
                    { 2, "In Progress" },
                    { 3, "Completed" },
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "StatusTypes", keyColumn: "Id", keyValue: 1);

            migrationBuilder.DeleteData(table: "StatusTypes", keyColumn: "Id", keyValue: 2);

            migrationBuilder.DeleteData(table: "StatusTypes", keyColumn: "Id", keyValue: 3);
        }
    }
}
