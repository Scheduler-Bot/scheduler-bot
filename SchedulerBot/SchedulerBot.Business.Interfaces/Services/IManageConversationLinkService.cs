using System.Threading.Tasks;
using SchedulerBot.Business.Entities;
using SchedulerBot.Database.Entities;

namespace SchedulerBot.Business.Interfaces.Services
{
	/// <summary>
	/// Representation of the Business Logic Layer for <see cref="ManageConversationLink"/>.
	/// </summary>
	public interface IManageConversationLinkService
	{
		/// <summary>
		/// Validates and mark visited <see cref="ManageConversationLink"/> selected by <paramref name="manageId"/> asynchronous.
		/// </summary>
		/// <param name="manageId">The identifier of <see cref="ManageConversationLink"/>.</param>
		Task<ExecutionResult> ValidateAndMarkVisitedAsync(string manageId);

		/// <summary>
		/// Gets a <see cref="ManageConversationLink"/> by <paramref name="text"/> asynchronous.
		/// </summary>
		/// <param name="text">The text of <see cref="ManageConversationLink"/>.</param>
		Task<ManageConversationLink> GetByTextAsync(string text);
	}
}
