using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/SystemFunToolSetting")]
    public class SystemFunToolSettingController : ControllerBase
    {
        private readonly ISysFunToolSettingService _sysFunToolSettingService;

        public SystemFunToolSettingController(ISysFunToolSettingService sysFunToolSettingService)
        {
            _sysFunToolSettingService = sysFunToolSettingService;
        }

        [HttpGet]
        [Route("{userID}/{sysID}/{funControllerID}/{funActionName}")]
        public async Task<IActionResult> QuerySystemFunToolSettings([FromRoute] string userID, [FromRoute] string sysID, [FromRoute] string funControllerID, [FromRoute] string funActionName, [FromQuery] string cultureID)
        {
            var result = await _sysFunToolSettingService.GetSystemFunToolSettings(userID, sysID, funControllerID, funActionName, cultureID);
            return Ok(result);
        }

        [HttpGet]
        [Route("ControllerIDs/{sysID}")]
        public async Task<IActionResult> QuerySystemFunToolControllerIDs([FromRoute] string sysID, [FromQuery] string condition, [FromQuery] string cultureID)
        {
            var result = await _sysFunToolSettingService.GetSystemFunToolControllerIDs(sysID, condition, cultureID);
            return Ok(result);
        }

        [HttpGet]
        [Route("FunNames/{sysID}/{funControllerID}")]
        public async Task<IActionResult> QuerySystemFunToolFunNames([FromRoute] string sysID, [FromRoute] string funControllerID, [FromQuery] string condition, [FromQuery] string cultureID)
        {
            var result = await _sysFunToolSettingService.GetSystemFunToolFunNames(sysID, funControllerID, condition, cultureID);
            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> EditSystemFunToolSetting([FromBody] SystemUserToolPara systemUserToolPara)
        {
            await _sysFunToolSettingService.EditSystemFunToolSetting(systemUserToolPara);
            return Ok();
        }

        [HttpPut]
        [Route("{userID}/{sysID}/{funControllerID}/{funActionName}")]
        public async Task<IActionResult> DeleteSystemFunToolSetting([FromRoute] string userID, [FromRoute] string sysID, [FromRoute] string funControllerID, [FromRoute] string funActionName, [FromBody] SystemUserToolPara systemUserToolPara)
        {
            await _sysFunToolSettingService.DeleteSystemFunToolSetting(userID, sysID, funControllerID, funActionName, systemUserToolPara);
            return Ok();
        }

        [HttpPost]
        [Route("CopyFunTool/{toolNO}")]
        public async Task<IActionResult> CopySystemUserFunTool([FromRoute] string toolNO, [FromBody] SystemUserToolPara systemUserToolPara)
        {
            await _sysFunToolSettingService.CopySystemUserFunTool(toolNO, systemUserToolPara);
            return Ok();
        }
    }
}