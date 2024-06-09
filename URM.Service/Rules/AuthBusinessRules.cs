using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using URM.Core.DTOs;
using URM.Core.Exceptions;
using URM.Core.Models;
using URM.Service.Constants;
using URM.Service.Rules;

public class AuthBusinessRules
{
	private readonly UserManager<AppUser> _userManager;
	private readonly RoleManager<AppRole> _roleManager;

	public AuthBusinessRules(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
	{
		_userManager = userManager;
		_roleManager = roleManager;
	}

	public async Task EnsureRoleExists(string roleName)
	{
		var roleExists = await _roleManager.RoleExistsAsync(roleName);
		if (!roleExists)
		{
			var role = new AppRole { Name = roleName, NormalizedName = roleName.ToUpper() };
			var result = await _roleManager.CreateAsync(role);
			if (!result.Succeeded)
			{
				var errors = string.Join("; ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
				throw new InvalidOperationException("Rol oluşturulamadı: " + errors);
			}
		}
	}
}