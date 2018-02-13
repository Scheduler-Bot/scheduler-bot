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
                    Id = table.Column<Guid>(nullable: false),
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
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ScheduledMessageId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledMessageRuns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduledMessageRuns_ScheduledMessages_ScheduledMessageId",
                        column: x => x.ScheduledMessageId,
                        principalTable: "ScheduledMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledMessageRuns_ScheduledMessageId",
                table: "ScheduledMessageRuns",
                column: "ScheduledMessageId");
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
