using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/SystemRoleCondition")]
    public class SystemRoleConditionController : ControllerBase
    {
        private readonly ISysRoleConditionService _service;
        public SystemRoleConditionController(ISysRoleConditionService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> QuerySystemRoleConditions([FromQuery] string roleConditionID, [FromQuery] string roleID, [FromQuery] string sysID,[FromQuery] string cultureID, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var (rowCount, systemRoleConditions) = await _service.GetSystemRoleConditions(roleConditionID, roleID, sysID, cultureID, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = rowCount,
                SystemRoleConditionList = systemRoleConditions
            });
        }

        [HttpGet]
        [Route("Detail/{sysID}")]
        public async Task<IActionResult> QuerySystemRoleConditionDetail([FromRoute]string sysID, [FromQuery] string roleConditionID)
        {
            var result = await _service.GetSystemRoleConditionDetail(sysID, roleConditionID);
            return Ok(new
            {
                SystemRoleConditionDetail = result
            });
        }

        [HttpPut]
        [Route("Detail")]
        public async Task<IActionResult> EditSystemRoleConditionDetail([FromBody] SysRoleConditionDetailPara sysRoleConditionDetailPara)
        {
            await _service.EditSystemRoleConditionDetail(sysRoleConditionDetailPara);
            return Ok();
        }

        [HttpDelete]
        [Route("Detail/{sysID}/{roleConditionID}")]
        public async Task<IActionResult> DeleteSystemRoleConditionDetail([FromRoute] string sysID, [FromRoute] string roleConditionID)
        {
            await _service.DeleteSystemRoleConditionDetail(sysID, roleConditionID);
            return Ok();
        }
    }
}
