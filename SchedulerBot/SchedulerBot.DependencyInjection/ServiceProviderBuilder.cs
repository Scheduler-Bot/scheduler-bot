using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

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

			return serviceProvider;
		}
	}
}
