using System.Threading.Tasks;
using SchedulerBot.Business.Entities;

namespace SchedulerBot.Business.Interfaces.Services
{
	public interface IManageConversationLinkService
	{
		Task<CommandExecutionResult> ValidateAndMarkVisited(string manageId);
	}
}
