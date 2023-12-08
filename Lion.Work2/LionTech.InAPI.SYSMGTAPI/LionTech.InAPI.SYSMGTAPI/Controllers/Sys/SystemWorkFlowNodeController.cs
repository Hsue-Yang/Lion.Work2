using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/SystemWorkFlowNode")]
    public class SystemWorkFlowNodeController : ControllerBase
    {
        private readonly ISysWorkFlowNodeService _service;

        public SystemWorkFlowNodeController(ISysWorkFlowNodeService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("SystemWorkFlowName/{sysID}/{wfFlowID}/{wfFlowVer}")]
        public async Task<IActionResult> QuerySystemWorkFlowName([FromRoute] string sysID, [FromRoute] string wfFlowID, [FromRoute] string wfFlowVer, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemWorkFlowName(sysID, wfFlowID, wfFlowVer, cultureID);
            return Ok(result);
        }

        [HttpGet]
        [Route("BackSystemWorkFlowNodes/{sysID}/{wfFlowID}/{wfFlowVer}")]
        public async Task<IActionResult> QueryBackSystemWorkFlowNodes([FromRoute] string sysID, [FromRoute] string wfFlowID, [FromRoute] string wfFlowVer, [FromQuery] string wfNodeID, [FromQuery] string cultureID)
        {
            var result = await _service.GetBackSystemWorkFlowNodes(sysID, wfFlowID, wfFlowVer, wfNodeID, cultureID);
            return Ok(result);
        }
        [HttpGet]
        [Route("Roles/{sysID}/{wfFlowID}/{wfFlowVer}")]
        public async Task<IActionResult> QuerySystemWorkFlowNodeRoles([FromRoute] string sysID, [FromRoute] string wfFlowID, [FromRoute] string wfFlowVer, [FromQuery] string wfNodeID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemWorkFlowNodeRoles(sysID, wfFlowID, wfFlowVer, wfNodeID, cultureID);
            return Ok(result);
        }
        [HttpGet]
        [Route("{sysID}/{wfFlowID}/{wfFlowVer}")]
        public async Task<IActionResult> QuerySystemWorkFlowNode([FromRoute] string sysID, [FromRoute] string wfFlowID, [FromRoute] string wfFlowVer, [FromQuery] string wfNodeID)
        {
            var result = await _service.GetSystemWorkFlowNode(sysID, wfFlowID, wfFlowVer, wfNodeID);
            return Ok(result);
        }
        [HttpGet]
        [Route("List/{sysID}/{wfFlowID}/{wfFlowVer}")]
        public async Task<IActionResult> QuerySystemWorkFlowNodes([FromRoute] string sysID, [FromRoute] string wfFlowID, [FromRoute] string wfFlowVer)
        {
            var result = await _service.GetSystemWorkFlowNodes(sysID, wfFlowID, wfFlowVer);
            return Ok(result);
        }
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> EditSystemWorkFlowNode([FromBody] SystemWorkFlowNodePara para)
        {
            var result = await _service.EditSystemWorkFlowNode(para);
            return Ok(result);
        }
        [HttpGet]
        [Route("{sysID}/{wfFlowID}/{wfFlowVer}/ChildsIsExists")]
        public async Task<IActionResult> CheckWorkFlowChildsIsExist([FromRoute] string sysID, [FromRoute] string wfFlowID, [FromRoute] string wfFlowVer, [FromQuery] string wfNodeID)
        {
            var result = await _service.IsWorkFlowChildsExist(sysID, wfFlowID, wfFlowVer, wfNodeID);
            return Ok(new
            {
                IsExist = result
            });
        }
        [HttpDelete]
        [Route("{sysID}/{wfFlowID}/{wfFlowVer}")]
        public async Task<IActionResult> DeleteSystemWorkFlowNode([FromRoute] string sysID, [FromRoute] string wfFlowID, [FromRoute] string wfFlowVer, [FromQuery] string wfNodeID)
        {
            await _service.DeleteSystemWorkFlowNodeDetail(sysID, wfFlowID, wfFlowVer, wfNodeID);
            return Ok();
        }
        [HttpGet]
        [Route("CheckRunTime/{sysID}/{wfFlowID}/{wfFlowVer}")]
        public async Task<IActionResult> IsWorkFlowHasRunTime([FromRoute] string sysID, [FromRoute] string wfFlowID, [FromRoute] string wfFlowVer)
        {
            var result = await _service.IsWorkFlowHasRunTime(sysID, wfFlowID, wfFlowVer);
            return Ok(new
            {
                IsRunTime = result
            });
        }
        [HttpGet]
        [Route("IDs/{sysID}/{wfFlowID}/{wfFlowVer}")]
        public async Task<IActionResult> QuerySystemWorkFlowNodeIDs([FromRoute] string sysID, [FromRoute] string wfFlowID, [FromRoute] string wfFlowVer, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemWorkFlowNodeIDs(sysID, wfFlowID, wfFlowVer, cultureID);
            return Ok(new
            {
                SystemWorkFlowNodeIDs = result
            });
        }
    }
}
