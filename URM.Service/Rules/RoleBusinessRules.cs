using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URM.Core.Enums;
using URM.Core.Exceptions;
using URM.Core.Models;
using URM.Service.Constants;

namespace URM.Service.Rules
{

	public class RoleBusinessRules
	{
		private readonly RoleManager<AppRole> _roleManager;
		private readonly UserManager<AppUser> _userManager; 
		private readonly IHttpContextAccessor _httpContextAccessor; 

		public RoleBusinessRules(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
		{
			_roleManager = roleManager;
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
		}


		/// <summary>
		///  İş Kuralı: Rolün benzersiz olup olmadığını kontrol et
		/// </summary>
		/// <param name="roleName"></param>
		/// <returns></returns>
		/// <exception cref="BusinessException"></exception>
		public async Task RoleNameShouldBeUnique(string roleName)
		{
			var roleExists = await _roleManager.RoleExistsAsync(roleName);
			if (roleExists)
				throw new BusinessException(Messages.RoleAlreadyExists);

		}

		/// <summary>
		///  İş Kuralı: Rolün var olup olmadığını kontrol et
		/// </summary>
		/// <param name="roleName"></param>
		/// <returns></returns>
		/// <exception cref="BusinessException"></exception>
		public async Task RoleShouldExistWhenUpdatedOrDeleted(string roleName)
		{
			var role = await _roleManager.FindByNameAsync(roleName);
			if (role == null)
				throw new BusinessException(Messages.RoleNotFound);
		}

		public async Task OnlyAdminCanDeleteRole()
		{
			var user = _httpContextAccessor.HttpContext.User;
			var appUser = await _userManager.GetUserAsync(user);
			if (appUser == null || !await _userManager.IsInRoleAsync(appUser, Roles.Admin.ToString()));
				throw new BusinessException(Messages.UnauthorizedAccessOnlyAdmins);
		}


		public async Task AdminRoleRequired()
		{
			var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
			if (user == null || !await _userManager.IsInRoleAsync(user, Roles.Admin.ToString()))
				throw new BusinessException(Messages.OnlyAdminsCanPerformThisAction);
		}

		public async Task AdminOrEditorRoleRequired()
		{
			var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
				if (user == null || !(await _userManager.IsInRoleAsync(user, Roles.Admin.ToString()) || await _userManager.IsInRoleAsync(user, Roles.Editor.ToString())))
					throw new BusinessException(Messages.OnlyAdminsOrEditorsCanPerformThisAction);
		}

	}

}
