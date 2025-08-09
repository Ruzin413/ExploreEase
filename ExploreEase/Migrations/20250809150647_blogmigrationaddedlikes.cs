using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExploreEase.Migrations
{
    /// <inheritdoc />
    public partial class blogmigrationaddedlikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Blogdb",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Blogdb");
        }
    }
}
