using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    public partial class SystemRoleController
    {
        [HttpGet]
        [Route("{sysID}/{roleID}/Funs")]
        public async Task<IActionResult> QuerySystemRoleFunsList([FromRoute] string sysID, [FromRoute] string roleID, [FromQuery] string funControllerId, [FromQuery] string cultureID, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var (rowCount, systemRoleFunList) = await _service.GetSystemRoleFunList(sysID, roleID, funControllerId, cultureID, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = rowCount,
                SystemRoleFunsList = systemRoleFunList
            });
        }

        [HttpPost]
        [Route("{sysID}/{roleID}/Funs")]
        public async Task<IActionResult> EditSystemRoleFuns([FromBody] SystemRoleFunEditLists systemRoleFunEditLists)
        {
            await _service.EditSystemRoleFunList(systemRoleFunEditLists);
            return Ok();
        }
    }
}