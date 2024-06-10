using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using URM.Core.DTOs;
using URM.Core.Enums;
using URM.Core.Exceptions;
using URM.Core.Extensions;
using URM.Core.Models;
using URM.Core.Services;
using URM.Core.Utilities.Results;
using URM.Service.Constants;

namespace URM.Service.Services
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<AppUser> _userManager;

		private readonly IMapper _mapper;
		private readonly ITokenService _tokenService;
		private readonly AuthBusinessRules _authBusinessRules;



		public AuthService(UserManager<AppUser> userManager, IMapper mapper, ITokenService tokenService, AuthBusinessRules authBusinessRules)
		{
			_userManager = userManager;
			_mapper = mapper;
			_tokenService = tokenService;
			_authBusinessRules = authBusinessRules;
		}

		public async Task<NewUserDto> SignIn(LoginDto loginDto)
		{
			loginDto.EnsureNotNull(Messages.FillAllFields);

			var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == loginDto.UserName.ToLower());

			user.EnsureNotNull(Messages.UserNotFound);


			await _authBusinessRules.CheckPasswordAsync(user, loginDto.Password);

			var userInfo = _mapper.Map<NewUserDto>(user);
			userInfo.Token = await _tokenService.CreateToken(user);
			return userInfo;

		}

		public async Task<NewUserDto> SignUp(RegisterDto registerDto)
		{
			registerDto.EnsureNotNull(Messages.FillAllFields);

			var user = _mapper.Map<AppUser>(registerDto);
			var result = await _userManager.CreateAsync(user, registerDto.Password);

			if (result.Succeeded)
			{

				await _authBusinessRules.EnsureRoleExists(Roles.User.ToString());
				await _userManager.AddToRoleAsync(user, Roles.User.ToString());

				var token = await _tokenService.CreateToken(user);
				var newUserDto = _mapper.Map<NewUserDto>(user);
				newUserDto.Token = token;

				return newUserDto;
			}
			else
			{
				var errors = string.Join("; ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
				throw new InvalidOperationException(errors);
			}
		}
	}
}
