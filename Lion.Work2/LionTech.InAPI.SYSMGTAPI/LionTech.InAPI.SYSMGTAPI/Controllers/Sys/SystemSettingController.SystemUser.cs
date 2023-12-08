using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    public partial class SystemSettingController
    {
        [HttpGet]
        [Route("{sysID}/SystemUsers")]
        public async Task<IActionResult> QuerySystemUsers([FromRoute] string sysID, [FromQuery] string queryUserID, [FromQuery] string queryUserNM, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var result = await _service.GetSystemUserList(sysID, queryUserID, queryUserNM, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = result.rowCount,
                SystemUsers = result.systemUserList
            });
        }
    }
}