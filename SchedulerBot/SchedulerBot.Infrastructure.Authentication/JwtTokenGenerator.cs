using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
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
			base64SigningKey = configuration["Secrets:Authentication:0:SigningKey"];
			issuer = configuration["Secrets:Authentication:0:Issuer"];
			audience = configuration["Secrets:Authentication:0:Audience"];
			expirationPeriod = TimeSpan.Parse(configuration["Secrets:Authentication:0:ExpirationPeriod"], CultureInfo.InvariantCulture);
		}

		#endregion

		#region Implementation of IJwtTokenGenerator

		/// <inheritdoc />
		public string GenerateToken(string username)
		{
			SecurityTokenDescriptor tokenDescriptor = CreateTokenDescriptor(username);
			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}

		#endregion

		#region Private Methods

		private SecurityTokenDescriptor CreateTokenDescriptor(string username)
		{
			SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Convert.FromBase64String(base64SigningKey));
			SigningCredentials credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
			DateTime currentDateTime = DateTime.UtcNow;
			SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = CreateIdentity(username),
				Audience = audience,
				Issuer = issuer,
				IssuedAt = currentDateTime,
				NotBefore = currentDateTime,
				Expires = currentDateTime + expirationPeriod,
				SigningCredentials = credentials
			};

			return tokenDescriptor;
		}

		private static ClaimsIdentity CreateIdentity(string username)
		{
			return new ClaimsIdentity(new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString("N")),
				new Claim(JwtRegisteredClaimNames.Jti, username)
			});
		}

		#endregion
	}
}
