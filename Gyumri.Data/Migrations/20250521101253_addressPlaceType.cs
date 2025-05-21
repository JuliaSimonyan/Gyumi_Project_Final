using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gyumri.Data.Migrations
{
    /// <inheritdoc />
    public partial class addressPlaceType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Places",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressArm",
                table: "Places",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressRu",
                table: "Places",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlaceType",
                table: "Places",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "AddressArm",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "AddressRu",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "PlaceType",
                table: "Places");
        }
    }
}
