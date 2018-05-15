using Autofac;
using Microsoft.Extensions.Hosting;
using SchedulerBot.Business.Services;

namespace SchedulerBot.DependencyInjection.Modules
{
	/// <summary>
	/// Registers the business services.
	/// </summary>
	/// <seealso cref="Module" />
	internal class BusinessServiceModule : Module
	{
		/// <inheritdoc />
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			builder
				.RegisterType<ScheduledMessageProcessorService>()
				.As<IHostedService>()
				.SingleInstance();
		}
	}
}
