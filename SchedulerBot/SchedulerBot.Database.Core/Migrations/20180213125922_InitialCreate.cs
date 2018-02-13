using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace SchedulerBot.Database.Core.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScheduledMessages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ConversationId = table.Column<string>(nullable: false),
                    Cron = table.Column<string>(nullable: false),
                    Text = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledMessageRuns",
                columns: table => new
                {
                    ScheduledMessageId = table.Column<int>(nullable: false),
                    RanAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledMessageRuns", x => new { x.ScheduledMessageId, x.RanAt });
                    table.ForeignKey(
                        name: "FK_ScheduledMessageRuns_ScheduledMessages_ScheduledMessageId",
                        column: x => x.ScheduledMessageId,
                        principalTable: "ScheduledMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduledMessageRuns");

            migrationBuilder.DropTable(
                name: "ScheduledMessages");
        }
    }
}
