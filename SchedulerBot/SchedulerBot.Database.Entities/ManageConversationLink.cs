using System;
using SchedulerBot.Database.Entities.Interfaces;

namespace SchedulerBot.Database.Entities
{
	/// <summary>
	/// Represents the temporary link allowing a user to manage a conversation.
	/// </summary>
	/// <seealso cref="ICreatedOn" />
	public class ManageConversationLink : ICreatedOn
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the channel identifier.
		/// </summary>
		public string ChannelId { get; set; }

		/// <summary>
		/// Gets or sets the conversation identifier.
		/// </summary>
		public string ConversationId { get; set; }

		/// <summary>
		/// Gets or sets the link text.
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this link has been visited.
		/// </summary>
		public bool IsVisited { get; set; }

		/// <summary>
		/// Gets or sets the entity creation time.
		/// </summary>
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Gets or sets the link expiration time.
		/// </summary>
		public DateTime ExpiresOn { get; set; }
	}
}
