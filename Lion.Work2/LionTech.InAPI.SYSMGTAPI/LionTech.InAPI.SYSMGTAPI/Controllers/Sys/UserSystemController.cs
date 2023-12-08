using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/UserSystem")]
    public class UserSystemController : ControllerBase
    {
        private readonly IUserSystemService _service;

        public UserSystemController(IUserSystemService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> QueryUserStatusList(string userID, string userNM, int pageIndex, int pageSize)
        {
            var result = await _service.GetUserStatusList(userID, userNM, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = result.rowCount,
                UserSystems = result.UserSystemList
            });
        }

        [HttpGet]
        [Route("Detail/{userID}")]
        public async Task<IActionResult> QueryUserRawData([FromRoute] string userID)
        {
            var result = await _service.GetUserRawData(userID);
            return Ok(new
            {
                UserRawData = result
            });
        }

        [HttpGet]
        [Route("Role/{userID}")]
        public async Task<IActionResult> QueryUserOutSourcingSystemRoles([FromRoute] string userID, string cultureID)
        {
            var result = await _service.GetUserOutSourcingSystemRoles(userID, cultureID);
            return Ok(new
            {
                UserSystemRoles = result
            });
        }

        [HttpPost]
        [Route("")]
        public async Task EditUserOutSourcingSystemRoles(UserSystemRoleParas para)
        {
            await _service.EditUserOutSourcingSystemRoles(para);
        }
    }
}