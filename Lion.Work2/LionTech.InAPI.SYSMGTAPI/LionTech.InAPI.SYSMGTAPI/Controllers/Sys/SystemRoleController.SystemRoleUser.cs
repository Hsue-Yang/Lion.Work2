using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    public partial class SystemRoleController
    {
        [HttpGet]
        [Route("{sysID}/{roleID}/Users")]
        public async Task<IActionResult> QuerySystemRoleUsers([FromRoute] string sysID, [FromRoute] string roleID, [FromQuery] string userID, [FromQuery] string userNM, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var (rowCount , systemRoleUserList) = await _service.GetSystemRoleUserList(sysID, roleID, userID, userNM, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = rowCount,
                SystemRoleUserList = systemRoleUserList
            });
        }
    }
}