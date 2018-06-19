using System.Threading;
using System.Threading.Tasks;
using Autofac;
using SchedulerBot.Database.Interfaces;
using SchedulerBot.Database.Interfaces.Repositories;

namespace SchedulerBot.Database.Core
{
	/// <inheritdoc/>
	public class UnitOfWork : IUnitOfWork
	{
		private readonly SchedulerBotContext dbContext;
		private readonly ILifetimeScope lifetimeScope;

		/// <summary>
		/// Initializes a new instance of the <see cref="UnitOfWork"/> class.
		/// </summary>
		/// <param name="dbContext">The database context.</param>
		/// <param name="lifetimeScope">The lifetime scope.</param>
		public UnitOfWork(SchedulerBotContext dbContext, ILifetimeScope lifetimeScope)
		{
			this.dbContext = dbContext;
			this.lifetimeScope = lifetimeScope;

			ManageConversationLinks = GetRepository<IManageConversationLinkRepository>();
			ScheduledMessageDetails = GetRepository<IScheduledMessageDetailsRepository>();
			ScheduledMessageDetailsServiceUrls = GetRepository<IScheduledMessageDetailsServiceUrlRepository>();
			ScheduledMessageEvents = GetRepository<IScheduledMessageEventRepository>();
			ScheduledMessages = GetRepository<IScheduledMessageRepository>();
			ServiceUrls = GetRepository<IServiceUrlRepository>();
		}

		#region Repositories

		#region MS SQL

		/// <inheritdoc/>
		public IManageConversationLinkRepository ManageConversationLinks { get; }

		/// <inheritdoc/>
		public IScheduledMessageDetailsRepository ScheduledMessageDetails { get; }

		/// <inheritdoc/>
		public IScheduledMessageDetailsServiceUrlRepository ScheduledMessageDetailsServiceUrls { get; }

		/// <inheritdoc/>
		public IScheduledMessageEventRepository ScheduledMessageEvents { get; }

		/// <inheritdoc/>
		public IScheduledMessageRepository ScheduledMessages { get; }

		/// <inheritdoc/>
		public IServiceUrlRepository ServiceUrls { get; }

		#endregion MsSQL

		#endregion

		/// <inheritdoc/>
		public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			return dbContext.SaveChangesAsync(cancellationToken);
		}

		private T GetRepository<T>() where T : class
		{
			T repository = lifetimeScope.Resolve<T>();
			return repository;
		}
	}
}
