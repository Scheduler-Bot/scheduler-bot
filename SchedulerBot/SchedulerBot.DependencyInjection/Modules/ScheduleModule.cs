using Autofac;
using SchedulerBot.Infrastructure.Schedule;

namespace SchedulerBot.DependencyInjection.Modules
{
	/// <summary>
	/// Registers the dependencies related to scheduling.
	/// </summary>
	/// <seealso cref="Module" />
	internal class ScheduleModule : Module
	{
		/// <inheritdoc />
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			builder
				.RegisterAssemblyTypes(typeof(CronSchedule).Assembly)
				.AsImplementedInterfaces()
				.InstancePerLifetimeScope();
		}
	}
}
