using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseDefintion.Migrations
{
    /// <inheritdoc />
    public partial class Added_AltText_to_Picture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AltText",
                table: "ItemPictures",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AltText",
                table: "ItemPictures");
        }
    }
}
