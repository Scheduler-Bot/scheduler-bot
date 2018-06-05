using System.Threading.Tasks;
using SchedulerBot.Database.Interfaces.Repositories;

namespace SchedulerBot.Database.Interfaces
{
	/// <summary>
	/// Unit of Work for a Database access.
	/// </summary>
	public interface IUnitOfWork
	{
		/// <summary>
		/// Gets <see cref="IManageConversationLinkRepository"/>.
		/// </summary>
		IManageConversationLinkRepository ManageConversationLinks { get; }

		/// <summary>
		/// Gets <see cref="IScheduledMessageDetailsRepository"/>.
		/// </summary>
		IScheduledMessageDetailsRepository ScheduledMessageDetails { get; }

		/// <summary>
		/// Gets <see cref="IScheduledMessageDetailsServiceUrlRepository"/>.
		/// </summary>
		IScheduledMessageDetailsServiceUrlRepository ScheduledMessageDetailsServiceUrls { get; }

		/// <summary>
		/// Gets <see cref="IScheduledMessageEventRepository"/>.
		/// </summary>
		IScheduledMessageEventRepository ScheduledMessageEvents { get; }

		/// <summary>
		/// Gets <see cref="IScheduledMessageRepository"/>.
		/// </summary>
		IScheduledMessageRepository ScheduledMessages { get; }

		/// <summary>
		/// Gets <see cref="IServiceUrlRepository"/>.
		/// </summary>
		IServiceUrlRepository ServiceUrls { get; }

		/// <summary>
		/// Saves all changes made in this context to the database.
		/// </summary>
		void SaveChanges();

		/// <summary>
		/// Asynchronously saves all changes made in this context to the database.
		/// </summary>
		/// <returns>
		///     A task that represents the asynchronous save operation. The task result contains the
		///     number of state entries written to the database.
		/// </returns>
		Task<int> SaveChangesAsync();
	}
}
