using Autofac;
using SchedulerBot.Database.Interfaces;

namespace SchedulerBot.DependencyInjection.Modules
{
	/// <summary>
	/// Registers the business services.
	/// </summary>
	/// <seealso cref="Module" />
	internal class DatabaseModule : Module
	{
		/// <inheritdoc />
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			base.Load(builder);

			builder
				.RegisterAssemblyTypes(typeof(IUnitOfWork).Assembly)
				.AsImplementedInterfaces()
				.InstancePerLifetimeScope();
		}
	}
}
