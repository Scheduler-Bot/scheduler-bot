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
					Schedule = table.Column<string>(nullable: false),
					Text = table.Column<string>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ScheduledMessages", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "ScheduledMessageLogs",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					CreatedOn = table.Column<DateTime>(nullable: false),
					ScheduledMessageId = table.Column<Guid>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ScheduledMessageLogs", x => x.Id);
					table.ForeignKey(
						name: "FK_ScheduledMessageLogs_ScheduledMessages_ScheduledMessageId",
						column: x => x.ScheduledMessageId,
						principalTable: "ScheduledMessages",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_ScheduledMessageLogs_ScheduledMessageId",
				table: "ScheduledMessageLogs",
				column: "ScheduledMessageId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "ScheduledMessageLogs");

			migrationBuilder.DropTable(
				name: "ScheduledMessages");
		}
	}
}
