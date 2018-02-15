using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;
using SchedulerBot.Database.Core;

namespace SchedulerBot
{
	public class Program
	{
		public static void Main(string[] args)
		{
			IWebHost host = BuildWebHost(args);

			using (IServiceScope scope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
				IServiceProvider scopeServiceProvider = scope.ServiceProvider;
				SchedulerBotContext context = scopeServiceProvider.GetRequiredService<SchedulerBotContext>();

				context.Database.Migrate();
			}

			host.Run();
		}

		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration((ctx, builder) =>
				{
					string keyVaultEndpoint = GetKeyVaultEndpoint();
					if (!string.IsNullOrEmpty(keyVaultEndpoint))
					{
						AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
						KeyVaultClient keyVaultClient = new KeyVaultClient(
							new KeyVaultClient.AuthenticationCallback(
								azureServiceTokenProvider.KeyVaultTokenCallback));
						builder.AddAzureKeyVault(
							keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
					}
					else
					{
						builder.AddUserSecrets<Startup>();
					}
				})
				.UseStartup<Startup>()
				.Build();

		private static string GetKeyVaultEndpoint() => Environment.GetEnvironmentVariable("KEYVAULT_ENDPOINT");
	}
}
