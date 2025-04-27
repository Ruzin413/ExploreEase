using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExploreEase.Migrations
{
    /// <inheritdoc />
    public partial class TourPackageMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TourPackage",
                columns: table => new
                {
                    TourPackageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourPackage", x => x.TourPackageId);
                });

            migrationBuilder.CreateTable(
                name: "dayHotels",
                columns: table => new
                {
                    DayHotelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TourPackageId = table.Column<int>(type: "int", nullable: false),
                    DayNumber = table.Column<int>(type: "int", nullable: false),
                    HotelName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HotelDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    HotelLocation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dayHotels", x => x.DayHotelId);
                    table.ForeignKey(
                        name: "FK_dayHotels_TourPackage_TourPackageId",
                        column: x => x.TourPackageId,
                        principalTable: "TourPackage",
                        principalColumn: "TourPackageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "hotelImage",
                columns: table => new
                {
                    HotelImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayHotelId = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hotelImage", x => x.HotelImageId);
                    table.ForeignKey(
                        name: "FK_hotelImage_dayHotels_DayHotelId",
                        column: x => x.DayHotelId,
                        principalTable: "dayHotels",
                        principalColumn: "DayHotelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dayHotels_TourPackageId",
                table: "dayHotels",
                column: "TourPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_hotelImage_DayHotelId",
                table: "hotelImage",
                column: "DayHotelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "hotelImage");

            migrationBuilder.DropTable(
                name: "dayHotels");

            migrationBuilder.DropTable(
                name: "TourPackage");
        }
    }
}
