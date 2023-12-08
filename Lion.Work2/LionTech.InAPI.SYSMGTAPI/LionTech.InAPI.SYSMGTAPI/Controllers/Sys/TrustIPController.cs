using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/TrustIP")]
    public class TrustIPController : ControllerBase
    {
        private readonly ITrustIPService _service;

        public TrustIPController(ITrustIPService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> QueryTrustIPs(TrustIPPara para)
        {
            var result = await _service.GetTrustIPs(para);
            return Ok(new
            {
                RowCount = result.rowCount,
                TrustIPList = result.TrustIPList
            }) ;
        }

        [HttpGet]
        [Route("Detail/{IPBegin}/{IPEnd}")]
        public async Task<IActionResult> QueryTrustIPDetail([FromRoute] string IPBegin, [FromRoute] string IPEnd, string cultureID)
        {
            var result = await _service.GetTrustIPDetail(IPBegin, IPEnd, cultureID);
            return Ok(new
            {
                TrustIPDetail = result
            });
        }

        [HttpPost]
        [Route("Valid")]
        public async Task<IActionResult> QueryValidTrustIPRepeated(TrustIP trustIP)
        {
            var result = await _service.GetValidTrustIPRepeated(trustIP);
            return Ok(result);
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> EditTrustIP(TrustIP trustIP)
        {
            var result = await _service.EditTrustIP(trustIP);
            return Ok(result);
        }

        [HttpDelete]
        [Route("{IPBegin}/{IPEnd}")]
        public async Task<IActionResult> DeleteTrustIP([FromRoute] string IPBegin, [FromRoute] string IPEnd)
        {
            var result = await _service.DeleteTrustIP(IPBegin, IPEnd);
            return Ok(result);
        }

        [HttpPost]
        [Route("MongoDB")]
        public async Task<IActionResult> InsertTrustIPMongoDB(RecordSysTrustIP para)
        {
            await _service.InsertTrustIPMongoDB(para);
            return Ok();
        }
    }
}