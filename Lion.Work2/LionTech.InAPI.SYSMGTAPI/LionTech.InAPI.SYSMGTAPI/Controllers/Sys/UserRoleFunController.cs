using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/UserRoleFun")]
    public partial class UserRoleFunController : ControllerBase
    {
        private readonly IUserRoleFunService _service;

        public UserRoleFunController(IUserRoleFunService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> QueryUserRoleFuns([FromQuery] string userID, [FromQuery] string userNM, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var result = await _service.GetUserRoleFuns(userID, userNM, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = result.rowCount,
                SysUserRoleFunList = result.userRoleFunList
            });
        }

        [HttpGet]
        [Route("UserMainInfo/{userID}")]
        public async Task<IActionResult> QueryUserMainInfo([FromRoute] string userID)
        {
            var result = await _service.GetUserMainInfo(userID);
            return Ok(result);
        }

        [HttpGet]
        [Route("SystemRoleGroupCollects/{roleGroupID}")]
        public async Task<IActionResult> QuerySystemRoleGroupCollects([FromRoute] string roleGroupID)
        {
            var result = await _service.GetSystemRoleGroupCollects(roleGroupID);
            return Ok(result);
        }

        [HttpGet]
        [Route("UserSystemRoles/{userID}/{updUserID}/{cultureID}")]
        public async Task<IActionResult> QueryUserSystemRoles([FromRoute] string userID, [FromRoute] string updUserID, [FromRoute] string cultureID)
        {
            var result = await _service.GetUserSystemRoles(userID, updUserID, cultureID);
            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> EditUserSystemRole([FromBody] UserRoleFunDetailParaList userRoleFunDetailParaList)
        {
            await _service.EditUserSystemRole(
                userRoleFunDetailParaList.userRoleFunDetailPara,
                userRoleFunDetailParaList.userRoleFunDetailParaList);
            return Ok();
        }

        [HttpGet]
        [Route("UserMenuFuns/{userID}/{cultureID}")]
        public async Task<IActionResult> QueryUserMenuFuns([FromRoute] string userID, [FromRoute] string cultureID)
        {
            var result = await _service.GetUserMenuFuns(userID, cultureID);
            return Ok(result);
        }
    }
}
