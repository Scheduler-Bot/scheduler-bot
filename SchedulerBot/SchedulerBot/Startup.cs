using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Bot.Connector;
using SchedulerBot.DependencyInjection;
using SchedulerBot.Extensions;
using SchedulerBot.Infrastructure.Interfaces.Configuration;
using SchedulerBot.Middleware;

namespace SchedulerBot
{
	/// <summary>
	/// Describes how the application startup happens.
	/// </summary>
	public class Startup
	{
		private readonly IConfiguration configuration;

		/// <summary>
		/// Initializes a new instance of the <see cref="Startup"/> class.
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		public Startup(IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		/// <summary>
		/// Configures the services injected at runtime.
		/// </summary>
		/// <param name="services">The services.</param>
		/// <returns>The service provider.</returns>
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext();
//			services.AddAuthentication()
//				.AddBotAuthentication(configuration)
//				.AddManageConversationAuthentication(configuration);
//			services.AddMvc(options => options.Filters.Add<TrustServiceUrlAttribute>());
			services.AddSpaStaticFiles(options => options.RootPath = "wwwroot");

			services.AddBot<Bots.SchedulerBot>(options =>
			{
				var secretKey = configuration.GetSection("botFileSecret")?.Value;
				var botFilePath = configuration.GetSection("botFilePath")?.Value;

				// Loads .bot configuration file and adds a singleton that your Bot can access through dependency injection.
				var botConfig = BotConfiguration.Load(botFilePath ?? @".\BotConfiguration.bot", secretKey);
			});

			ServiceProviderBuilder serviceProviderBuilder = new ServiceProviderBuilder();
			IServiceProvider serviceProvider = serviceProviderBuilder.Build(services);

			configuration.Bind(serviceProvider.GetRequiredService<IApplicationConfiguration>());

			return serviceProvider;
		}

		/// <summary>
		/// Configures the specified application.
		/// </summary>
		/// <param name="app">The application.</param>
		/// <param name="env">The environment.</param>
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			app.UseDefaultFiles();
			app.UseStaticFiles();
			app.UseMiddleware<ApplicationContextMiddleware>();
			app.UseAuthentication();
			app.UseMvc();

			bool isDevelopment = env.IsDevelopment();

			if (!isDevelopment)
			{
				app.UseStatusCodePages("text/plain", "Status code page, status code: {0}");
			}
			else
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSpa(builder =>
			{
				builder.Options.SourcePath = "ClientApp";

				if (isDevelopment)
				{
					builder.UseAngularCliServer(npmScript: "start");
				}
			});
		}
	}
}
