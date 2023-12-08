using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/SystemRecord")]
    public class SystemRecordController : ControllerBase
    {
        private readonly ISysRecordService _service;
        public SystemRecordController(ISysRecordService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> QuerySystemRecord([FromQuery] LogPara logPara)
        {
            //開頭為大寫英文(SQL)
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
            };
            var result = await _service.GetSystemRecord(logPara);

            //不要取有null的欄位
            string jsonStr = JsonConvert.SerializeObject(result, settings);
            return Ok(jsonStr);
        }
    }
}
