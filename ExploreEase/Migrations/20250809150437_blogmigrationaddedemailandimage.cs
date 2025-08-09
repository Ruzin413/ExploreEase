using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExploreEase.Migrations
{
    /// <inheritdoc />
    public partial class blogmigrationaddedemailandimage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "image",
                table: "Blogdb",
                newName: "Blogimage");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Blogdb",
                type: "nvarchar(max)",
                maxLength: 10000,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Blogdb");

            migrationBuilder.RenameColumn(
                name: "Blogimage",
                table: "Blogdb",
                newName: "image");
        }
    }
}
