using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/SystemFunElm")]

    public partial class SystemFunElmController : ControllerBase
    {
        private readonly ISysFunElmService _service;

        public SystemFunElmController(ISysFunElmService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{sysID}")]
        public async Task<IActionResult> QuerySysFunElmList([FromRoute] string sysID, [FromQuery] string isDisable,
            [FromQuery] string elmID, [FromQuery] string elmName, [FromQuery] string funControllerID,
            [FromQuery] string funActionName, [FromQuery] string cultureID, [FromQuery] int pageIndex,
            [FromQuery] int pageSize)
        {
            var (rowCount, systemFunElmList) = await _service.GetSysFunElmList(sysID, isDisable, elmID, elmName,
                funControllerID, funActionName, cultureID, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = rowCount,
                SystemFunElmList = systemFunElmList
            });
        }

        [HttpGet]
        [Route("{sysID}/{funControllerID}/{funActionName}/{elmID}/IsExists")]
        public async Task<IActionResult> CheckSystemFunElmIdIsExists([FromRoute] string sysID, [FromRoute] string elmID,
            [FromRoute] string funControllerID, [FromRoute] string funActionName)
        {
            bool isExists = await _service.CheckSystemFunElmIdIsExists(sysID, elmID, funControllerID, funActionName);
            return Ok(new
            {
                IsExists = isExists
            });
        }

        [HttpGet]
        [Route("{sysID}/{funControllerID}/{funActionName}/{elmID}")]
        public async Task<IActionResult> QuerySystemFunElmDetail([FromRoute] string sysID, [FromRoute] string elmID,
            [FromRoute] string funControllerID, [FromRoute] string funActionName)
        {
            var result = await _service.GetSystemFunElmDetail(sysID, elmID, funControllerID, funActionName);
            return Ok(result);
        }

        [HttpGet]
        [Route("{sysID}/{funControllerID}/{funActionName}/{elmID}/Roles")]
        public async Task<IActionResult> QuerySystemFunElmRoleList([FromRoute] string sysID, [FromRoute] string elmID,
            [FromRoute] string funControllerID, [FromRoute] string funActionName, [FromQuery] string cultureID)
        {
            var result =
                await _service.GetSystemFunElmRoleList(sysID, elmID, funControllerID, funActionName, cultureID);
            return Ok(result);
        }

        [HttpGet]
        [Route("{sysID}/{funControllerID}/{funActionName}/{elmID}/Info")]
        public async Task<IActionResult> QuerySystemFunElmInfo([FromRoute] string sysID, [FromRoute] string elmID,
            [FromRoute] string funControllerID, [FromRoute] string funActionName, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemFunElmInfo(sysID, elmID, funControllerID, funActionName, cultureID);
            return Ok(result);
        }

        [HttpPost]
        [Route("{sysID}/{funControllerID}/{funActionName}/{elmID}")]
        public async Task<IActionResult> EditSystemFunElmDetail([FromBody] SystemFunElm systemFunElm)
        {
            await _service.EditSystemFunElmDetail(systemFunElm);
            return Ok();
        }

        [HttpPost]
        [Route("{sysID}/{funControllerID}/{funActionName}/{elmID}/Roles")]
        public async Task<IActionResult> EditSystemFunElmRole([FromRoute] string sysID, [FromRoute] string elmID,
            [FromRoute] string funControllerID, [FromRoute] string funActionName,
            [FromBody] List<ElmRoleInfoValue> elmRoleInfoValues)
        {
            await _service.EditSystemFunElmRole(sysID, elmID, funControllerID, funActionName, elmRoleInfoValues);
            return Ok();
        }
    }
}