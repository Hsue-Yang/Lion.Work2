using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/SystemFunToolPara")]
    public class SystemFunToolParaController : Controller
    {
        private readonly ISysFunToolParaService _sysFunToolParaService;

        public SystemFunToolParaController(ISysFunToolParaService sysFunToolParaService)
        {
            _sysFunToolParaService = sysFunToolParaService;
        }

        [HttpGet]
        [Route("Forms/{userID}/{sysID}/{funControllerID}/{funActionName}/{toolNo}/{cultureID}")]
        public async Task<IActionResult> QuerySystemFunToolParaForms([FromRoute] string userID, [FromRoute] string sysID, [FromRoute] string funControllerID, [FromRoute] string funActionName, [FromRoute] string toolNo, [FromRoute] string cultureID)
        {
            var result = await _sysFunToolParaService.GetSystemFunToolParaForms(userID, sysID, funControllerID, funActionName, toolNo, cultureID);
            return Ok(result);
        }

        [HttpGet]
        [Route("{userID}/{sysID}/{funControllerID}/{funActionName}/{toolNo}")]
        public async Task<IActionResult> QuerySystemFunToolParas([FromRoute] string userID, [FromRoute] string sysID, [FromRoute] string funControllerID, [FromRoute] string funActionName, [FromRoute] string toolNo, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var (rowCount, systemFunToolParaList) = await _sysFunToolParaService.GetSystemFunToolParas(userID, sysID, funControllerID, funActionName, toolNo, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = rowCount,
                SystemFunToolParaList = systemFunToolParaList
            });
        }
    }
}
