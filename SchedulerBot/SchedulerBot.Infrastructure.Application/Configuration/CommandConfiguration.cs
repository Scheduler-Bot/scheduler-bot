using SchedulerBot.Infrastructure.Interfaces.Configuration;

namespace SchedulerBot.Infrastructure.Application.Configuration
{
	/// <inheritdoc />
	public class CommandConfiguration : ICommandConfiguration
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CommandConfiguration"/> class.
		/// </summary>
		/// <param name="manage">The manage.</param>
		/// <param name="next">The next.</param>
		public CommandConfiguration(
			IManageCommandConfiguration manage,
			INextCommandConfiguration next)
		{
			Manage = manage;
			Next = next;
		}

		/// <inheritdoc />
		public IManageCommandConfiguration Manage { get; set; }

		/// <inheritdoc />
		public INextCommandConfiguration Next { get; set; }
	}
}
