using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Songbird.Web.Migrations
{
    public partial class MiscUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LunchGames_BinaryFiles_IconId",
                table: "LunchGames");

            migrationBuilder.RenameColumn(
                name: "HexColor",
                table: "Projects",
                newName: "AccentColor");

            migrationBuilder.AddColumn<Guid>(
                name: "IconId",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "IconId",
                table: "LunchGames",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "IconId",
                table: "Customers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_IconId",
                table: "Projects",
                column: "IconId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_IconId",
                table: "Customers",
                column: "IconId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_BinaryFiles_IconId",
                table: "Customers",
                column: "IconId",
                principalTable: "BinaryFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LunchGames_BinaryFiles_IconId",
                table: "LunchGames",
                column: "IconId",
                principalTable: "BinaryFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_BinaryFiles_IconId",
                table: "Projects",
                column: "IconId",
                principalTable: "BinaryFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_BinaryFiles_IconId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_LunchGames_BinaryFiles_IconId",
                table: "LunchGames");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_BinaryFiles_IconId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_IconId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Customers_IconId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IconId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IconId",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "AccentColor",
                table: "Projects",
                newName: "HexColor");

            migrationBuilder.AlterColumn<Guid>(
                name: "IconId",
                table: "LunchGames",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LunchGames_BinaryFiles_IconId",
                table: "LunchGames",
                column: "IconId",
                principalTable: "BinaryFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
