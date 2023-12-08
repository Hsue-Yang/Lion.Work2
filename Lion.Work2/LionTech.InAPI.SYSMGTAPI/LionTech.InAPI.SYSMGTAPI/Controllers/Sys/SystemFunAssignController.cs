using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/SystemFunAssign")]
    public class SystemFunAssignController : Controller
    {
        private readonly ISysFunAssignService _service;

        public SystemFunAssignController(ISysFunAssignService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{sysID}/{funControllerID}/{funActionName}")]
        public async Task<IActionResult> QuerySystemFunAssigns([FromRoute] string sysID, [FromRoute] string funControllerID, [FromRoute] string funActionName)
        {
            var result = await _service.GetSystemFunAssigns(sysID, funControllerID, funActionName);
            return Ok(new
            {
                SystemFunAssigns = result
            });
        }

        [HttpPost]
        [Route("")]
        public async Task EditSystemFunAssign(SystemFunAssignPara para)
        {
            await _service.EditSystemFunAssign(para);
        }

        [HttpPost]
        [Route("FunRawDatas")]
        public async Task<IActionResult> QueryFunRawDatas([FromBody] List<UserFunctionValue> userFunction, string userID, string cultureID)
        {
            var result = await _service.GetFunRawDatas(userFunction, userID, cultureID);
            return Ok(new
            {
                SysFunNMInfo = result
            });
        }

        [HttpPost]
        [Route("UserRawDatas")]
        public async Task<IActionResult> QueryUserRawDatas([FromBody] List<UserMain> userIDList)
        {
            var result = await _service.GetUserRawDatas(userIDList);
            return Ok(new
            {
                UserRawDataList = result
            });
        }
    }
}
