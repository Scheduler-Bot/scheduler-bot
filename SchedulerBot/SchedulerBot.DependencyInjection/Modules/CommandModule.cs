using Autofac;
using SchedulerBot.Business.Commands;
using SchedulerBot.Business.Interfaces;

namespace SchedulerBot.DependencyInjection.Modules
{
	/// <summary>
	/// Registers the commands.
	/// </summary>
	/// <seealso cref="Module" />
	internal class CommandModule : Module
	{
		/// <inheritdoc />
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			builder
				.RegisterAssemblyTypes(typeof(BotCommand).Assembly)
				.AssignableTo<IBotCommand>()
				.AsImplementedInterfaces()
				.InstancePerLifetimeScope();
		}
	}
}
