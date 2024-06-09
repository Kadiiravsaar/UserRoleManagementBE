using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using URM.Core.Exceptions;
using URM.Core.Models;
using URM.Service.Constants;

namespace URM.Service.Rules
{
	public class UserBusinessRules : Exception
	{
		private readonly UserManager<AppUser> _userManager; 

		public UserBusinessRules(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
		}

		public async Task CheckIfUserExists(AppUser user)
		{
			var userId = await _userManager.FindByIdAsync(user.Id);
			if (userId == null)
			{
				throw new BusinessException(Messages.UserNotFound);
			}
		}
	}

}
