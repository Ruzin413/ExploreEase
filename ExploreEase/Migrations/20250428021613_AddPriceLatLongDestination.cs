using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExploreEase.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceLatLongDestination : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Destination",
                table: "TourPackage",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "Lat",
                table: "TourPackage",
                type: "real",
                maxLength: 100,
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Long",
                table: "TourPackage",
                type: "real",
                maxLength: 100,
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "price",
                table: "TourPackage",
                type: "int",
                maxLength: 100,
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Destination",
                table: "TourPackage");

            migrationBuilder.DropColumn(
                name: "Lat",
                table: "TourPackage");

            migrationBuilder.DropColumn(
                name: "Long",
                table: "TourPackage");

            migrationBuilder.DropColumn(
                name: "price",
                table: "TourPackage");
        }
    }
}
