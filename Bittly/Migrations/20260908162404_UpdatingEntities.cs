using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bittly.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OriginalUrls_AspNetUsers_UserId",
                table: "OriginalUrls");

            migrationBuilder.DropTable(
                name: "ShortUrls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OriginalUrls",
                table: "OriginalUrls");

            migrationBuilder.RenameTable(
                name: "OriginalUrls",
                newName: "Urls");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Urls",
                newName: "LongUrl");

            migrationBuilder.RenameColumn(
                name: "OriginalUrlId",
                table: "Urls",
                newName: "UrlId");

            migrationBuilder.RenameIndex(
                name: "IX_OriginalUrls_UserId",
                table: "Urls",
                newName: "IX_Urls_UserId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExiprationDate",
                table: "Urls",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ShortUrl",
                table: "Urls",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Urls",
                table: "Urls",
                column: "UrlId");

            migrationBuilder.CreateIndex(
                name: "IX_Urls_ShortUrl",
                table: "Urls",
                column: "ShortUrl",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Urls_AspNetUsers_UserId",
                table: "Urls",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Urls_AspNetUsers_UserId",
                table: "Urls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Urls",
                table: "Urls");

            migrationBuilder.DropIndex(
                name: "IX_Urls_ShortUrl",
                table: "Urls");

            migrationBuilder.DropColumn(
                name: "ExiprationDate",
                table: "Urls");

            migrationBuilder.DropColumn(
                name: "ShortUrl",
                table: "Urls");

            migrationBuilder.RenameTable(
                name: "Urls",
                newName: "OriginalUrls");

            migrationBuilder.RenameColumn(
                name: "LongUrl",
                table: "OriginalUrls",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "UrlId",
                table: "OriginalUrls",
                newName: "OriginalUrlId");

            migrationBuilder.RenameIndex(
                name: "IX_Urls_UserId",
                table: "OriginalUrls",
                newName: "IX_OriginalUrls_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OriginalUrls",
                table: "OriginalUrls",
                column: "OriginalUrlId");

            migrationBuilder.CreateTable(
                name: "ShortUrls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginalUrlId = table.Column<int>(type: "int", nullable: false),
                    ShortenedUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortUrls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShortUrls_OriginalUrls_OriginalUrlId",
                        column: x => x.OriginalUrlId,
                        principalTable: "OriginalUrls",
                        principalColumn: "OriginalUrlId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShortUrls_OriginalUrlId",
                table: "ShortUrls",
                column: "OriginalUrlId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OriginalUrls_AspNetUsers_UserId",
                table: "OriginalUrls",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
