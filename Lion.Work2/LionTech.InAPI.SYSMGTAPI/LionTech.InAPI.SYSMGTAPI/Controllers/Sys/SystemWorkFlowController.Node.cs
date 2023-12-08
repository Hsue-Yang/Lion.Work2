using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    public partial class SystemWorkFlowController
    {
        [HttpGet]
        [Route("Node/{sysID}/{cultureID}/{wfFlowID}/{wfFlowVer}")]
        public async Task<IActionResult> QuerySystemWorkFlowNodes([FromRoute] string sysID, [FromRoute] string cultureID, [FromRoute] string wfFlowID, [FromRoute] string wfFlowVer)
        {
            var result = await _service.GetSystemWorkFlowNodes(sysID, cultureID, wfFlowID, wfFlowVer);
            return Ok(new
            {
                systemWorkFlowNodeList = result
            });
        }

        [HttpGet]
        [Route("Node/Info/{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}")]
        public async Task<IActionResult> QuerySystemWorkFlowNode([FromRoute] string sysID, [FromRoute] string wfFlowID, [FromRoute] string wfFlowVer, [FromRoute] string wfNodeID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemWorkFlowNode(sysID, wfFlowID, wfFlowVer, wfNodeID, cultureID);
            return Ok(new
            {
                systemWorkFlowNode = result
            });
        }
    }
}