using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gyumri.Data.Migrations
{
    /// <inheritdoc />
    public partial class ArticlesModelEdited : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleBlocks_Articles_ArticleId",
                table: "ArticleBlocks");

            migrationBuilder.AlterColumn<int>(
                name: "Raiting",
                table: "Places",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ArticleId",
                table: "Places",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ArticleId",
                table: "ArticleBlocks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Places_ArticleId",
                table: "Places",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleBlocks_Articles_ArticleId",
                table: "ArticleBlocks",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Places_Articles_ArticleId",
                table: "Places",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleBlocks_Articles_ArticleId",
                table: "ArticleBlocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Places_Articles_ArticleId",
                table: "Places");

            migrationBuilder.DropIndex(
                name: "IX_Places_ArticleId",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "Places");

            migrationBuilder.AlterColumn<int>(
                name: "Raiting",
                table: "Places",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ArticleId",
                table: "ArticleBlocks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleBlocks_Articles_ArticleId",
                table: "ArticleBlocks",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id");
        }
    }
}
