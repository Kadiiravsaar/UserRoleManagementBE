using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using URM.Core.DTOs;
using URM.Core.Services;
using URM.Service.Constants;
using URM.Service.Services;

namespace URM.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RolesController : ControllerBase
	{
		private readonly IRoleService _roleService;

		public RolesController(IRoleService roleService)
		{
			_roleService = roleService;
		}

		[HttpPost("Create")]
		[Authorize(Roles = "Admin,Editor")]
		public async Task<IActionResult> CreateRole(RoleDto roleDto)
		{	
			var result = await _roleService.CreateRoleAsync(roleDto);
			return Ok(result); 
		}

		[HttpPut("Update")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> UpdateRole(string roleName, RoleDto roleDto)
		{
			var result = await _roleService.UpdateRoleAsync(roleName, roleDto);
			return Ok(result); 
		}

		[HttpDelete("Delete")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteRole(string roleName)
		{
			var result = await _roleService.DeleteRoleAsync(roleName);
			if (result.Success)
			{
				return Ok(result); 
			}
			return BadRequest(result);
		}

		[HttpGet("List")]
		public async Task<IActionResult> GetAllRoles()
		{
			var roles = await _roleService.GetAllRolesAsync();
			return Ok(roles);
		}

		[HttpPost("Remove")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> RemoveRoleFromUser(string userId, string roleName)
		{
			var result = await _roleService.RemoveRoleFromUserAsync(userId, roleName);
			return Ok(result); 
		}


		[HttpPost("Assign")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AssignRoleToUser(string userId, string roleName)
		{
			var result = await _roleService.AssignRoleToUserAsync(userId, roleName);
			return Ok(result); 
		}

	}
}
