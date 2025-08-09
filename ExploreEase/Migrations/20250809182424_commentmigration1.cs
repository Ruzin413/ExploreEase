using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExploreEase.Migrations
{
    /// <inheritdoc />
    public partial class commentmigration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "commentdb",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogId = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                    comment = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_commentdb", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "commentdb");
        }
    }
}
