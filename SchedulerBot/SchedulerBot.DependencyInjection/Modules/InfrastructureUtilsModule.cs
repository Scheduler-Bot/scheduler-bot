using Autofac;
using SchedulerBot.Infrastructure.Utils;

namespace SchedulerBot.DependencyInjection.Modules
{
	/// <summary>
	/// Registers utilities related to infrastructure.
	/// </summary>
	/// <seealso cref="Module" />
	internal class InfrastructureUtilsModule : Module
	{
		/// <inheritdoc />
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			builder
				.RegisterAssemblyTypes(typeof(WebUtility).Assembly)
				.AsImplementedInterfaces()
				.InstancePerLifetimeScope();
		}
	}
}
