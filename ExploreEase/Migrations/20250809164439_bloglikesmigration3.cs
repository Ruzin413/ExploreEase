using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExploreEase.Migrations
{
    /// <inheritdoc />
    public partial class bloglikesmigration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "BlogLikes");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "BlogLikes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BlogLikes");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BlogLikes",
                type: "nvarchar(max)",
                maxLength: 10000,
                nullable: false,
                defaultValue: "");
        }
    }
}
