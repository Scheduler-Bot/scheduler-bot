using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SchedulerBot.Business.Services;
using SchedulerBot.Database.Core;

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
			string appId = Configuration[MicrosoftAppCredentials.MicrosoftAppIdKey];
			string appPassword = Configuration[MicrosoftAppCredentials.MicrosoftAppPasswordKey];
			SimpleCredentialProvider credentialProvider = new SimpleCredentialProvider(appId, appPassword);

			services.AddAuthentication(
				options =>
				{
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddBotAuthentication(credentialProvider);
			services.AddSingleton<ICredentialProvider>(credentialProvider);

			string connectionString = Configuration.GetConnectionString("SchedulerBotDatabase");

			services.AddDbContext<SchedulerBotContext>(builder => builder.UseSqlite(connectionString));
			services.AddMvc(options => options.Filters.Add<TrustServiceUrlAttribute>());
			services.AddSingleton<IHostedService, ScheduledMessageProcessorService>();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			app.UseStaticFiles();
			app.UseAuthentication();
			app.UseMvc();
		}
	}
}
