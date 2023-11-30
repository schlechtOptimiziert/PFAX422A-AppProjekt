using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseDefintion.Migrations
{
    /// <inheritdoc />
    public partial class ChangeItemPictures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bytes",
                table: "ItemPictures");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ItemPictures");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "ItemPictures");

            migrationBuilder.RenameColumn(
                name: "FileExtension",
                table: "ItemPictures",
                newName: "FileName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "ItemPictures",
                newName: "FileExtension");

            migrationBuilder.AddColumn<byte[]>(
                name: "Bytes",
                table: "ItemPictures",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ItemPictures",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Size",
                table: "ItemPictures",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
