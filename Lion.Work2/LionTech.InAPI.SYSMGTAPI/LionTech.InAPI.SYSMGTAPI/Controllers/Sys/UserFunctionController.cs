using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/UserFunction")]
    public class UserFunctionController : ControllerBase
    {
        private readonly IUserFunctionService _service;

        public UserFunctionController(IUserFunctionService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{userID}/{updUserID}")]
        public async Task<ActionResult> QueryUserFunctions([FromRoute] string userID, [FromRoute] string updUserID, string cultureID)
        {
            var result = await _service.GetUserFunctions(userID, updUserID, cultureID);
            return Ok(new
            {
                UserFunctionList = result
            });
        }

        [HttpPost]
        [Route("")]
        public async Task EditUserFunction(UserFunctionDetail para)
        {
            await _service.EditUserFunction(para);
        }
    }
}