using Autofac;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Interfaces;
using SchedulerBot.Database.Repositories;

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

			builder
				.RegisterType<UnitOfWork>()
				.As<IUnitOfWork>()
				.InstancePerLifetimeScope();

			builder
				.RegisterAssemblyTypes(typeof(BaseRepository<>).Assembly)
				.AsImplementedInterfaces()
				.InstancePerLifetimeScope();
		}
	}
}
