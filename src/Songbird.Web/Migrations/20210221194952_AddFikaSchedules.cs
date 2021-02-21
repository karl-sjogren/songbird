using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Songbird.Web.Migrations
{
    public partial class AddFikaSchedules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FikaSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FikaSchedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FikaMatches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FikaScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FikaMatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FikaMatches_FikaSchedules_FikaScheduleId",
                        column: x => x.FikaScheduleId,
                        principalTable: "FikaSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FikaMatchUser",
                columns: table => new
                {
                    FikaMatchesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FikaMatchUser", x => new { x.FikaMatchesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_FikaMatchUser_FikaMatches_FikaMatchesId",
                        column: x => x.FikaMatchesId,
                        principalTable: "FikaMatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FikaMatchUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FikaMatches_FikaScheduleId",
                table: "FikaMatches",
                column: "FikaScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_FikaMatchUser_UsersId",
                table: "FikaMatchUser",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FikaMatchUser");

            migrationBuilder.DropTable(
                name: "FikaMatches");

            migrationBuilder.DropTable(
                name: "FikaSchedules");
        }
    }
}
