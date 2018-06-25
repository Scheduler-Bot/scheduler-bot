using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchedulerBot.Database.Entities;

namespace SchedulerBot.Database.Interfaces.Repositories
{
	/// <summary>
	/// Repository for <see cref="ScheduledMessageEvent"/>.
	/// </summary>
	public interface IScheduledMessageEventRepository : IRepository<ScheduledMessageEvent>
	{
		/// <summary>
		/// Gets the next message event by <paramref name="conversationId"/>.
		/// </summary>
		/// <param name="conversationId">The conversation identifier.</param>
		/// <returns>
		/// A task that represents the asynchronous operation.
		/// The task result contains the found <see cref="ScheduledMessageEvent"/> instance.
		/// </returns>
		Task<ScheduledMessageEvent> GetNextMessageEventAsync(string conversationId);

		/// <summary>
		/// Gets all pending <see cref="ScheduledMessageEvent" /> joined with <see cref="ScheduledMessage" />.
		/// </summary>
		/// <param name="tillTime">The DateTime till which events should be provided.</param>
		/// <returns>
		/// A task that represents the asynchronous operation.
		/// The task result contains the collection of found <see cref="ScheduledMessageEvent"/> instances.
		/// </returns>
		Task<IList<ScheduledMessageEvent>> GetAllPendingWithScheduledMessages(DateTime tillTime);
	}
}
