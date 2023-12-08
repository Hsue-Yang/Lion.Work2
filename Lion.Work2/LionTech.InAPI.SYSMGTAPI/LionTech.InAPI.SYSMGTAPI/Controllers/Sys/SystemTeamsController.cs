using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/SystemTeams")]
    public class SystemTeamsController : ControllerBase
    {
        private readonly ISysTeamsService _service;

        public SystemTeamsController(ISysTeamsService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{sysID}")]
        public async Task<IActionResult> QuerySystemTeamsList([FromRoute] string sysID, [FromQuery] string cultureID, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var result = await _service.GetSystemTeamsList(sysID, cultureID, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = result.rowCount,
                SystemTeamss = result.SystemTeamsList
            });
        }

        [HttpGet]
        [Route("{sysID}/{teamsChannelID}")]
        public async Task<IActionResult> QuerySystemTeams([FromRoute] string sysID, [FromRoute] string teamsChannelID)
        {
            var result = await _service.GetSystemTeams(sysID, teamsChannelID);
            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> EditSystemTeams([FromBody] SysTeams systemTeams)
        {
            await _service.EditSystemTeamsDetail(systemTeams);
            return Ok();
        }

        [HttpDelete]
        [Route("{sysID}/{teamsChannelID}")]
        public async Task<IActionResult> DeleteSystemTeams([FromRoute] string sysID, [FromRoute] string teamsChannelID)
        {
            await _service.DeleteSystemTeamsDetail(sysID, teamsChannelID);
            return Ok();
        }
    }
}