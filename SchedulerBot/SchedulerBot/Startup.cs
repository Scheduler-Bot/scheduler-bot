using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Rest;
using SchedulerBot.Business.Commands;
using SchedulerBot.Business.Commands.Utils;
using SchedulerBot.Business.Interfaces;
using SchedulerBot.Business.Services;
using SchedulerBot.Database.Core;
using SchedulerBot.Infrastructure.Interfaces;
using SchedulerBot.Infrastructure.Utils;

namespace SchedulerBot
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			string appId = Configuration["Secrets:MicrosoftAppIdKey"];
			string appPassword = Configuration["Secrets:MicrosoftAppPassword"];
			SimpleCredentialProvider credentialProvider = new SimpleCredentialProvider(appId, appPassword);

			services.AddAuthentication(
				options =>
				{
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddBotAuthentication(credentialProvider);

			services.AddSingleton<ICredentialProvider>(credentialProvider);
			services.AddSingleton<ServiceClientCredentials>(new MicrosoftAppCredentials(appId, appPassword));

			string connectionString = Configuration.GetConnectionString("SchedulerBotDatabase");

			services.AddDbContext<SchedulerBotContext>(builder => builder.UseSqlite(connectionString));
			services.AddMvc(options => options.Filters.Add<TrustServiceUrlAttribute>());
			services.AddSingleton<IHostedService, ScheduledMessageProcessorService>();
			services.AddTransient<IScheduleParser, CronScheduleParser>();
			services.AddTransient<IScheduleDescriptionFormatter, CronDescriptionFormatter>();
			services.AddTransient<ICommandSelector, CommandSelector>();
			services.AddTransient<ICommandRequestParser, CommandRequestParser>();
			services.AddTransient<IList<IBotCommand>>(provider => new[]
			{
				CreateAddCommand(provider)
			});
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceScopeFactory scopeFactory)
		{
			app.UseDefaultFiles();
			app.UseStaticFiles();
			app.UseAuthentication();
			app.UseMvc();

			using (IServiceScope scope = scopeFactory.CreateScope())
			{
				IServiceProvider scopeServiceProvider = scope.ServiceProvider;
				SchedulerBotContext context = scopeServiceProvider.GetRequiredService<SchedulerBotContext>();

				context.Database.Migrate();
			}
		}

		private static IBotCommand CreateAddCommand(IServiceProvider serviceProvider)
		{
			return new AddCommand(
				serviceProvider.GetRequiredService<SchedulerBotContext>(),
				serviceProvider.GetRequiredService<IScheduleParser>(),
				serviceProvider.GetRequiredService<IScheduleDescriptionFormatter>());
		}
	}
}
