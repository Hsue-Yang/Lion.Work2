using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/SystemWorkFlow")]
    public partial class SystemWorkFlowController : ControllerBase
    {
        private readonly ISysWorkFlowService _service;

        public SystemWorkFlowController(ISysWorkFlowService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{sysID}")]
        public async Task<IActionResult> QuerySystemWorkFLow([FromRoute] string sysID, [FromQuery] string wfFlowGroupID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemWorkFlows(sysID, wfFlowGroupID, cultureID);
            return Ok(new
            {
                systemWorkFlowList = result
            });
        }

        [HttpGet]
        [Route("Detail/{sysID}/{wfFlowID}/{wfFlowVer}")]
        public async Task<IActionResult> QuerySystemWorkFlowDetail([FromRoute] string sysID, [FromRoute] string wfFlowID, [FromRoute] string wfFlowVer)
        {
            var result = await _service.GetSystemWorkFlowDetail(sysID, wfFlowID, wfFlowVer);
            return Ok(result);
        }

        [HttpGet]
        [Route("IsExists/{sysID}/{wfFlowID}/{wfFlowVer}")]
        public async Task<IActionResult> CheckWorkFlowIsExists([FromRoute] string sysID, [FromRoute] string wfFlowID, [FromRoute] string wfFlowVer)
        {
            bool isExists = await _service.CheckWorkFlowIsExists(sysID, wfFlowID, wfFlowVer);
            return Ok(new
            {
                IsExists = isExists
            });
        }

        [HttpGet]
        [Route("Roles/{sysID}")]
        public async Task<IActionResult> QuerySystemWorkFlowRoles([FromRoute] string sysID, [FromQuery] string wfFlowID, [FromQuery] string wfFlowVer, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemWorkFlowRoles(sysID, wfFlowID, wfFlowVer, cultureID);

            return Ok(result);
        }

        [HttpPost]
        [Route("Detail/SystemWorkFlowDetail")]
        public async Task<IActionResult> EditSystemWorkFlowDetail([FromBody] SystemWorkFlowDetails systemWorkFlowDetails)
        {
            bool result = await _service.EditSystemWorkFlowDetail(systemWorkFlowDetails);

            return Ok(new
            {
                EditStatus = result
            });
        }

        [HttpDelete]
        [Route("Detail/{sysID}/{wfFlowID}/{wfFlowVer}")]
        public async Task<IActionResult> DeleteSystemWorkFlowDetail([FromRoute] string sysID, [FromRoute] string wfFlowID, [FromRoute] string wfFlowVer)
        {
            bool result = await _service.DeleteSystemWorkFlowDetail(sysID, wfFlowID, wfFlowVer);

            return Ok(new
            {
                DeleteStatus = result
            });
        }

        [HttpGet]
        [Route("Node/ByFlowID/{sysID}/{userID}/{cultureID}/{wfFlowGroupID}")]
        public async Task<IActionResult> QuerySysUserSystemWorkFlowID([FromRoute] string sysID, [FromRoute] string userID, [FromRoute] string cultureID, [FromRoute] string wfFlowGroupID)
        {
            var result = await _service.GetSysUserSystemWorkFlowIDs(sysID, userID, cultureID, wfFlowGroupID);
            return Ok(new
            {
                sysUserSystemWorkFlowIDList = result
            });
        }
    }
}
