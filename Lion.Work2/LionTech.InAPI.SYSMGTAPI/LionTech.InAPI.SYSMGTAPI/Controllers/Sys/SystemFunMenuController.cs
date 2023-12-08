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
    [Route("v1/SystemFunMenu")]
    public partial class SystemFunMenuController : ControllerBase
    {
        private readonly ISysFunMenuService _service;

        public SystemFunMenuController(ISysFunMenuService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{sysID}/Ids")]
        public async Task<IActionResult> QuerySystemFunMenuByIds([FromRoute] string sysID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemFunMenuByIdList(sysID, cultureID);
            return Ok(result);
        }

        [HttpGet]
        [Route("{sysID}")]
        public async Task<IActionResult> QuerySystemFunMenus([FromRoute] string sysID, [FromQuery] string cultureID, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var result = await _service.GetSystemFunMenuList(sysID, cultureID, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = result.rowCount,
                SystemFunMenus = result.SystemFunMenuList
            });
        }

        [HttpGet]
        [Route("{sysID}/{funMenu}")]
        public async Task<IActionResult> QuerySystemFunMenu([FromRoute] string sysID, [FromRoute] string funMenu)
        {
            var result = await _service.GetSystemFunMenuDetail(sysID, funMenu);
            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> EditSystemFunMenu([FromBody] SystemFunMenuMain systemFunMenu)
        {
            await _service.EditSystemFunMenuDetail(systemFunMenu);
            return Ok();
        }

        [HttpDelete]
        [Route("{sysID}/{funMenu}")]
        public async Task<IActionResult> DeleteSystemFunMenu([FromRoute] string sysID, [FromRoute] string funMenu)
        {
            var result = await _service.DeleteSystemFunMenuDetail(sysID, funMenu);
            if (result == EnumDeleteSystemFunMenuResult.Success)
            {
                return Ok();
            }
            return BadRequest(new { Message = result.ToString() });
        }
    }
}