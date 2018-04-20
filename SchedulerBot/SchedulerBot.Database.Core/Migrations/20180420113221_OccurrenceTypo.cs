using Microsoft.EntityFrameworkCore.Migrations;

namespace SchedulerBot.Database.Core.Migrations
{
	public partial class OccurrenceTypo : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.RenameColumn(
				name: "NextOccurence",
				table: "ScheduledMessageEvents",
				newName: "NextOccurrence");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.RenameColumn(
				name: "NextOccurrence",
				table: "ScheduledMessageEvents",
				newName: "NextOccurence");
		}
	}
}
