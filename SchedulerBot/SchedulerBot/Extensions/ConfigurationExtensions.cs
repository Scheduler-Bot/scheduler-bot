﻿using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SchedulerBot.Authentication;
using SchedulerBot.Database.Core;

namespace SchedulerBot.Extensions
{
	/// <summary>
	/// Provides extension methods used during the application configuration.
	/// </summary>
	internal static class ConfigurationExtensions
	{
		/// <summary>
		/// Adds the azure secrets.
		/// </summary>
		/// <param name="builder">The builder.</param>
		/// <returns>The same <see cref="ConfigurationBinder"/> instance which has been passed to the method.</returns>
		public static IConfigurationBuilder AddAzureSecrets(this IConfigurationBuilder builder)
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

		/// <summary>
		/// Verifies whether the database is migrated and applies migrations if not.
		/// </summary>
		/// <param name="host">The host.</param>
		/// <returns>The same <see cref="IWebHost"/> instance which has been passed to the method.</returns>
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

		/// <summary>
		/// Gets the connection string from the specified configuration.
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		/// <returns>The connection string</returns>
		public static string GetConnectionString(this IConfiguration configuration)
		{
			string settingName = IsDevelopment() ? "ConnectionString" : "Secrets:ConnectionString";
			string connectionString = configuration[settingName];

			return connectionString;
		}

		/// <summary>
		/// Configures the authentication scheme used for managing conversations.
		/// </summary>
		/// <param name="builder">The builder.</param>
		/// <param name="configuration">The configuration.</param>
		/// <returns>
		/// The same authentication builder that is passed as an argument
		/// so that it can be used in further configuration chain.
		/// </returns>
		public static AuthenticationBuilder AddManageConversationAuthentication(
			this AuthenticationBuilder builder,
			IConfiguration configuration)
		{
			return builder
				.AddJwtBearer(
					ManageConversationAuthenticationConfiguration.AuthenticationSchemeName,
					ManageConversationAuthenticationConfiguration.AuthenticationSchemeDisplayName,
					options => ConfigureJwtValidation(options, configuration));
		}

		private static string GetKeyVaultEndpoint() => Environment.GetEnvironmentVariable("KEYVAULT_ENDPOINT");

		private static bool IsDevelopment()
		{
			string currentEnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

			return EnvironmentName.Development.Equals(currentEnvironmentName, StringComparison.Ordinal);
		}

		private static void ConfigureJwtValidation(JwtBearerOptions options, IConfiguration configuration)
		{
			TokenValidationParameters validationParameters = options.TokenValidationParameters;

			validationParameters.ValidateIssuer = true;
			validationParameters.ValidateIssuerSigningKey = true;
			validationParameters.ValidateAudience = true;
			validationParameters.ValidateLifetime = true;
			validationParameters.RequireSignedTokens = true;
			validationParameters.RequireExpirationTime = true;
			validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(configuration["Secrets:Authentication:0:SigningKey"]));
			validationParameters.ValidAudience = configuration["Secrets:Authentication:0:Audience"];
			validationParameters.ValidIssuer = configuration["Secrets:Authentication:0:Issuer"];
		}
	}
}
