using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/SystemRole")]
    public partial class SystemRoleController : ControllerBase
    {
        private readonly ISysRoleService _service;

        public SystemRoleController(ISysRoleService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{sysID}/Ids")]
        public async Task<IActionResult> QuerySystemRoleByIds([FromRoute] string sysID, [FromQuery] string roleCategoryID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemRoleByIdList(sysID, roleCategoryID, cultureID);
            return Ok(result);
        }

        [HttpGet]
        [Route("{sysID}")]
        public async Task<IActionResult> QuerySystemRole([FromRoute] string sysID, [FromQuery] string roleID, [FromQuery] string roleCategoryID, [FromQuery] string cultureID, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var result = await _service.GetSystemRoleList(sysID, roleID, roleCategoryID, cultureID, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = result.rowCount,
                SystemRoles = result.SystemRoleList
            });
        }

        [HttpGet]
        [Route("{sysID}/{roleID}")]
        public async Task<IActionResult> QuerySystemRole([FromRoute] string sysID, [FromRoute] string roleID)
        {
            var result = await _service.GetSystemRole(sysID, roleID);
            return Ok(result);
        }

        [HttpPost]
        [Route("Category")]
        public async Task<IActionResult> EditSystemRoleByCategory([FromBody] SystemRoleMain systemRole)
        {
            await _service.EditSystemRoleByCategory(systemRole);
            return Ok();
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> EditSystemRole([FromBody] SystemRoleMain systemRole)
        {
            await _service.EditSystemRoleDetail(systemRole);
            return Ok();
        }

        [HttpDelete]
        [Route("{sysID}/{roleID}")]
        public async Task<IActionResult> DeleteSystemRole([FromRoute] string sysID, [FromRoute] string roleID)
        {
            await _service.DeleteSystemRoleDetail(sysID, roleID);
            return Ok();
        }
    }
}