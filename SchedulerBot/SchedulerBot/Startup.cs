using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Authentication;
using SchedulerBot.Business.Commands;
using SchedulerBot.Business.Commands.Utils;
using SchedulerBot.Business.Entities;
using SchedulerBot.Business.Interfaces;
using SchedulerBot.Business.Services;
using SchedulerBot.Database.Core;
using SchedulerBot.Extensions;
using SchedulerBot.Infrastructure.BotConnector;
using SchedulerBot.Infrastructure.Interfaces.BotConnector;
using SchedulerBot.Infrastructure.Interfaces.Schedule;
using SchedulerBot.Infrastructure.Schedule;

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
		public void ConfigureServices(IServiceCollection services)
		{
			string appId = configuration["Secrets:MicrosoftAppIdKey"];
			string appPassword = configuration["Secrets:MicrosoftAppPassword"];
			SimpleCredentialProvider credentialProvider = new SimpleCredentialProvider(appId, appPassword);

			services.AddAuthentication(
				options =>
				{
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddBotAuthentication(credentialProvider);

			services.AddSingleton(new AppCredentials(appId, appPassword));

			string connectionString = configuration.GetConnectionString();

			services.AddDbContext<SchedulerBotContext>(builder => builder.UseSqlServer(connectionString));

			services.AddMvc(options => options.Filters.Add<TrustServiceUrlAttribute>());

			services.AddSingleton<IHostedService, ScheduledMessageProcessorService>();

			services.AddSingleton<IMessageProcessor, MessageProcessor>();

			services.AddTransient<IScheduleParser, CronScheduleParser>();
			services.AddTransient<IScheduleDescriptionFormatter, CronDescriptionFormatter>();
			services.AddTransient<ICommandSelector, CommandSelector>();
			services.AddTransient<ICommandRequestParser, CommandRequestParser>();
			services.AddTransient<AddCommand>();
			services.AddTransient<RemoveCommand>();
			services.AddTransient<ListCommand>();
			services.AddTransient<EchoCommand>();
			services.AddTransient<NextCommand>();
			services.AddTransient<IList<IBotCommand>>(provider => new IBotCommand[]
			{
				provider.GetRequiredService<AddCommand>(),
				provider.GetRequiredService<RemoveCommand>(),
				provider.GetRequiredService<ListCommand>(),
				provider.GetRequiredService<EchoCommand>(),
				provider.GetRequiredService<NextCommand>()
			});

			services.AddSpaStaticFiles(options => options.RootPath = "wwwroot");
		}

		/// <summary>
		/// Configures the specified application.
		/// </summary>
		/// <param name="app">The application.</param>
		/// <param name="env">The environment.</param>
		/// <param name="scopeFactory">The scope factory.</param>
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceScopeFactory scopeFactory)
		{
			app.UseDefaultFiles();
			app.UseStaticFiles();
			app.UseAuthentication();
			app.UseMvc();
			app.UseExceptionHandler();

			bool isDevelopment = env.IsDevelopment();

			if (!isDevelopment)
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
