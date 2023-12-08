using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/SystemLineBot")]
    public partial class SystemLineController : ControllerBase
    {
        private readonly ISysLineService _service;

        public SystemLineController(ISysLineService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{sysID}/{lineID}/IsExists")]
        public async Task<IActionResult> CheckSystemLineBotIdIsExists([FromRoute] string sysID, [FromRoute] string lineID)
        {
            bool isExists = await _service.CheckSystemLineBotIdIsExists(sysID, lineID);
            return Ok(new
            {
                IsExists = isExists
            });
        }

        [HttpGet]
        [Route("{sysID}/LineIds")]
        public async Task<IActionResult> QuerySystemLineBotIdList([FromRoute] string sysID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemLineBotIdList(sysID, cultureID);
            return Ok(new 
            { 
                LineBotIDList = result 
            });
        }

        [HttpGet]
        [Route("{sysID}")]
        public async Task<IActionResult> QuerySystemLineBotAccountList([FromRoute] string sysID, [FromQuery] string lineID, [FromQuery] string cultureID, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var (rowCount, systemLineAccountList) = await _service.GetSystemLineBotAccountList(sysID, lineID, cultureID, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = rowCount,
                SystemLineAccountList = systemLineAccountList
            });
        }

        [HttpGet]
        [Route("{sysID}/{lineID}")]
        public async Task<IActionResult> QuerySystemLineBotAccountDetail([FromRoute] string sysID, [FromRoute] string lineID)
        {
            var result = await _service.GetSystemLineBotAccountDetail(sysID, lineID);
            return Ok(result);
        }

        [HttpPost]
        [Route("{sysID}/{lineID}")]
        public async Task<IActionResult> EditSystemLineBotAccountDetail([FromRoute] string sysID, [FromRoute] string lineID, [FromBody] SystemLine systemLine)
        {
            await _service.EditSystemLineBotAccountDetail(systemLine);
            return Ok();
        }

        [HttpDelete]
        [Route("{sysID}/{lineID}")]
        public async Task<IActionResult> DeleteSystemLineBotAccountByIds([FromRoute] string sysID, [FromRoute] string lineID)
        {
            await _service.DeleteSystemLineById(sysID, lineID);
            return Ok();
        }
    }
}