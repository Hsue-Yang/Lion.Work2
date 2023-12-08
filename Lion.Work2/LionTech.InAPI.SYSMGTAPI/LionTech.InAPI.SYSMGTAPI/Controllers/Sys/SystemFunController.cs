using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using LionTech.InAPI.SYSMGTAPI.Model.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/SystemFun")]
    public partial class SystemFunController : ControllerBase
    {
        private readonly ISysFunService _service;
        private readonly string ClientUserID;

        public SystemFunController(ISysFunService service, IHttpContextAccessor httpContext)
        {
            ClientUserID = httpContext.HttpContext.Request.Query["ClientUserID"];
            _service = service;
        }

        [HttpGet]
        [Route("UserAuthorization")]
        public async Task<IActionResult> QueryUserSystemFuns([FromQuery] string cultureID)
        {
            var result = await _service.GetUserSystemFunList(ClientUserID, cultureID);
            return Ok(result);
        }

        [HttpGet]
        [Route("{sysID}")]
        public async Task<IActionResult> QuerySystemFuns([FromRoute] string sysID, [FromQuery] SystemFunModel model, [FromQuery] string cultureID, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var result = await _service.GetSystemFunList(sysID, model.SubSysID, model.FunControllerID, model.FunActionName, model.FunGroupNM, model.FunNM, model.FunMenuSysID, model.FunMenu, cultureID, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = result.rowCount,
                SystemFuns = result.SystemFunList
            });
        }

        [HttpGet]
        [Route("{sysID}/{funControllerID}/{funActionName}")]
        public async Task<IActionResult> QuerySystemFun([FromRoute] string sysID, [FromRoute] string funControllerID, [FromRoute] string funActionName, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemFunDetail(sysID, funControllerID, funActionName, cultureID);
            return Ok(result);
        }

        [HttpGet]
        [Route("{sysID}/Roles")]
        public async Task<IActionResult> QuerySystemFunRoles([FromRoute] string sysID, [FromQuery] string funControllerID, [FromQuery] string funActionName, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemFunRoleList(sysID, funControllerID, funActionName, cultureID);
            return Ok(result);
        }

        [HttpPost]
        [Route("Purview")]
        public async Task<IActionResult> EditSystemFunByPurview([FromBody] SystemFunMain systemFun)
        {
            await _service.EditSystemFunByPurview(systemFun);
            return Ok();
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> EditSystemFunDetail([FromBody] SystemFunDetailModel model)
        {
            await _service.EditSystemFunDetail(
                model.GetDataTable(),
                model.GetSystemRoleFunDataTable(),
                model.GetSystemMenuFunDataTable());
            return Ok();
        }

        [HttpDelete]
        [Route("{sysID}/{funControllerID}/{funActionName}")]
        public async Task<IActionResult> DeleteSystemFun([FromRoute] string sysID, [FromRoute] string funControllerID, [FromRoute] string funActionName)
        {
            var result = await _service.DeleteSystemFunDetail(sysID, funControllerID, funActionName);
            if (result == EnumDeleteSystemFunDetailResult.Success)
            {
                return Ok();
            }
            return BadRequest(new { Message = result.ToString() });
        }

        [HttpGet]
        [Route("MenuFuns/{sysID}/{funControllerID}/{funActionName}")]
        public async Task<IActionResult> QuerySystemMenuFuns([FromRoute] string sysID, [FromRoute] string funControllerID, [FromRoute] string funActionName)
        {
            var result = await _service.GetSystemMenuFunList(sysID, funControllerID, funActionName);
            return Ok(result);
        }

        [HttpGet]
        [Route("FunNames")]
        public async Task<IActionResult> QuerySystemFunNames([FromQuery] string sysID, [FromQuery] string funControllerID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemFunNameList(sysID, funControllerID, cultureID);
            return Ok(new
            {
                systemFunNameList = result
            });
        }

        [HttpGet]
        [Route("FunActions")]
        public async Task<IActionResult> QuerySystemFunActions([FromQuery] string cultureID)
        {
            var result = await _service.GetSystemFunActionList(cultureID);
            return Ok(new
            {
                systemFunActionList = result
            });
        }
    }
}