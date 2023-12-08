using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/SystemWorkFlowChart")]
    public class SystemWorkFlowChartController : Controller
    {
        private readonly ISysWorkFlowChartService _service;
        public SystemWorkFlowChartController(ISysWorkFlowChartService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("NodePositions/{sysID}/{wfFlowID}/{wfFlowVer}")]
        public async Task<ActionResult> QuerySystemWorkFlowNodePositions([FromRoute] string sysID, [FromRoute] string wfFlowID, [FromRoute] string wfFlowVer)
        {
            var result = await _service.GetSystemWorkFlowNodePositions(sysID, wfFlowID, wfFlowVer);
            return Ok(result);
        }

        [HttpGet]
        [Route("ArrowPositions/{sysID}/{wfFlowID}/{wfFlowVer}")]
        public async Task<ActionResult> QuerySystemWorkFlowArrowPositions([FromRoute] string sysID, [FromRoute] string wfFlowID, [FromRoute] string wfFlowVer)
        {
            var result = await _service.GetSystemWorkFlowArrowPositions(sysID, wfFlowID, wfFlowVer);
            return Ok(result);
        }
    }
}