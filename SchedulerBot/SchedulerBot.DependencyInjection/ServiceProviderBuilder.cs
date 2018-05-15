using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchedulerBot.Infrastructure.Interfaces.Configuration;

namespace SchedulerBot.DependencyInjection
{
	/// <summary>
	/// Provides an ability to build the service provider with necessary component registrations.
	/// </summary>
	public class ServiceProviderBuilder
	{
		/// <summary>
		/// Creates a service provider with the registered dependencies.
		/// </summary>
		/// <param name="services">The services.</param>
		/// <returns>The built service provider.</returns>
		public IServiceProvider Build(IServiceCollection services)
		{
			ContainerBuilder builder = new ContainerBuilder();

			builder.Populate(services);
			builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());

			IContainer container = builder.Build();
			IServiceProvider serviceProvider = new AutofacServiceProvider(container);

			InitializeConfiguration(serviceProvider);

			return serviceProvider;
		}

		private static void InitializeConfiguration(IServiceProvider serviceProvider)
		{
			IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
			IApplicationConfiguration applicationConfiguration = serviceProvider.GetRequiredService<IApplicationConfiguration>();

			// Initialize application configuration.
			configuration.Bind(applicationConfiguration);
		}
	}
}
