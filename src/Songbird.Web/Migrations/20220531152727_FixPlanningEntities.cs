using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Songbird.Web.Migrations
{
    /// <inheritdoc />
    public partial class FixPlanningEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledStatuses_UserSchedules_UserScheduleId",
                table: "ScheduledStatuses");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledStatuses_UserScheduleId",
                table: "ScheduledStatuses");

            migrationBuilder.DropColumn(
                name: "UserScheduleId",
                table: "ScheduledStatuses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserScheduleId",
                table: "ScheduledStatuses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledStatuses_UserScheduleId",
                table: "ScheduledStatuses",
                column: "UserScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledStatuses_UserSchedules_UserScheduleId",
                table: "ScheduledStatuses",
                column: "UserScheduleId",
                principalTable: "UserSchedules",
                principalColumn: "Id");
        }
    }
}
