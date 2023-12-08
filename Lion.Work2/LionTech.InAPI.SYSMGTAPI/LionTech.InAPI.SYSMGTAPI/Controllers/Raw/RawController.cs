using LionTech.InAPI.SYSMGTAPI.Service.Raw.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Raw
{
    [Authorize]
    [ApiController]
    [Route("v1/Raw")]
    public class RawController : ControllerBase
    {
        private readonly IRawService _service;

        public RawController(IRawService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("RawCMOrgCom")]
        public async Task<IActionResult> QueryRawCMOrgComs()
        {
            var result = await _service.GetRawCMOrgComs();
            return Ok(new
            {
                RawCMOrgComs = result
            });
        }

        [HttpGet]
        [Route("RawCMOrgUnit")]
        public async Task<IActionResult> QueryRawCMOrgUnits()
        {
            var result = await _service.GetRawCMOrgUnits();

            return Ok(new
            {
                RawCMOrgUnits = result
            });
        }

        [HttpGet]
        [Route("RawCMBusinessUnit")]
        public async Task<IActionResult> QueryRawCMBusinessUnits(string cultureID)
        {
            var result = await _service.GetRawCMBusinessUnits(cultureID);

            return Ok(new
            {
                RawCMBusinessUnits = result
            });
        }

        [HttpGet]
        [Route("RawCMCountry")]
        public async Task<IActionResult> QueryRawCMCountry(string cultureID)
        {
            var result = await _service.GetRawCMCountries(cultureID);

            return Ok(new
            {
                RawCMCountries = result
            });
        }

        [HttpGet]
        [Route("RawUsers")]
        public async Task<IActionResult> QueryRawUserList([FromQuery] string condition, [FromQuery] int limit )
        {
            var result = await _service.GetRawUsers(condition, limit);
            return Ok(result);
        }
    }
}