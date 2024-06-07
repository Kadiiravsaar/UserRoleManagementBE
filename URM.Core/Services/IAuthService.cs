using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URM.Core.DTOs;

namespace URM.Core.Services
{
	public interface IAuthService
	{
		Task<NewUserDto> SignUp(RegisterDto registerDto);
		Task<NewUserDto> SignIn(LoginDto loginDto);
	}
}
