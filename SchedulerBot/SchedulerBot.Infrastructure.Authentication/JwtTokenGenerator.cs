using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SchedulerBot.Infrastructure.Interfaces.Authentication;

namespace SchedulerBot.Infrastructure.Authentication
{
	/// <inheritdoc />
	public class JwtTokenGenerator : IJwtTokenGenerator
	{
		#region Private Fields

		private readonly string base64SigningKey;
		private readonly string issuer;
		private readonly string audience;
		private readonly TimeSpan expirationPeriod;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="JwtTokenGenerator"/> class.
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		public JwtTokenGenerator(IConfiguration configuration)
		{
			// TODO: This is not generic. Revisit it.
			base64SigningKey = configuration["Secrets:Authentication[0]:SigningKey"];
			issuer = configuration["Secrets:Authentication[0]:Issuer"];
			audience = configuration["Secrets:Authentication[0]:Audience"];
			expirationPeriod = TimeSpan.Parse(configuration["Secrets:Authentication[0]:ExpirationPeriod"], CultureInfo.InvariantCulture);
		}

		#endregion

		#region Implementation of IJwtTokenGenerator

		/// <inheritdoc />
		public string GenerateToken(IDictionary<string, string> claims)
		{
			SecurityTokenDescriptor tokenDescriptor = CreateTokenDescriptor(claims);
			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}

		#endregion

		#region Private Methods

		private SecurityTokenDescriptor CreateTokenDescriptor(IDictionary<string, string> claims)
		{
			SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Convert.FromBase64String(base64SigningKey));
			SigningCredentials credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
			DateTime currentDateTime = DateTime.UtcNow;
			SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = CreateIdentity(claims),
				Audience = audience,
				Issuer = issuer,
				IssuedAt = currentDateTime,
				NotBefore = currentDateTime,
				Expires = currentDateTime + expirationPeriod,
				SigningCredentials = credentials
			};

			return tokenDescriptor;
		}

		private static ClaimsIdentity CreateIdentity(IDictionary<string, string> claims)
		{
			ClaimsIdentity identity = new ClaimsIdentity();

			identity.AddClaims(CreateDefaultClaims());
			identity.AddClaims(CreateUserDefinedClaims(claims));

			return identity;
		}

		private static IEnumerable<Claim> CreateDefaultClaims()
		{
			yield return new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N"));
		}

		private static IEnumerable<Claim> CreateUserDefinedClaims(IDictionary<string, string> claimsDictionary)
		{
			return claimsDictionary.Select(pair => new Claim(pair.Key, pair.Value));
		}

		#endregion
	}
}
