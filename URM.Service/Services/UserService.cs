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
using URM.Core.Ultities.Results;
using URM.Service.Constants;

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
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				throw new KeyNotFoundException("Kullanıcı bulunamadı.");
			} // burada iş kuralını düzgün yaz

			var roles = await _userManager.GetRolesAsync(user);


			// burada mapp kullan
			var userDetailDto = new UserDetailDto
			{
				Id = user.Id,
				UserName = user.UserName,
				Email = user.Email,
				Roles = roles.ToList()
			};

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
