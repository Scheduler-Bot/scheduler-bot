using SchedulerBot.Database.Entities;

namespace SchedulerBot.Database.Interfaces.Repositories
{
	/// <summary>
	/// Repository for <see cref="ScheduledMessage"/>.
	/// </summary>
	public interface IScheduledMessageRepository : IRepository<ScheduledMessage>
	{
	}
}
