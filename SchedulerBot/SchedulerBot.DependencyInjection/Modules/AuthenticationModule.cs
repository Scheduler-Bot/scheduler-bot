using Autofac;
using SchedulerBot.Infrastructure.Authentication;

namespace SchedulerBot.DependencyInjection.Modules
{
	/// <summary>
	/// Registers authentication dependencies.
	/// </summary>
	/// <seealso cref="Module" />
	internal class AuthenticationModule : Module
	{
		/// <inheritdoc />
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			builder
				.RegisterAssemblyTypes(typeof(JwtTokenGenerator).Assembly)
				.AsImplementedInterfaces()
				.InstancePerLifetimeScope();
		}
	}
}
