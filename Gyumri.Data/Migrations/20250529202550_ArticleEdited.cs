using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gyumri.Data.Migrations
{
    /// <inheritdoc />
    public partial class ArticleEdited : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TitleArm",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleRus",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentArm",
                table: "ArticleBlocks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentRus",
                table: "ArticleBlocks",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TitleArm",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "TitleRus",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ContentArm",
                table: "ArticleBlocks");

            migrationBuilder.DropColumn(
                name: "ContentRus",
                table: "ArticleBlocks");
        }
    }
}
