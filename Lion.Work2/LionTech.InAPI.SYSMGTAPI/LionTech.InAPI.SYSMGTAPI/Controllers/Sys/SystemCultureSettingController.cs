using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/SystemCultureSetting")]
    public class SystemCultureSettingController : ControllerBase
    {
        private readonly ISysCultureSettingService _service;

        public SystemCultureSettingController(ISysCultureSettingService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("IDs")]
        public async Task<IActionResult> QuerySystemCultureIDs()
        {
            var result = await _service.GetSystemCultureIDs();
            return Ok(result);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> QuerySystemCultures([FromQuery] string cultureID, [FromQuery] int pageIndex, [FromQuery] int pageSize)
        {
            var (rowCount, systemCultures) = await _service.GetSystemCultures(cultureID, pageIndex, pageSize);
            return Ok(new
            {
                RowCount = rowCount,
                SystemCultures = systemCultures
            });
        }

        [HttpGet]
        [Route("{cultureID}")]
        public async Task<IActionResult> QuerySystemCultureDetail([FromRoute] string cultureID)
        {
            var result = await _service.GetSystemCultureDetail(cultureID);
            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> EditSystemCultureDetail([FromBody] SystemCulture model)
        {
            await _service.EditSystemCultureDetail(model);
            return Ok();
        }

        [HttpDelete]
        [Route("{cultureID}")]
        public async Task<IActionResult> DeleteSystemCultureDetail([FromRoute] string cultureID)
        {
            await _service.DeleteSystemCultureDetail(cultureID);
            return Ok();
        }

        [HttpPost]
        [Route("GenerateCultureJsonFile")]
        public async Task<IActionResult> GenerateCultureJsonFile()
        {
            await _service.GenerateCultureJsonFile();
            return Ok();
        }
    }
}