using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using URM.Core.Models;
using URM.Core.Services;

namespace URM.Service.Services
{
	public class TokenService : ITokenService
	{
		private readonly IConfiguration _configuration;
		private readonly UserManager<AppUser> _userManager;
		private readonly SymmetricSecurityKey _key;

		public TokenService(IConfiguration configuration, UserManager<AppUser> userManager)
		{
			_configuration = configuration;
			_userManager = userManager;
			_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]));
		}


		public async Task<string> CreateToken(AppUser appUser)
		{
			var roles = await _userManager.GetRolesAsync(appUser);
			var claims = new List<Claim>
				{

					new Claim(JwtRegisteredClaimNames.NameId, appUser.Id),
					new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
					new Claim(JwtRegisteredClaimNames.GivenName, appUser.UserName)

				};

			// Rol bilgilerini Claim olarak ekle
			claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

			var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
			var tokenDescription = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(7),
				SigningCredentials = creds,
				Issuer = _configuration["JWT:Issuer"],
				Audience = _configuration["JWT:Audience"]
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescription);
			return tokenHandler.WriteToken(token);
		}

	}
}
