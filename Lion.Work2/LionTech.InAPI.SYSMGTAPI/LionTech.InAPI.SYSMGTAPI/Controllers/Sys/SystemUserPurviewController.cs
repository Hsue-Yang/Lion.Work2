using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/UserPurview")]
    public class SystemUserPurviewController : ControllerBase
    {
        private readonly ISysUserPurviewService _service;
        public SystemUserPurviewController(ISysUserPurviewService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> QuerySysUserPurviews([FromQuery] string userID, [FromQuery] string updUserID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSysUserPurviews(userID, updUserID, cultureID);
            return Ok(new
            {
                SysUserPurviewList = result
            });
        }

        [HttpGet]
        [Route("Detail")]
        public async Task<IActionResult> QueryPurviewNames([FromQuery] string sysID,[FromQuery] string cultureID)
        {
            var result=await _service.GetPurviewNames(sysID, cultureID);
            return Ok(new
            {
                PurviewNameList = result
            });
        }

        [HttpGet]
        [Route("Detail/{sysID}")]
        public async Task<IActionResult> QuerySysUserPurviewDetails([FromRoute] string sysID, [FromQuery] string userID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSysUserPurviewDetails(sysID, userID, cultureID);
            return Ok(new
            {
                SysUserPurviewDetailList = result
            });
        }

        [HttpPut]
        [Route("Detail")]
        public async Task<IActionResult> EditSysUserPurviewDetail([FromBody] UserPurviewPara userPurviewPara)
        {
            await _service.EditSysUserPurviewDetail(userPurviewPara);
            return Ok();
        }
    }
}
