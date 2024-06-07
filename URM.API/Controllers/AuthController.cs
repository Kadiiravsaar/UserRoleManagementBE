using Microsoft.AspNetCore.Mvc;
using URM.Core.DTOs;
using URM.Core.Services;

namespace URM.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}


		[HttpPost("Register")]
		public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
		{
			var user = await _authService.SignUp(registerDto);
			return Ok(user);
		}


		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
		{
			var user = await _authService.SignIn(loginDto);
			return Ok(user);
		}

	}
}
