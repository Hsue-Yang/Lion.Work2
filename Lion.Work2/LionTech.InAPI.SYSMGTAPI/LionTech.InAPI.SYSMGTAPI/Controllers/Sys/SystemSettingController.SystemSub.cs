using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    public partial class SystemSettingController
    {
        [HttpGet]
        [Route("UserSystem/SystemSubs")]
        public async Task<IActionResult> QueryUserSystemSubs([FromQuery] string cultureID)
        {
            var result = await _service.GetUserSystemSubList(ClientUserID, cultureID);
            return Ok(result);
        }

        [HttpGet]
        [Route("{sysID}/SystemSub/Ids")]
        public async Task<IActionResult> QuerySystemSubByIds([FromRoute] string sysID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemSubByIdList(sysID, cultureID);
            return Ok(result);
        }

        [HttpGet]
        [Route("{sysID}/SystemSubs")]
        public async Task<IActionResult> QuerySystemSubs([FromRoute] string sysID)
        {
            var result = await _service.GetSystemSubList(sysID);
            return Ok(result);
        }

        [HttpPost]
        [Route("SystemSub")]
        public async Task<IActionResult> EditSystemSub([FromBody] SystemSub systemSub)
        {
            await _service.EditSystemSubDetail(systemSub, ClientUserID);
            return Ok();
        }

        [HttpDelete]
        [Route("SystemSub/{sysID}")]
        public async Task<IActionResult> DeleteSystemSub([FromRoute] string sysID)
        {
            await _service.DeleteSystemSubDetail(sysID);
            return Ok();
        }
    }
}