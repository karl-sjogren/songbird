using Microsoft.EntityFrameworkCore.Migrations;

namespace Songbird.Web.Migrations
{
    public partial class AddIsEligibleForFikaScheduling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEligibleForFikaScheduling",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEligibleForFikaScheduling",
                table: "Users");
        }
    }
}
