using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AGPU_API.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlPropertyAndSeedAGPUTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Images",
                table: "AGPUs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.InsertData(
                table: "AGPUs",
                columns: new[] { "Id", "AverageBenchPercentage", "Brand", "Images", "Model", "Name", "Price", "ReleaseDate", "ValuePercentage" },
                values: new object[,]
                {
                    { 1, 100.0, "Nvidia", "[\"Uploads/GPU/RTX_3060.jpg\"]", "RTX 3060", "Nvidia RTX 3060", 28000m, new DateTime(2021, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 94.799999999999997 },
                    { 2, 71.400000000000006, "Nvidia", "[\"Uploads/GPU/GTX_1660SUPER.jpg\"]", "GTX 1660S (Super)", "Nvidia GTX 1660S (Super)", 20000m, new DateTime(2019, 10, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 70.799999999999997 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AGPUs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AGPUs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "Images",
                table: "AGPUs");
        }
    }
}
