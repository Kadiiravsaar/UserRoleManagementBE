using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using URM.Core.DTOs;
using URM.Core.Services;
using URM.Core.Ultities.Results;
using URM.Service.Constants;
using URM.Service.Services;

namespace URM.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RoleController : ControllerBase
	{
		private readonly IRoleService _roleService;

		public RoleController(IRoleService roleService)
		{
			_roleService = roleService;
		}

		[HttpPost("create")]
		[Authorize]
		public async Task<IActionResult> CreateRole(RoleDto roleDto)
		{
			var result = await _roleService.CreateRoleAsync(roleDto);
			return Ok(result); 
		}

		[HttpPut("update")]
		
		public async Task<IActionResult> UpdateRole(string roleName, RoleDto roleDto)
		{
			var result = await _roleService.UpdateRoleAsync(roleName, roleDto);
			return Ok(result); 
		}

		[HttpDelete("delete/{roleName}")]
		[Authorize]
		public async Task<IActionResult> DeleteRole(string roleName)
		{
			var result = await _roleService.DeleteRoleAsync(roleName);
			if (result.Success)
			{
				return Ok(result); 
			}
			return BadRequest(result);
		}

		[HttpGet("list")]
		public async Task<IActionResult> GetAllRoles()
		{
			var roles = await _roleService.GetAllRolesAsync();
			return Ok(roles);
		}

		[HttpPost("remove")]
		[Authorize]
		public async Task<IActionResult> RemoveRoleFromUser(string userId, string roleName)
		{
			var result = await _roleService.RemoveRoleFromUserAsync(userId, roleName);
			return Ok(result); 
		}


		[HttpPost("assign")]
		
		public async Task<IActionResult> AssignRoleToUser(string userId, string roleName)
		{
			var result = await _roleService.AssignRoleToUserAsync(userId, roleName);
			return Ok(result); 
		}

	}
}
