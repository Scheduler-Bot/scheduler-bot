using System.Collections.Generic;
using System.Threading.Tasks;
using SchedulerBot.Database.Entities;

namespace SchedulerBot.Business.Interfaces.Services
{
	/// <summary>
	/// Representation of the Business Logic Layer for <see cref="ScheduledMessageDetails"/>.
	/// </summary>
	public interface IScheduledMessageDetailsService
	{
		/// <summary>
		/// Gets <see cref="IList{ScheduledMessageDetails}"/> for provided <paramref name="manageLink"/>.
		/// </summary>
		/// <param name="manageLink">The manage link.</param>
		/// <returns></returns>
		Task<IList<ScheduledMessageDetails>> GetByManageConversationLinkAsync(ManageConversationLink manageLink);
	}
}
