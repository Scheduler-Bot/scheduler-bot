using Autofac;
using SchedulerBot.Business.Commands.Utils;

namespace SchedulerBot.DependencyInjection.Modules
{
	/// <summary>
	/// Registers utilities in the business scope.
	/// </summary>
	/// <seealso cref="Module" />
	internal class BusinessUtilsModule : Module
	{
		/// <inheritdoc />
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			builder
				.RegisterAssemblyTypes(typeof(CommandSelector).Assembly)
				.Where(type => type.IsInNamespaceOf<CommandSelector>())
				.AsImplementedInterfaces()
				.InstancePerLifetimeScope();
		}
	}
}
