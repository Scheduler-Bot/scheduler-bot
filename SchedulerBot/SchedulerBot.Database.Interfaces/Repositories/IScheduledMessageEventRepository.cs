using SchedulerBot.Database.Entities;

namespace SchedulerBot.Database.Interfaces.Repositories
{
	/// <summary>
	/// Repository for <see cref="ScheduledMessageEvent"/>.
	/// </summary>
	public interface IScheduledMessageEventRepository : IRepository<ScheduledMessageEvent>
	{
	}
}
