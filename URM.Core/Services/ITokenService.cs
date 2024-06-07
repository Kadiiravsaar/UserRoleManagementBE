using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URM.Core.Models;

namespace URM.Core.Services
{
	public interface ITokenService
	{
		Task<string> CreateToken(AppUser appUser);
	}
}
