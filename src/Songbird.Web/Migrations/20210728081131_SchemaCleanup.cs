using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Songbird.Web.Migrations
{
    public partial class SchemaCleanup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationLogFilters_Applications_ApplicationId1",
                table: "ApplicationLogFilters");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Projects_ProjectId1",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Customers_CustomerId1",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_CustomerId1",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Applications_ProjectId1",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationLogFilters_ApplicationId1",
                table: "ApplicationLogFilters");

            migrationBuilder.DropColumn(
                name: "CustomerId1",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectId1",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "ApplicationId1",
                table: "ApplicationLogFilters");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ApplicationLogFilters",
                newName: "Environment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Environment",
                table: "ApplicationLogFilters",
                newName: "Name");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId1",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId1",
                table: "Applications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationId1",
                table: "ApplicationLogFilters",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CustomerId1",
                table: "Projects",
                column: "CustomerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ProjectId1",
                table: "Applications",
                column: "ProjectId1");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationLogFilters_ApplicationId1",
                table: "ApplicationLogFilters",
                column: "ApplicationId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationLogFilters_Applications_ApplicationId1",
                table: "ApplicationLogFilters",
                column: "ApplicationId1",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Projects_ProjectId1",
                table: "Applications",
                column: "ProjectId1",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Customers_CustomerId1",
                table: "Projects",
                column: "CustomerId1",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
