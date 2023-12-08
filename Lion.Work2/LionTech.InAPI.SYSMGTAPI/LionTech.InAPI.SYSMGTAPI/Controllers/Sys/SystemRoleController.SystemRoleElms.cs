using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    public partial class SystemRoleController
    {
        [HttpGet]
        [Route("{sysID}/{roleID}/Elms")]
        public async Task<IActionResult> QuerySystemRoleElms([FromRoute] string sysID, [FromRoute] string roleID,
            [FromQuery] string cultureID, [FromQuery] string funControllerId, [FromQuery] string funactionNM,
            [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var (rowCount, systemRoleElmList) = await _service.GetSystemRoleElmsList(sysID, roleID, cultureID,
                funControllerId, funactionNM, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = rowCount,
                systemRoleElmList
            });
        }

        [HttpPost]
        [Route("{sysID}/{roleID}/Elms")]
        public async Task<IActionResult> EditSystemRoleElms([FromBody] SystemRoleElmEditLists systemRoleElmEditLists)
        {
            await _service.EditSystemRoleElmsList(systemRoleElmEditLists);
            return Ok();
        }

        [HttpGet]
        [Route("{sysID}/{cultureID}/ElmIds")]
        public async Task<IActionResult> QuerySystemFunElmIds([FromRoute] string sysID, [FromRoute] string cultureID,
            [FromQuery] string funControllerId, [FromQuery] string funactionNM)
        {
            var result = await _service.GetSystemFunElmByIdList(sysID, cultureID, funControllerId, funactionNM);
            return Ok(new
            {
                systemElmIDList = result
            });
        }
    }
}