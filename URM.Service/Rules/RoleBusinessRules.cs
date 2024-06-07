using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URM.Core.Models;
using URM.Service.Constants;

namespace URM.Service.Rules
{
	public class BusinessException : Exception
	{
		public BusinessException(string message) : base(message)
		{
		}

		public BusinessException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}


	public class RoleBusinessRules
	{
		private readonly RoleManager<AppRole> _roleManager;
		private readonly UserManager<AppUser> _userManager; // Kullanıcı yönetimi için UserManager ekleniyor.
		private readonly IHttpContextAccessor _httpContextAccessor; // HTTP context'e erişim sağlamak için.

		public RoleBusinessRules(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
		{
			_roleManager = roleManager;
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task RoleNameShouldBeUnique(string roleName)
		{
			var roleExists = await _roleManager.RoleExistsAsync(roleName);
			if (roleExists)
				throw new BusinessException(Messages.RoleAlreadyExists);

		}

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
			if (appUser == null || !(await _userManager.IsInRoleAsync(appUser, "Admin")))
				throw new BusinessException(Messages.UnauthorizedAccessOnlyAdmins);

		}


		public async Task AdminRoleRequired()
		{
			var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
			if (user == null || !await _userManager.IsInRoleAsync(user, "Admin"))
				throw new BusinessException(Messages.OnlyAdminsCanPerformThisAction);
		}

		public async Task AdminOrEditorRoleRequired()
		{
			var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
			if (user == null || !(await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "Editör")))
				throw new BusinessException(Messages.OnlyAdminsOrEditorsCanPerformThisAction);

		}




	}

}
