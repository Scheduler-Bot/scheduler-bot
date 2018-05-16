using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace SchedulerBot.Database.Core.Migrations
{
	public partial class ServiceUrl : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			const string backupServiceUrls =
@"
CREATE TABLE #tempServiceUrls ([MessageId] UNIQUEIDENTIFIER NOT NULL, [ServiceUrl] NVARCHAR(MAX) NOT NULL);

INSERT INTO #tempServiceUrls ([MessageId], [ServiceUrl])
SELECT SMD.[ScheduledMessageId], SMD.[ServiceUrl]
FROM [dbo].[ScheduledMessageDetails] SMD;
";
			migrationBuilder.Sql(backupServiceUrls);

			migrationBuilder.DropForeignKey(
				name: "FK_ScheduledMessageDetails_ScheduledMessages_ScheduledMessageId",
				table: "ScheduledMessageDetails");

			migrationBuilder.DropForeignKey(
				name: "FK_ScheduledMessageEvents_ScheduledMessages_ScheduledMessageId",
				table: "ScheduledMessageEvents");

			migrationBuilder.DropColumn(
				name: "ServiceUrl",
				table: "ScheduledMessageDetails");

			migrationBuilder.CreateTable(
				name: "ServiceUrls",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					Address = table.Column<string>(nullable: false),
					CreatedOn = table.Column<DateTime>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ServiceUrls", x => x.Id);
				});

			const string insertServiceUrls =
@"
INSERT INTO [dbo].[ServiceUrls] ([Id], [Address], [CreatedOn])
SELECT NEWID(), SU.[ServiceUrl], GETUTCDATE()
FROM (
	SELECT DISTINCT TSU.[ServiceUrl]
	FROM #tempServiceUrls TSU) AS SU;
";

			migrationBuilder.Sql(insertServiceUrls);

			migrationBuilder.CreateTable(
				name: "ScheduledMessageDetailsServiceUrls",
				columns: table => new
				{
					DetailsId = table.Column<Guid>(nullable: false),
					ServiceUrlId = table.Column<Guid>(nullable: false),
					CreatedOn = table.Column<DateTime>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ScheduledMessageDetailsServiceUrls", x => new { x.DetailsId, x.ServiceUrlId });
					table.ForeignKey(
						name: "FK_ScheduledMessageDetailsServiceUrls_ScheduledMessageDetails_DetailsId",
						column: x => x.DetailsId,
						principalTable: "ScheduledMessageDetails",
						principalColumn: "ScheduledMessageId",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_ScheduledMessageDetailsServiceUrls_ServiceUrls_ServiceUrlId",
						column: x => x.ServiceUrlId,
						principalTable: "ServiceUrls",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			const string insertDetailsServiceUrls =
@"
INSERT INTO [dbo].[ScheduledMessageDetailsServiceUrls] ([DetailsId], [ServiceUrlId], [CreatedOn])
SELECT TSU.[MessageId], SU.[Id], GETUTCDATE()
FROM #tempServiceUrls TSU
INNER JOIN [dbo].[ServiceUrls] SU ON TSU.[ServiceUrl]= SU.[Address]
";

			migrationBuilder.Sql(insertDetailsServiceUrls);

			migrationBuilder.CreateIndex(
				name: "IX_ScheduledMessageDetailsServiceUrls_ServiceUrlId",
				table: "ScheduledMessageDetailsServiceUrls",
				column: "ServiceUrlId");

			migrationBuilder.CreateIndex(
				name: "IX_ScheduledMessageDetailsServiceUrls_DetailsId_ServiceUrlId_CreatedOn",
				table: "ScheduledMessageDetailsServiceUrls",
				columns: new[] { "DetailsId", "ServiceUrlId", "CreatedOn" });

			migrationBuilder.CreateIndex(
				name: "IX_ServiceUrls_Address",
				table: "ServiceUrls",
				column: "Address",
				unique: true);

			migrationBuilder.AddForeignKey(
				name: "FK_ScheduledMessageDetails_ScheduledMessages_ScheduledMessageId",
				table: "ScheduledMessageDetails",
				column: "ScheduledMessageId",
				principalTable: "ScheduledMessages",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_ScheduledMessageEvents_ScheduledMessages_ScheduledMessageId",
				table: "ScheduledMessageEvents",
				column: "ScheduledMessageId",
				principalTable: "ScheduledMessages",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			const string dropTempServiceUrls = @"DROP TABLE #tempServiceUrls;";

			migrationBuilder.Sql(dropTempServiceUrls);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_ScheduledMessageDetails_ScheduledMessages_ScheduledMessageId",
				table: "ScheduledMessageDetails");

			migrationBuilder.DropForeignKey(
				name: "FK_ScheduledMessageEvents_ScheduledMessages_ScheduledMessageId",
				table: "ScheduledMessageEvents");

			const string backupServiceUrls =
@"
CREATE TABLE #tempServiceUrls ([MessageId] UNIQUEIDENTIFIER NOT NULL, [ServiceUrl] NVARCHAR(MAX) NOT NULL);

INSERT INTO #tempServiceUrls ([MessageId], [ServiceUrl])
SELECT DSU.[DetailsId], SU.[Address]
FROM [dbo].[ScheduledMessageDetailsServiceUrls] DSU
INNER JOIN [dbo].[ServiceUrls] SU ON DSU.[ServiceUrlId] = SU.[Id];
";
			migrationBuilder.Sql(backupServiceUrls);

			migrationBuilder.DropTable(
				name: "ScheduledMessageDetailsServiceUrls");

			migrationBuilder.DropTable(
				name: "ServiceUrls");

			migrationBuilder.AddColumn<string>(
				name: "ServiceUrl",
				table: "ScheduledMessageDetails",
				nullable: false,
				defaultValue: "");

			const string restoreServiceUrls =
@"
UPDATE SMD
SET SMD.[ServiceUrl] = TSU.[ServiceUrl]
FROM [dbo].[ScheduledMessageDetails] SMD
INNER JOIN #tempServiceUrls TSU ON SMD.[ScheduledMessageId] = TSU.[MessageId];
";

			migrationBuilder.Sql(restoreServiceUrls);

			migrationBuilder.AddForeignKey(
				name: "FK_ScheduledMessageDetails_ScheduledMessages_ScheduledMessageId",
				table: "ScheduledMessageDetails",
				column: "ScheduledMessageId",
				principalTable: "ScheduledMessages",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_ScheduledMessageEvents_ScheduledMessages_ScheduledMessageId",
				table: "ScheduledMessageEvents",
				column: "ScheduledMessageId",
				principalTable: "ScheduledMessages",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			const string dropTempServiceUrls = @"DROP TABLE #tempServiceUrls;";

			migrationBuilder.Sql(dropTempServiceUrls);
		}
	}
}
