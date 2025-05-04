using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gyumri.Data.Migrations
{
    /// <inheritdoc />
    public partial class ArticlesModelEditedVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Places_Articles_ArticleId",
                table: "Places");

            migrationBuilder.AlterColumn<int>(
                name: "ArticleId",
                table: "Places",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Places_Articles_ArticleId",
                table: "Places",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Places_Articles_ArticleId",
                table: "Places");

            migrationBuilder.AlterColumn<int>(
                name: "ArticleId",
                table: "Places",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Places_Articles_ArticleId",
                table: "Places",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
