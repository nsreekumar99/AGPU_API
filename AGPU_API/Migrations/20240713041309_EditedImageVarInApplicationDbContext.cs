using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AGPU_API.Migrations
{
    /// <inheritdoc />
    public partial class EditedImageVarInApplicationDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AGPUs",
                keyColumn: "Id",
                keyValue: 1,
                column: "Images",
                value: "[\"/Uploads/GPU/RTX_3060.jpg\"]");

            migrationBuilder.UpdateData(
                table: "AGPUs",
                keyColumn: "Id",
                keyValue: 2,
                column: "Images",
                value: "[\"/Uploads/GPU/GTX_1660SUPER.jpg\"]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AGPUs",
                keyColumn: "Id",
                keyValue: 1,
                column: "Images",
                value: "[\"Uploads/GPU/RTX_3060.jpg\"]");

            migrationBuilder.UpdateData(
                table: "AGPUs",
                keyColumn: "Id",
                keyValue: 2,
                column: "Images",
                value: "[\"Uploads/GPU/GTX_1660SUPER.jpg\"]");
        }
    }
}
