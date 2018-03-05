using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.DependencyInjection;

namespace SchedulerBot.Web.UI
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSpaStaticFiles(options => options.RootPath = "wwwroot");
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			bool isDevelopment = env.IsDevelopment();

			if (!isDevelopment)
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseStaticFiles();
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
