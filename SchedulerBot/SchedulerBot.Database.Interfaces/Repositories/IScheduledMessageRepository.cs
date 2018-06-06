﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Entities.Enums;

namespace SchedulerBot.Database.Interfaces.Repositories
{
	/// <summary>
	/// Repository for <see cref="ScheduledMessage"/>.
	/// </summary>
	public interface IScheduledMessageRepository : IRepository<ScheduledMessage>
	{
		/// <summary>
		/// Gets the list of ScheduledMessages by <paramref name="conversationId"/> and <paramref name="state"/>.
		/// </summary>
		/// <param name="conversationId">The conversation identifier.</param>
		/// <param name="state">The state.</param>
		/// <returns>
		/// A task that represents the asynchronous operation.
		/// The task result contains the collection of found <see cref="ScheduledMessage"/> instances.
		/// </returns>
		Task<IList<ScheduledMessage>> GetByConversationIdAndStateAsync(
			string conversationId,
			ScheduledMessageState state);

		/// <summary>
		/// Gets the <see cref="ScheduledMessage"/> in active status by <paramref name="conversationId"/> and <paramref name="messageId"/> asynchronous.
		/// </summary>
		/// <param name="conversationId">The conversation identifier.</param>
		/// <param name="messageId">The message identifier.</param>
		/// <returns>
		/// A task that represents the asynchronous operation.
		/// The task result contains the found <see cref="ScheduledMessage"/> instance.
		/// </returns>
		Task<ScheduledMessage> GetActiveByIdAndConversationIdAsync(
			string conversationId,
			Guid messageId);

		/// <summary>
		/// Gets the <see cref="ScheduledMessage"/> in active state by <paramref name="messageId"/> with events asynchronous.
		/// </summary>
		/// <param name="messageId">The message identifier.</param>
		/// <returns>
		/// A task that represents the asynchronous operation.
		/// The task result contains the found <see cref="ScheduledMessage"/> instance.
		/// </returns>
		Task<ScheduledMessage> GetActiveByIdWithEventsAsync(Guid messageId);
	}
}
