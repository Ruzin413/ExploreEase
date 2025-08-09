using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExploreEase.Migrations
{
    /// <inheritdoc />
    public partial class bloglikestatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "likestatus",
                table: "Blogdb",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "likestatus",
                table: "Blogdb");
        }
    }
}
