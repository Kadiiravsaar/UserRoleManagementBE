using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URM.Core.DTOs;
using URM.Core.Ultities.Results;

namespace URM.Core.Services
{
	public interface IRoleService
	{
		Task<IResult> CreateRoleAsync(RoleDto roleDto);
		Task<IResult> UpdateRoleAsync(string roleName, RoleDto roleDto);
		Task<IResult> DeleteRoleAsync(string roleName);
		Task<IDataResult<List<RoleDto>>> GetAllRolesAsync();
		Task<IResult> AssignRoleToUserAsync(string userId, string roleName);
		Task<IResult> RemoveRoleFromUserAsync(string userId, string roleName);
	}
}
