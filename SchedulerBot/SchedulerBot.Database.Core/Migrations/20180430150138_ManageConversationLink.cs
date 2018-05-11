using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace SchedulerBot.Database.Core.Migrations
{
	public partial class ManageConversationLink : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "ManageConversationLinks",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					ChannelId = table.Column<string>(nullable: false),
					ConversationId = table.Column<string>(nullable: false),
					CreatedOn = table.Column<DateTime>(nullable: false),
					ExpiresOn = table.Column<DateTime>(nullable: false),
					IsVisited = table.Column<bool>(nullable: false),
					Text = table.Column<string>(maxLength: 64, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ManageConversationLinks", x => x.Id);
				});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "ManageConversationLinks");
		}
	}
}
