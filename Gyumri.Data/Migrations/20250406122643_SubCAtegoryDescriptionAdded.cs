using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gyumri.Data.Migrations
{
    /// <inheritdoc />
    public partial class SubCAtegoryDescriptionAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Subcategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionArm",
                table: "Subcategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionRu",
                table: "Subcategories",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Subcategories");

            migrationBuilder.DropColumn(
                name: "DescriptionArm",
                table: "Subcategories");

            migrationBuilder.DropColumn(
                name: "DescriptionRu",
                table: "Subcategories");
        }
    }
}
