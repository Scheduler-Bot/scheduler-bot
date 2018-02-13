using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace SchedulerBot.Database.Core.Migrations
{
    public partial class ScheduledMessageCron : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PerformAt",
                table: "ScheduledMessages");

            migrationBuilder.DropColumn(
                name: "State",
                table: "ScheduledMessages");

            migrationBuilder.AddColumn<string>(
                name: "Cron",
                table: "ScheduledMessages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cron",
                table: "ScheduledMessages");

            migrationBuilder.AddColumn<DateTime>(
                name: "PerformAt",
                table: "ScheduledMessages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "ScheduledMessages",
                nullable: false,
                defaultValue: 0);
        }
    }
}
