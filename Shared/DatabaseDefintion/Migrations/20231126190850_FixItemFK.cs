using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseDefintion.Migrations
{
    /// <inheritdoc />
    public partial class FixItemFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemPlatformLinks_Items_PlatformId",
                table: "ItemPlatformLinks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_ItemPlatformLinks_Items_PlatformId",
                table: "ItemPlatformLinks",
                column: "PlatformId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
