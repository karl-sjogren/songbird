using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Songbird.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddDaysAndSplitStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "ScheduledStatuses",
                newName: "MorningStatus");

            migrationBuilder.AddColumn<string>(
                name: "AfternoonStatus",
                table: "ScheduledStatuses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DayOfWeek",
                table: "ScheduledStatuses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AfternoonStatus",
                table: "ScheduledStatuses");

            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "ScheduledStatuses");

            migrationBuilder.RenameColumn(
                name: "MorningStatus",
                table: "ScheduledStatuses",
                newName: "Status");
        }
    }
}
