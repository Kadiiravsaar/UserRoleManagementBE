using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using URM.Core.DTOs;
using URM.Core.Models;
using URM.Core.Services;

namespace URM.Service.Services
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly RoleManager<AppRole> _roleManager;
		private readonly IMapper _mapper;
		private readonly ITokenService _tokenService;

		public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IMapper mapper, ITokenService tokenService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_mapper = mapper;
			_tokenService = tokenService;
		}

		public async Task<NewUserDto> SignIn(LoginDto loginDto)
		{
			if (loginDto == null)
			{
				throw new ArgumentNullException(nameof(loginDto));
			}

			var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());
			if (user == null) return null;

			var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
			if (result.Succeeded)
			{

				var userInfo = new NewUserDto
				{
					UserName = user.UserName,
					Email = user.Email,
					Token = await _tokenService.CreateToken(user)
				};
				return userInfo;
			}
			else
			{
				throw new Exception("Geçersiz kullanıcı adı veya şifre."); // Giriş başarısız olduğunda istenilen işlemin yapılması gerekir.
			}

		}



		public async Task<NewUserDto> SignUp(RegisterDto registerDto)
		{
			if (registerDto == null)
			{
				throw new ArgumentNullException(nameof(registerDto));
			}
			var user = new AppUser()

			// burada da hata yönetimi yap
			{
				UserName = registerDto.UserName,
				Email = registerDto.Email
			};
			var result = await _userManager.CreateAsync(user, registerDto.Password);

			if (result.Succeeded)
			{
				// Rol kontrolü ve atama işlemi
				var roleExists = await _roleManager.RoleExistsAsync("User");
				if (!roleExists)
				{
					await _roleManager.CreateAsync(new AppRole { Name = "User", NormalizedName = "USER" });
				}
				await _userManager.AddToRoleAsync(user, "User");

				// Token oluşturma
				var token = await _tokenService.CreateToken(user);

				return new NewUserDto
				{
					UserName = user.UserName,
					Email = user.Email,
					Token = token
				};
			}
			else
			{
				var errors = string.Join("; ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
				throw new InvalidOperationException("Kullanıcı oluşturulamadı: " + errors);
			}
		}
	}
}
