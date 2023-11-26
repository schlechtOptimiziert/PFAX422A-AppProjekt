using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseDefintion.Migrations
{
    /// <inheritdoc />
    public partial class AddPlatform : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Platforms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platforms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemPlatformLinks",
                columns: table => new
                {
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    PlatformId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemPlatformLinks", x => new { x.ItemId, x.PlatformId });
                    table.ForeignKey(
                        name: "FK_ItemPlatformLinks_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemPlatformLinks_Platforms_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "Platforms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemPlatformLinks_PlatformId",
                table: "ItemPlatformLinks",
                column: "PlatformId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemPlatformLinks");

            migrationBuilder.DropTable(
                name: "Platforms");
        }
    }
}
