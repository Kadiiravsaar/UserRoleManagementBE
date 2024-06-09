using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URM.Core.Services;
using URM.Core.Models;
using Microsoft.EntityFrameworkCore;
using URM.Core.DTOs;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using URM.Service.Constants;
using URM.Core.Utilities.Results;
using URM.Core.Extensions;

namespace URM.Service.Services
{
	public class UserService : IUserService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IMapper _mapper;
		public UserService(UserManager<AppUser> userManager, IMapper mapper)
		{
			_userManager = userManager;
			_mapper = mapper;
		}
		public async Task<IDataResult<UserDetailDto>> GetUserByIdAsync(string userId)
		{
			userId.EnsureNotNull(Messages.FillAllFields); // Validation Extension kullanımı

			var user = await _userManager.FindByIdAsync(userId);
			user.EnsureNotNull(Messages.UserNotFound); // Validation Extension kullanımı

			var roles = await _userManager.GetRolesAsync(user);

			var userDetailDto = _mapper.Map<UserDetailDto>(user);
			userDetailDto.Roles = roles.ToList(); // Rolleri manuel olarak ekliyoruz

			return new SuccessDataResult<UserDetailDto>(userDetailDto);
		}


		
		public async Task<IDataResult<List<UserDto>>> UserList()
		{
			var users = await _userManager.Users.ToListAsync();
			var usersDto = _mapper.Map<List<UserDto>>(users);
			return new SuccessDataResult<List<UserDto>>(usersDto);
		}
	}

}
