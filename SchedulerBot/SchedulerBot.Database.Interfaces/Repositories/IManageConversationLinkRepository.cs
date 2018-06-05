using System.Threading.Tasks;
using SchedulerBot.Database.Entities;

namespace SchedulerBot.Database.Interfaces.Repositories
{
	/// <summary>
	/// Repository for <see cref="ManageConversationLink"/>.
	/// </summary>
	public interface IManageConversationLinkRepository: IRepository<ManageConversationLink>
	{
		/// <summary>
		/// Gets the <see cref="ManageConversationLink"/> by <paramref name="text"/>.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns></returns>
		Task<ManageConversationLink> GetByTextAsync(string text);
	}
}
