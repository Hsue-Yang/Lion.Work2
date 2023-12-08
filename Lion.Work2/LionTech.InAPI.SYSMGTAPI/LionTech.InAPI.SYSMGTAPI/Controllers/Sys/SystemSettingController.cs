using LionTech.AspNetCore.Utility.SERP;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/SystemSetting")]
    public partial class SystemSettingController : ControllerBase
    {
        private readonly ISysSettingService _service;
        private readonly string ClientUserID;

        public SystemSettingController(ISysSettingService service, IHttpContextAccessor httpContext)
        {
            ClientUserID = httpContext.HttpContext.Request.Query["ClientUserID"];
            _service = service;
        }

        [HttpGet]
        [Route("IsITManager/{sysID}")]
        public async Task<IActionResult> IsITManager([FromRoute] string sysID)
        {
            bool isITManager = await _service.CheckIsITManager(sysID, ClientUserID);
            return Ok(new
            {
                IsITManager = isITManager
            });
        }

        [HttpGet]
        [Route("CodeManagement")]
        public async Task<IActionResult> QueryCodeManagement([FromQuery] string codeKind, [FromQuery] string cultureID
            , [FromQuery] List<string> codeIDs, [FromQuery] string codeParent = null, [FromQuery] bool isFormatNMID = true)
        {
            var result = await _service.GetCodeManagementList(codeKind, cultureID, codeIDs, codeParent, isFormatNMID);
            return Ok(result);
        }

        [HttpGet]
        [Route("FilePath/{sysID}")]
        public async Task<IActionResult> QuerySystemFilePath([FromRoute] string sysID)
        {
            var filePath = await _service.GetSystemFilePath(sysID);
            return Ok(new
            {
                FilePath = filePath
            });
        }

        [HttpGet]
        [Route("System/Ids")]
        public async Task<IActionResult> QueryAllSystemByIds([FromQuery] string cultureID)
        {
            var result = await _service.GetAllSystemByIdList(cultureID);
            return Ok(result);
        }

        [HttpGet]
        [Route("UserSystem/Ids")]
        public async Task<IActionResult> QueryUserSystemByIds([FromQuery] bool isExcludeOutsourcing, [FromQuery] string cultureID)
        {
            var result = await _service.GetUserSystemByIdList(ClientUserID, isExcludeOutsourcing, cultureID);
            return Ok(result);
        }

        [HttpGet]
        [Route("UserSystems")]
        public async Task<IActionResult> UserSystems([FromQuery] string cultureID)
        {
            bool isITManager = await _service.CheckIsITManager(EnumSystemID.ERPAP.ToString(), ClientUserID);
            var sysSettingList = await _service.GetSystemSettingList(ClientUserID, cultureID);
            return Ok(new
            {
                IsITManager = isITManager,
                SystemSettings = sysSettingList
            });
        }

        [HttpGet]
        [Route("{sysID}")]
        public async Task<IActionResult> QuerySystemSetting([FromRoute] string sysID)
        {
            var main = await _service.GetSystemMain(sysID);
            if (main != null)
            {
                return Ok(main);
            }
            return NotFound();
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> EditSystemSetting([FromBody] SystemMain systemMain, [FromQuery] string clientIPAddress)
        {
            var isManager = await _service.CheckIsITManager(EnumSystemID.ERPAP.ToString(), ClientUserID);

            if (isManager == false)
            {
                return BadRequest();
            }

            var result = await _service.EditSystemSettingDetail(systemMain, ClientUserID, clientIPAddress);

            if (result == true)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete]
        [Route("{sysID}")]
        public async Task<IActionResult> DeleteSystemSetting([FromRoute] string sysID)
        {
            var isManager = await _service.CheckIsITManager(EnumSystemID.ERPAP.ToString(), ClientUserID);

            if (isManager == false)
            {
                return BadRequest();
            }

            var result = await _service.DeleteSystemSettingDetail(sysID);

            if (result == EnumDeleteSystemSettingResult.Success)
            {
                return Ok();
            }

            return BadRequest(new { Message = result.ToString() });
        }

        [HttpGet]
        [Route("UserSystemSysIDs/{userID}")]
        public async Task<IActionResult> QueryUserSystemSysIDs([FromRoute] string userID, bool excludeOutsourcing, string cultureID)
        {
            var result = await _service.GetUserSystemSysIDs(userID, excludeOutsourcing, cultureID);
            return Ok(new
            {
                UserSystemSysIDs = result
            });
        }

        [HttpGet]
        [Route("SystemSysIDs")]
        public async Task<IActionResult> QuerySystemSysIDs(bool excludeOutsourcing, string cultureID)
        {
            var result = await _service.GetSystemSysIDs(excludeOutsourcing, cultureID);
            return Ok(new
            {
                SystemSysIDs = result
            });
        }

        [HttpGet]
        [Route("SystemRoleGroups")]
        public async Task<IActionResult> QuerySystemRoleGroups(string cultureID)
        {
            var result = await _service.GetSystemRoleGroups(cultureID);
            return Ok(new
            {
                SystemRoleGroups = result
            });
        }

        [HttpGet]
        [Route("SystemConditionIDs")]
        public async Task<IActionResult> QuerySystemConditionIDs(string sysID, string cultureID)
        {
            var result = await _service.GetSystemConditionIDs(sysID, cultureID);
            return Ok(new
            {
                SystemConditionIDs = result
            });
        }
    }
}