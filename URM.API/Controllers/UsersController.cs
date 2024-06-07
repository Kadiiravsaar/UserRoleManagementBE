using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using URM.Core.Services;

namespace URM.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUserService _userService;

		public UsersController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet]
		public async Task<IActionResult> GetList()
		{
			var userList = await _userService.UserList();
			return Ok(userList);
		}

		[HttpGet("Id")]
		public async Task<IActionResult> GetById(string userId)
		{
			var user = await _userService.GetUserByIdAsync(userId);
			return Ok(user);
		}
	}
}
