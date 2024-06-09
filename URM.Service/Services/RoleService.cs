using Autofac.Extras.DynamicProxy;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using URM.Core.DTOs;
using URM.Core.Models;
using URM.Core.Services;
using URM.Core.Utilities.Results;
using URM.Service.Constants;
using URM.Service.Rules;


namespace URM.Service.Services
{
	public class RoleService : IRoleService
	{
		private readonly RoleManager<AppRole> _roleManager;
		private readonly UserManager<AppUser> _userManager;
		private readonly IMapper _mapper;
		private readonly RoleBusinessRules _roleBusinessRules;
		public RoleService(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager, IMapper mapper, RoleBusinessRules roleBusinessRules)
		{
			_roleManager = roleManager;
			_userManager = userManager;
			_mapper = mapper;
			_roleBusinessRules = roleBusinessRules;
		}

		//[ValidationAspect(typeof(RoleValidator))] 
		public async Task<IResult> CreateRoleAsync(RoleDto roleDto)
		{
			await _roleBusinessRules.AdminOrEditorRoleRequired();


			var role = _mapper.Map<AppRole>(roleDto);
			var result = await _roleManager.CreateAsync(role);
			if (!result.Succeeded)
			{
				return new ErrorResult("Role creation failed: " + result.Errors.FirstOrDefault()?.Description);
			}
			return new SuccessResult(Messages.RoleAdded);
		}

		public async Task<IResult> UpdateRoleAsync(string roleName, RoleDto roleDto)
		{
			await _roleBusinessRules.AdminRoleRequired();

			var role = await _roleManager.FindByNameAsync(roleName);
			if (role == null)
			{
				return new ErrorResult(Messages.RoleNotFound);
			}

			_mapper.Map(roleDto, role);
			role.NormalizedName = roleDto.Name.ToUpper();

			var result = await _roleManager.UpdateAsync(role);
			if (!result.Succeeded)
			{
				return new ErrorResult("Role update failed: " + result.Errors.FirstOrDefault()?.Description);
			}
			return new SuccessResult(Messages.RoleUpdated);
		}

		public async Task<IResult> DeleteRoleAsync(string roleName)
		{
			await _roleBusinessRules.AdminRoleRequired();

			var role = await _roleManager.FindByNameAsync(roleName);
			await _roleManager.DeleteAsync(role);
			return new SuccessResult(Messages.RoleDeleted);
		}

//		[AllowAnonymous] // interceptor bu kod ile bu metodda yoluna devam edecek
		public async Task<IDataResult<List<RoleDto>>> GetAllRolesAsync()
		{
			var roles = await _roleManager.Roles.ToListAsync();
			var roleDtos = _mapper.Map<List<RoleDto>>(roles);
			return new SuccessDataResult<List<RoleDto>>(roleDtos);
		}

		public async Task<IResult> AssignRoleToUserAsync(string userId, string roleName)
		{
			await _roleBusinessRules.AdminRoleRequired();

			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return new ErrorResult(Messages.UserNotFound);
			}

			var normalizedRoleName = await RoleNameFindAsync(roleName);
			if (await _userManager.IsInRoleAsync(user, normalizedRoleName))
			{
				return new ErrorResult(Messages.RoleAlreadyAssigned);
			}

			var result = await _userManager.AddToRoleAsync(user, normalizedRoleName);
			if (!result.Succeeded)
			{
				return new ErrorResult("Role assignment unsuccessful: " + result.Errors.FirstOrDefault()?.Description);
			}

			return new SuccessResult(Messages.RoleAssignedToUser);
		}

		public async Task<IResult> RemoveRoleFromUserAsync(string userId, string requestRoleName)
		{
			await _roleBusinessRules.AdminRoleRequired();
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return new ErrorResult(Messages.UserNotFound);
			}

			var roleName = await RoleNameFindAsync(requestRoleName);

			var result = await _userManager.RemoveFromRoleAsync(user, roleName);
			if (!result.Succeeded)
			{
				return new ErrorResult(Messages.RoleNotRegistered);
			}

			return new SuccessResult(Messages.UserRoleWasSuccessfullyRemoved);
		}

		private async Task<string> RoleNameFindAsync(string roleName)
		{
			var role = await _roleManager.FindByNameAsync(roleName);
			return role?.Name ?? throw new InvalidOperationException("Role not found.");
		}
	}
}