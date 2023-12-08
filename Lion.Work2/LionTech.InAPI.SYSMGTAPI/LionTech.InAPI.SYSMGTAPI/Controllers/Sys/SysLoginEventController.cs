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
    [Route("v1/SysLoginEvent")]
    public class SysLoginEventController : ControllerBase
    {
        private readonly ISysLoginEventService _service;

        public SysLoginEventController(ISysLoginEventService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{sysID}/Ids")]
        public async Task<IActionResult> QuerySysLoginEventById([FromRoute] string sysID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSysLoginEventListById(sysID, cultureID);
            return Ok(new {
                SysLoginEventIDList = result
            });
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> QuerySysLoginEventSettings([FromQuery] string sysID, [FromQuery] string cultureID, [FromQuery] string logineventID, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var (rowCount, sysLoginEventSettingList) = await _service.GetSysLoginEventSettingList(sysID, cultureID, logineventID, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = rowCount,
                SysLoginEventSettingList = sysLoginEventSettingList
            });
        }

        [HttpGet]
        [Route("{sysID}/{logineventID}/IsExists")]
        public async Task<IActionResult> CheckSystemLineBotIdIsExists([FromRoute] string sysID, [FromRoute] string logineventID)
        {
            bool isExists = await _service.CheckSysLoginEventIdIsExists(sysID, logineventID);
            return Ok(new
            {
                IsExists = isExists
            });
        }

        [HttpGet]
        [Route("{sysID}")]
        public async Task<IActionResult> QueryLoginEventSettingDetail([FromRoute] string sysID, [FromQuery] string logineventID)
        {
            var result = await _service.GetLoginEventSettingDetail(sysID, logineventID);
            return Ok(new
            {
                LoginEventSettingDetail = result
            });
        }

        [HttpPost]
        [Route("SettingSort")]
        public async Task<IActionResult> EditSysLoginEventSettingSort([FromBody] List<LoginEventSettingValue> loginEventSettingValue)
        {
            await _service.EditSysLoginEventSettingSort(loginEventSettingValue);
            return Ok();
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> EditLoginEventSettingDetail([FromBody] LoginEventSettingDetail loginEventSettingDetail)
        {
            await _service.EditLoginEventSettingDetail(loginEventSettingDetail);
            return Ok();
        }

        [HttpDelete]
        [Route("{sysID}/{logineventID}")]
        public async Task<IActionResult> DeleteLoginEventSettingDetail([FromRoute] string sysID, [FromRoute] string logineventID)
        {
            await _service.DeleteLoginEventSettingDetail(sysID, logineventID);
            return Ok();
        }
    }
}