﻿using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Interfaces.Repositories;

namespace SchedulerBot.Database.Repositories
{
	/// <inheritdoc cref="IManageConversationLinkRepository"/>
	public class ManageConversationLinkRepository : BaseRepository<ManageConversationLink>, IManageConversationLinkRepository
	{
		public ManageConversationLinkRepository(DbContext dbContext)
			: base(dbContext)
		{
		}

		public async Task<ManageConversationLink> GetByTextAsync(string text)
		{
			ManageConversationLink result = await DbSet.FirstOrDefaultAsync(link => link.Text == text);
			return result;
		}
	}
}
