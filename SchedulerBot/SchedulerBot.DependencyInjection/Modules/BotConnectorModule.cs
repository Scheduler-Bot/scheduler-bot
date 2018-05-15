using Autofac;
using SchedulerBot.Infrastructure.BotConnector;
using SchedulerBot.Infrastructure.Interfaces.BotConnector;

namespace SchedulerBot.DependencyInjection.Modules
{
	/// <summary>
	/// Register bot connector dependencies.
	/// </summary>
	/// <seealso cref="Module" />
	internal class BotConnectorModule : Module
	{
		/// <inheritdoc />
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			builder
				.RegisterType<MessageProcessor>()
				.As<IMessageProcessor>()
				.SingleInstance();
		}
	}
}
