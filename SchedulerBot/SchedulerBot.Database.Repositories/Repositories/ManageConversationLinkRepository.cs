﻿using System.Globalization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Interfaces.Repositories;

namespace SchedulerBot.Database.Repositories
{
	/// <inheritdoc cref="IManageConversationLinkRepository"/>
	public class ManageConversationLinkRepository : BaseRepository<ManageConversationLink>, IManageConversationLinkRepository
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ManageConversationLinkRepository"/> class.
		/// </summary>
		/// <param name="dbContext">The database context.</param>
		public ManageConversationLinkRepository(SchedulerBotContext dbContext)
			: base(dbContext)
		{
		}

		/// <inheritdoc />
		public async Task<ManageConversationLink> GetByTextAsync(string text)
		{
			ManageConversationLink result = await DbSet.FirstOrDefaultAsync(link => link.Text.ToUpper(CultureInfo.InvariantCulture) == text.ToUpper(CultureInfo.InvariantCulture));
			return result;
		}
	}
}
