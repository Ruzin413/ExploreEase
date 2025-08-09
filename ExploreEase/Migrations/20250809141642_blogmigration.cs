using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExploreEase.Migrations
{
    /// <inheritdoc />
    public partial class blogmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blogdb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", maxLength: 100000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogdb", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blogdb");
        }
    }
}
