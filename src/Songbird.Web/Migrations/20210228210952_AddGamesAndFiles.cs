using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Songbird.Web.Migrations
{
    public partial class AddGamesAndFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GameId",
                table: "LunchGamingDates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "BinaryFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Checksum = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BinaryFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LunchGames",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LunchGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LunchGames_BinaryFiles_IconId",
                        column: x => x.IconId,
                        principalTable: "BinaryFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LunchGamingDates_GameId",
                table: "LunchGamingDates",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_BinaryFiles_Checksum",
                table: "BinaryFiles",
                column: "Checksum");

            migrationBuilder.CreateIndex(
                name: "IX_LunchGames_IconId",
                table: "LunchGames",
                column: "IconId");

            migrationBuilder.AddForeignKey(
                name: "FK_LunchGamingDates_LunchGames_GameId",
                table: "LunchGamingDates",
                column: "GameId",
                principalTable: "LunchGames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LunchGamingDates_LunchGames_GameId",
                table: "LunchGamingDates");

            migrationBuilder.DropTable(
                name: "LunchGames");

            migrationBuilder.DropTable(
                name: "BinaryFiles");

            migrationBuilder.DropIndex(
                name: "IX_LunchGamingDates_GameId",
                table: "LunchGamingDates");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "LunchGamingDates");
        }
    }
}
