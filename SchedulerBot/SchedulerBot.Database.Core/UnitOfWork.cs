using System;
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
		#region Private Fields

		private readonly SchedulerBotContext dbContext;
		private readonly ILifetimeScope lifetimeScope;
		private readonly Lazy<IManageConversationLinkRepository> lazyManageConversationLinks;
		private readonly Lazy<IScheduledMessageDetailsRepository> lazyScheduledMessageDetails;
		private readonly Lazy<IScheduledMessageDetailsServiceUrlRepository> lazyScheduledMessageDetailsServiceUrls;
		private readonly Lazy<IScheduledMessageEventRepository> lazyScheduledMessageEvents;
		private readonly Lazy<IScheduledMessageRepository> lazyScheduledMessages;
		private readonly Lazy<IServiceUrlRepository> lazyServiceUrls;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="UnitOfWork"/> class.
		/// </summary>
		/// <param name="dbContext">The database context.</param>
		/// <param name="lifetimeScope">The lifetime scope.</param>
		public UnitOfWork(SchedulerBotContext dbContext, ILifetimeScope lifetimeScope)
		{
			this.dbContext = dbContext;
			this.lifetimeScope = lifetimeScope;

			lazyManageConversationLinks = InitializeLazyRepository<IManageConversationLinkRepository>();
			lazyScheduledMessageDetails = InitializeLazyRepository<IScheduledMessageDetailsRepository>();
			lazyScheduledMessageDetailsServiceUrls = InitializeLazyRepository<IScheduledMessageDetailsServiceUrlRepository>();
			lazyScheduledMessageEvents = InitializeLazyRepository<IScheduledMessageEventRepository>();
			lazyScheduledMessages = InitializeLazyRepository<IScheduledMessageRepository>();
			lazyServiceUrls = InitializeLazyRepository<IServiceUrlRepository>();
		}

		#endregion

		#region Repositories

		#region MS SQL

		/// <inheritdoc/>
		public IManageConversationLinkRepository ManageConversationLinks => lazyManageConversationLinks.Value;

		/// <inheritdoc/>
		public IScheduledMessageDetailsRepository ScheduledMessageDetails => lazyScheduledMessageDetails.Value;

		/// <inheritdoc/>
		public IScheduledMessageDetailsServiceUrlRepository ScheduledMessageDetailsServiceUrls => lazyScheduledMessageDetailsServiceUrls.Value;

		/// <inheritdoc/>
		public IScheduledMessageEventRepository ScheduledMessageEvents => lazyScheduledMessageEvents.Value;

		/// <inheritdoc/>
		public IScheduledMessageRepository ScheduledMessages => lazyScheduledMessages.Value;

		/// <inheritdoc/>
		public IServiceUrlRepository ServiceUrls => lazyServiceUrls.Value;

		#endregion MsSQL

		#endregion

		/// <inheritdoc/>
		public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			return dbContext.SaveChangesAsync(cancellationToken);
		}

		#region Private Methods

		private Lazy<T> InitializeLazyRepository<T>() where T : class
		{
			Lazy<T> lazyRepository = new Lazy<T>(GetRepository<T>);

			return lazyRepository;
		}

		private T GetRepository<T>() where T : class
		{
			T repository = lifetimeScope.Resolve<T>();

			return repository;
		}

		#endregion
	}
}
