using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using SchedulerBot.Extensions;

namespace SchedulerBot
{
	public class Program
	{
		public static void Main(string[] args) =>
			BuildWebHost(args)
				.EnsureDatabaseMigrated()
				.Run();

		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration((ctx, builder) => builder.AddAzureSecrets())
				.UseStartup<Startup>()
				.Build();
	}
}
