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
	private readonly SignInManager<AppUser> _signInManager;

	public AuthBusinessRules(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
	{
		_userManager = userManager;
		_roleManager = roleManager;
		_signInManager = signInManager;
	}

	/// <summary>
	/// İş Kuralı: Şifre kontrolü
	/// </summary>
	/// <param name="user"></param>
	/// <param name="password"></param>
	/// <returns></returns>
	/// <exception cref="BusinessException"></exception>
	public async Task CheckPasswordAsync(AppUser user, string password)
	{
		var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
		if (!result.Succeeded)
		{
			throw new BusinessException(Messages.InvalidUsernameOrPassword); 
		}
	}

	/// <summary>
	///  İş Kuralı: Rol varlığını kontrol et ve gerekiyorsa oluştur
	/// </summary>
	/// <param name="roleName"></param>
	/// <returns></returns>
	/// <exception cref="InvalidOperationException"></exception>
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