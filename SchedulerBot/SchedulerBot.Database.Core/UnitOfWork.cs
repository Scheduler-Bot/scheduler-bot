using System;
using Autofac;
using Microsoft.EntityFrameworkCore;
using SchedulerBot.Database.Interfaces;
using SchedulerBot.Database.Interfaces.Repositories;

namespace SchedulerBot.Database.Core
{
	/// <summary>
	/// Unit of Work.
	/// </summary>
	/// <seealso cref="SchedulerBot.Database.Interfaces.IUnitOfWork" />
	public class UnitOfWork : IUnitOfWork
	{
		private readonly DbContext dbContext;
		private readonly ILifetimeScope lifetimeScope;

		public UnitOfWork(ILifetimeScope lifetimeScope, DbContext dbContext)
		{
			this.dbContext = dbContext;
			this.lifetimeScope = lifetimeScope;
		}

		#region Repositories

		#region MS SQL

		/// <summary>
		/// Gets <see cref="T:SchedulerBot.Database.Interfaces.Repositories.IManageConversationLinkRepository" />.
		/// </summary>
		public IManageConversationLinkRepository ManageConversationLinks => GetRepository<IManageConversationLinkRepository>();

		/// <summary>
		/// Gets <see cref="T:SchedulerBot.Database.Interfaces.Repositories.IScheduledMessageDetailsRepository" />.
		/// </summary>
		public IScheduledMessageDetailsRepository ScheduledMessageDetails => GetRepository<IScheduledMessageDetailsRepository>();

		/// <summary>
		/// Gets <see cref="T:SchedulerBot.Database.Interfaces.Repositories.IScheduledMessageDetailsServiceUrlRepository" />.
		/// </summary>
		public IScheduledMessageDetailsServiceUrlRepository ScheduledMessageDetailsServiceUrls => GetRepository<IScheduledMessageDetailsServiceUrlRepository>();

		/// <summary>
		/// Gets <see cref="T:SchedulerBot.Database.Interfaces.Repositories.IScheduledMessageEventRepository" />.
		/// </summary>
		public IScheduledMessageEventRepository ScheduledMessageEvents => GetRepository<IScheduledMessageEventRepository>();

		/// <summary>
		/// Gets <see cref="T:SchedulerBot.Database.Interfaces.Repositories.IScheduledMessageRepository" />.
		/// </summary>
		public IScheduledMessageRepository ScheduledMessages => GetRepository<IScheduledMessageRepository>();

		/// <summary>
		/// Gets <see cref="T:SchedulerBot.Database.Interfaces.Repositories.IServiceUrlRepository" />.
		/// </summary>
		public IServiceUrlRepository ServiceUrls => GetRepository<IServiceUrlRepository>();

		#endregion MsSQL

		#endregion

		/// <summary>
		/// Save pending changes to the database
		/// </summary>
		public void SaveChanges()
		{
			try
			{
				dbContext.SaveChanges();
			}
			catch (Exception e)
			{
				//TODO: Log Error
				throw;
			}
		}

		private T GetRepository<T>() where T : class
		{
			T repository = lifetimeScope.Resolve<T>();
			return repository;
		}
	}
}
