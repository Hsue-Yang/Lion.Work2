using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/SystemWorkFlowNext")]
    public class SystemWorkFlowNextController : ControllerBase
    {
        private readonly ISysWorkFlowNextService _service;
        public SystemWorkFlowNextController(ISysWorkFlowNextService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}")]
        public async Task<ActionResult> QuerySystemWorkFlowNext([FromRoute] string sysID, [FromRoute] string wfFlowID, [FromRoute] string wfFlowVer, [FromRoute] string wfNodeID, string cultureID)
        {
            var result = await _service.GetSystemWorkFlowNext(sysID, wfFlowID, wfFlowVer, wfNodeID, cultureID);
            return Ok(new
            {
                SystemWorkFlowNextList = result
            });
        }

        [HttpGet]
        [Route("SystemWFNodes/{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}")]
        public async Task<ActionResult> QuerySystemWorkFlowNextNodes([FromRoute] string sysID, [FromRoute] string wfFlowID, [FromRoute] string wfFlowVer, [FromRoute] string wfNodeID, string nodeTypeListstr, string cultureID)
        {
            var result = await _service.GetSystemWorkFlowNextNodes(sysID, wfFlowID, wfFlowVer, wfNodeID, nodeTypeListstr, cultureID);
            return Ok(new
            {
                SystemWFNodeList = result
            });
        }

        [HttpGet]
        [Route("WFNext/{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}")]
        public async Task<ActionResult> QuerySystemWorkFlowNextDetail([FromRoute] string sysID, [FromRoute] string wfFlowID, [FromRoute] string wfFlowVer, [FromRoute] string wfNodeID, string nextWFNodeID, string cultureID)
        {
            var result = await _service.GetSystemWorkFlowNextDetail(sysID, wfFlowID, wfFlowVer, wfNodeID, nextWFNodeID, cultureID);
            return Ok(new
            {
                SystemWFNext = result
            });
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> EditSystemWorkFlowNext([FromBody] EditSystemWFNext editSystemWFNext)
        {
            await _service.EditSystemWorkFlowNext(editSystemWFNext);
            return Ok();
        }

        [HttpDelete]
        [Route("{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}")]
        public async Task<ActionResult> DeleteSystemWorkFlowNext([FromRoute] string sysID, [FromRoute] string wfFlowID, [FromRoute] string wfFlowVer, [FromRoute] string wfNodeID, string nextWFNodeID)
        {
            await _service.DeleteSystemWorkFlowNext(sysID, wfFlowID, wfFlowVer, wfNodeID, nextWFNodeID);
            return Ok();
        }


    }
}
