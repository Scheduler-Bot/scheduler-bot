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
	internal static class ConfigurationExtensions
	{
		internal static IConfigurationBuilder AddAzureSecrets(this IConfigurationBuilder builder)
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

			return builder;
		}

		internal static IWebHost EnsureDatabaseMigrated(this IWebHost host)
		{
			using (IServiceScope scope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
				IServiceProvider scopeServiceProvider = scope.ServiceProvider;
				SchedulerBotContext context = scopeServiceProvider.GetRequiredService<SchedulerBotContext>();

				context.Database.Migrate();
			}

			return host;
		}

		internal static string GetConnectionString(this IConfiguration configuration)
		{
			string settingName = IsDevelopment() ? "ConnectionString" : "Secrets:ConnectionString";
			string connectionString = configuration[settingName];

			return connectionString;
		}

		private static string GetKeyVaultEndpoint() => Environment.GetEnvironmentVariable("KEYVAULT_ENDPOINT");

		private static bool IsDevelopment()
		{
			string currentEnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

			return EnvironmentName.Development.Equals(currentEnvironmentName, StringComparison.Ordinal);
		}
	}
}
