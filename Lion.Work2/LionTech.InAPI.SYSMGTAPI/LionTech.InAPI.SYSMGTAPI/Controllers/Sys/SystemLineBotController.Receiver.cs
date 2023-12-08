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
    public partial class SystemLineReceiverController : ControllerBase
    {
        private readonly ISysLineReceiverService _service;
        public SystemLineReceiverController(ISysLineReceiverService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("Receiver/{sysID}/{lineID}/{receiverID}/IsExists")]
        public async Task<IActionResult> CheckSystemLineBotReceiverIdIsExists([FromRoute] string sysID, [FromRoute] string lineID, [FromQuery] string lineReceiverID, [FromRoute] string receiverID)
        {
            bool isExists = await _service.CheckSystemLineBotReceiverIdIsExists(sysID, lineID, lineReceiverID, receiverID);
            return Ok(new
            {
                IsExists = isExists
            });
        }

        [HttpGet]
        [Route("Receivers/{sysID}/{lineID}")]
        public async Task<IActionResult> QuerySystemLineBotReceiver([FromRoute] string sysID, [FromRoute] string lineID, [FromQuery] string cultureID, [FromQuery] string queryReceiverNM, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var (rowCount, systemLineReceiverList) = await _service.GetSystemLineBotReceiver(sysID, lineID, cultureID, queryReceiverNM, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = rowCount,
                SystemLineReceiverList = systemLineReceiverList
            });
        }

        [HttpGet]
        [Route("Receiver/{sysID}/{lineID}/{receiverID}")]
        public async Task<IActionResult> QuerySystemLineBotReceiverDetail([FromRoute] string sysID, [FromRoute] string receiverID, [FromRoute] string lineID, [FromQuery] string cultureID)
        {
            var result= await _service.GetSystemLineBotReceiverDetail(sysID, receiverID, lineID, cultureID);
            return Ok(result);
        }

        [HttpPost]
        [Route("Receiver/{sysID}/{lineID}/{receiverID}")]
        public async Task<IActionResult> EditSystemineBotReceiverDetail([FromRoute] string sysID, [FromBody] SystemLineReceiver systemLineReceiver)
        {
            await _service.EditSystemLineBotReceiverDetail(systemLineReceiver);
            return Ok();
        }
    }
}