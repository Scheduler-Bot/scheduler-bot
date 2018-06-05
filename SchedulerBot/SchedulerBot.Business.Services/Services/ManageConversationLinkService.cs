using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SchedulerBot.Business.Entities;
using SchedulerBot.Business.Interfaces.Services;
using SchedulerBot.Database.Entities;
using SchedulerBot.Database.Interfaces;

namespace SchedulerBot.Business.Services
{
	public class ManageConversationLinkService : BaseService, IManageConversationLinkService
	{
		public ManageConversationLinkService(IUnitOfWork unitOfWork, ILogger logger)
			: base(unitOfWork, logger)
		{
		}

		public async Task<CommandExecutionResult> ValidateAndMarkVisited(string manageId)
		{
			ManageConversationLink manageLink = await UnitOfWork.ManageConversationLinks.GetByTextAsync(manageId);

			if (manageLink != null && !manageLink.IsVisited && manageLink.ExpiresOn > DateTime.UtcNow)
			{
				Logger.LogInformation("Marking manage conversation id '{0}' as visited", manageId);

				manageLink.IsVisited = true;

				await UnitOfWork.SaveChangesAsync();

				return CommandExecutionResult.Success();
			}

			string errorMessage =
				$"No managing information has been found for the id '{manageId}'.Either the link does not exist or is has expired";
			Logger.LogInformation(errorMessage);
			return CommandExecutionResult.Error(errorMessage);
		}
	}
}
