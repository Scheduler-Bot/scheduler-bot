using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SchedulerBot.Business.Interfaces.Services;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Interfaces;

namespace SchedulerBot.Business.Services
{
	/// <inheritdoc cref="IScheduledMessageDetailsService"/>
	public class ScheduledMessageDetailsService : BaseService, IScheduledMessageDetailsService
	{
		public ScheduledMessageDetailsService(IUnitOfWork unitOfWork, ILogger logger)
			: base(unitOfWork, logger)
		{
		}

		/// <inheritdoc />
		public async Task<IList<ScheduledMessageDetails>> GetByManageConversationLinkAsync(ManageConversationLink manageLink)
		{
			IList<ScheduledMessageDetails> scheduledMessageDetails =
				await UnitOfWork.ScheduledMessageDetails.GetScheduledMessageDetailsWithEventsAsync(manageLink.ChannelId, manageLink.ConversationId);
			return scheduledMessageDetails;
		}
	}
}
