using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/SystemRoleCategory")]
    public partial class SystemRoleCategoryController : ControllerBase
    {
        private readonly ISysRoleCategoryService _service;

        public SystemRoleCategoryController(ISysRoleCategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{sysID}/Ids")]
        public async Task<IActionResult> QuerySystemRoleCategoryByIds([FromRoute] string sysID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemRoleCategoryByIdList(sysID, cultureID);
            return Ok(result);
        }

        [HttpGet]
        [Route("{sysID}")]
        public async Task<IActionResult> QuerySystemRoleCategories([FromRoute] string sysID, [FromQuery] string roleCategoryNM, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemRoleCategoryList(sysID, roleCategoryNM, cultureID);
            return Ok(result);
        }

        [HttpGet]
        [Route("{sysID}/{roleCategoryID}")]
        public async Task<IActionResult> QuerySystemRoleCategory([FromRoute] string sysID, [FromRoute] string roleCategoryID)
        {
            var result = await _service.GetSystemRoleCategoryDetail(sysID, roleCategoryID);
            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> EditSystemRoleCategory([FromBody] SystemRoleCategoryMain systemRoleCategory)
        {
            await _service.EditSystemRoleCategoryDetail(systemRoleCategory);
            return Ok();
        }

        [HttpDelete]
        [Route("{sysID}/{roleCategoryID}")]
        public async Task<IActionResult> DeleteSystemRoleCategory([FromRoute] string sysID, [FromRoute] string roleCategoryID)
        {
            var result = await _service.DeleteSystemRoleCategoryDetail(sysID, roleCategoryID);
            if (result == EnumDeleteSystemRoleCategoryDetailResult.Success)
            {
                return Ok();
            }
            return BadRequest(new { Message = result.ToString() });
        }
    }
}