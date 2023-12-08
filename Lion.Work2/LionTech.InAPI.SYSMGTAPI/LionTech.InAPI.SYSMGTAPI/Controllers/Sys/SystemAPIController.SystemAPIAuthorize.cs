using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    public partial class SystemAPIController
    {
        [HttpGet]
        [Route("{sysID}/{apiGroupID}/{apiFunID}/Authorizes")]
        public async Task<IActionResult> QuerySystemAPIAuthorizes([FromRoute] string sysID, [FromRoute] string apiGroupID, [FromRoute] string apiFunID, [FromQuery]string cultureID)
        {
            var result = await _service.GetSystemAPIAuthorizeList(sysID, apiGroupID, apiFunID, cultureID);
            return Ok(result);
        }

        [HttpPost]
        [Route("Authorize")]
        public async Task<IActionResult> EditSystemAPIAuthorize([FromBody] SystemAPIAuthorize systemAPIAuthorize)
        {
            await _service.EditSystemAPIAuthorize(systemAPIAuthorize);          
            return Ok();
        }

        [HttpDelete]
        [Route("Authorize/{sysID}/{apiGroupID}/{apiFunID}/{apiClientSysID}")]
        public async Task<IActionResult> DeleteSystemAPIAuthorize([FromRoute] string sysID, [FromRoute] string apiGroupID, [FromRoute] string apiFunID, [FromRoute] string apiClientSysID)
        {
            await _service.DeleteSystemAPIAuthorize(sysID, apiGroupID, apiFunID, apiClientSysID);
            return Ok();
        }
    }
}