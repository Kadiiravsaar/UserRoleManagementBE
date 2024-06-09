using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URM.Core.DTOs;
using URM.Core.Models;
using URM.Core.Utilities.Results;

namespace URM.Core.Services
{
	public interface IUserService
	{
		Task<IDataResult<List<UserDto>>> UserList();
		Task<IDataResult<UserDetailDto>> GetUserByIdAsync(string userId);
	}
}
