using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using SchedulerBot.Infrastructure.Interfaces.Authentication;
using SchedulerBot.Infrastructure.Interfaces.Configuration;

namespace SchedulerBot.Infrastructure.Authentication
{
	/// <inheritdoc />
	public class JwtTokenGenerator : IJwtTokenGenerator
	{
		#region Private Fields

		private readonly IAuthenticationConfiguration configuration;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="JwtTokenGenerator"/> class.
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		public JwtTokenGenerator(IAuthenticationConfiguration configuration)
		{
			this.configuration = configuration;
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
			SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Convert.FromBase64String(configuration.SigningKey));
			SigningCredentials credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
			DateTime currentDateTime = DateTime.UtcNow;
			SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = CreateIdentity(username),
				Audience = configuration.Audience,
				Issuer = configuration.Audience,
				IssuedAt = currentDateTime,
				NotBefore = currentDateTime,
				Expires = currentDateTime + configuration.ExpirationPeriod,
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
