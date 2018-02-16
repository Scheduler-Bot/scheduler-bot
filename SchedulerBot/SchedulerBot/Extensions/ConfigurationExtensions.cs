using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;
using SchedulerBot.Database.Core;

namespace SchedulerBot.Extensions
{
	public static class ConfigurationExtensions
	{
		public static IConfigurationBuilder AddSecrets(this IConfigurationBuilder builder)
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

			return builder;
		}

		public static IWebHost EnsureDatabaseMigrated(this IWebHost host)
		{
			using (IServiceScope scope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
				IServiceProvider scopeServiceProvider = scope.ServiceProvider;
				SchedulerBotContext context = scopeServiceProvider.GetRequiredService<SchedulerBotContext>();

				context.Database.Migrate();
			}

			return host;
		}

		private static string GetKeyVaultEndpoint() => Environment.GetEnvironmentVariable("KEYVAULT_ENDPOINT");
	}
}
