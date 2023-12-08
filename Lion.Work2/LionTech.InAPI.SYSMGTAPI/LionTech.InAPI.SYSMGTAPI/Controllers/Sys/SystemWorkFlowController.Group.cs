using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    public partial class SystemWorkFlowController
    {
        [HttpGet]
        [Route("Group/{sysID}")]
        public async Task<IActionResult> QuerySystemWorkFlowGroups([FromRoute] string sysID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemWorkFlowGroups(sysID, cultureID);
            return Ok(new
            {
                systemWorkFlowGroupList = result
            });
        }

        [HttpGet]
        [Route("Group/Detail/{sysID}/{wfFlowGroupID}")]
        public async Task<IActionResult> QuerySystemWorkFlowGroupDetail([FromRoute] string sysID, [FromRoute] string wfFlowGroupID)
        {
            var result = await _service.GetSystemWorkFlowGroupDetail(sysID, wfFlowGroupID);
            return Ok(new
            {
                systemWorkFlowGroupDetails = result
            });
        }

        [HttpPost]
        [Route("Group/Detail/SystemWorkFlowGroupDetail")]
        public async Task<IActionResult> EditSystemWorkFlowGroupDetail([FromBody] SystemWorkFlowGroupDetail systemWorkFlowGroupDetail)
        {
            var result = await _service.EditSystemWorkFlowGroupDetail(systemWorkFlowGroupDetail);
            return Ok(new
            {
                EditStatus = result
            });
        }

        [HttpGet]
        [Route("Group/IsExists/{sysID}/{wfFlowGRoupID}")]
        public async Task<IActionResult> CheckSystemWorkFlowGroupExists([FromRoute] string sysID, [FromRoute] string wfFlowGroupID)
        {
            bool isExists = await _service.CheckSystemWorkFlowGroupExists(sysID, wfFlowGroupID);

            return Ok(new
            {
                IsExists = isExists
            });
        }

        [HttpGet]
        [Route("Group/Flow/IsExists/{sysID}/{wfFlowGRoupID}")]
        public async Task<IActionResult> CheckSystemWorkFlowExists([FromRoute] string sysID, [FromRoute] string wfFlowGroupID)
        {
            bool isExists = await _service.CheckSystemWorkFlowExists(sysID, wfFlowGroupID);

            return Ok(new
            {
                IsExists = isExists
            });
        }

        [HttpDelete]
        [Route("Group/Detail/{sysID}/{wfFlowGroupID}")]
        public async Task<IActionResult> DeleteSystemWorkFlowGroupDetail([FromRoute] string sysID, [FromRoute] string wfFlowGroupID)
        {
            bool result = await _service.DeleteSystemWorkFlowGroupDetail(sysID, wfFlowGroupID);

            return Ok(new
            {
                DeleteStatus = result
            });
        }

        [HttpGet]
        [Route("Group/IDs/{sysID}")]
        public async Task<IActionResult> QuerySystemWorkFlowGroupIDs([FromRoute] string sysID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemWorkFlowGroupIDs(sysID, cultureID);
            return Ok(new
            {
                SystemWorkFlowGroupIDs = result
            });
        }
    }
}
