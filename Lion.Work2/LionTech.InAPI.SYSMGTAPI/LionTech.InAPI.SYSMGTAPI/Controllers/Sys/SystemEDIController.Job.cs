using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    public partial class SystemEDIController
    {

        [HttpGet]
        [Route("Job/{sysID}/{ediFlowID}")]
        public async Task<IActionResult> QuerySystemEDIJobs([FromRoute] string sysID, [FromRoute] string ediFlowID, [FromQuery] string ediJobType, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemEDIJobs(sysID, ediFlowID, ediJobType, cultureID);
            return Ok(result);
        }

        [HttpGet]
        [Route("Job/{sysID}/{ediFlowID}/{ediJobID}")]
        public async Task<IActionResult> QuerySystemEDIJobDetail([FromRoute] string sysID, [FromRoute] string ediFlowID, [FromRoute] string ediJobID)
        {
            var result = await _service.GetSystemEDIJobDetail(sysID, ediFlowID, ediJobID);
            return Ok(result);
        }

        [HttpGet]
        [Route("Job/{sysID}/{ediFlowID}/Ids")]
        public async Task<IActionResult> QuerySystemEDIJobByIds([FromRoute] string sysID, [FromRoute] string ediFlowID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemEDIJobByIds(sysID, ediFlowID, cultureID);
            return Ok(result);
        }

        [HttpGet]
        [Route("Job/{sysID}/{ediFlowID}/SortOrder")]
        public async Task<IActionResult> QueryJobMaxSortOrder([FromRoute] string sysID, [FromRoute] string ediFlowID)
        {
            var result = await _service.GetJobMaxSortOrder(sysID, ediFlowID);
            return Ok(result);
        }

        [HttpGet]
        [Route("Job/{sysID}/{ediFlowID}/{ediJobID}/Paras")]
        public async Task<IActionResult> QuerySystemEDIParas([FromRoute] string sysID, [FromRoute] string ediFlowID, [FromRoute] string ediJobID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemEDIParas(sysID, ediFlowID, ediJobID, cultureID);
            return Ok(result);
        }

        [HttpPost]
        [Route("Job/Sort")]
        public async Task<IActionResult> EditSystemEDIJobSortOrder([FromBody] List<EditEDIJobValue> editSystemEDIJob)
        {
            await _service.EditSystemEDIJobSortOrder(editSystemEDIJob);
            return Ok();
        }

        [HttpPost]
        [Route("Job/Detail")]
        public async Task<IActionResult> EditSystemEDIJobDetail([FromBody] EditEDIJobDetail editSystemEDIFlowDetailList)
        {
            await _service.EditSystemEDIJobDetail(editSystemEDIFlowDetailList);
            return Ok();
        }

        [HttpPost]
        [Route("Job/Import")]
        public async Task<IActionResult> EditSystemEDIJobImport([FromBody] EdiJobSettingPara ediJobSettingPara)
        {
            await _service.EditSystemEDIJobImport(ediJobSettingPara);
            return Ok();
        }

        [HttpDelete]
        [Route("Job/{sysID}/{ediFlowID}/{ediJobID}")]
        public async Task<IActionResult> DeleteSystemEDIJobDetail([FromRoute] string sysID, [FromRoute] string ediFlowID, [FromRoute] string ediJobID)
        {
            var result = await _service.DeleteSystemEDIJobDetail(sysID, ediFlowID, ediJobID);
            if (result == EnumDeleteResult.Success || result == EnumDeleteResult.DataExist)
            {
                return Ok(result.ToString());
            }
            return BadRequest(result.ToString());
        }

        [HttpPost]
        [Route("Job/Para/Sort")]
        public async Task<IActionResult> EditSystemEDIParaSortOrder([FromBody] List<EditEDIJobPara> editEDIJobParas)
        {
            await _service.EditSystemEDIParaSortOrder(editEDIJobParas);
            return Ok();
        }

        [HttpPost]
        [Route("Job/Para")]
        public async Task<IActionResult> EditSystemEDIPara([FromBody] EditEDIJobPara editEDIJobPara)
        {
            await _service.EditSystemEDIPara(editEDIJobPara);
            return Ok();
        }

        [HttpDelete]
        [Route("Job/Para/{sysID}/{ediFlowID}/{ediJobID}")]
        public async Task<IActionResult> DeleteSystemEDIPara([FromRoute] string sysID, [FromRoute] string ediFlowID, [FromRoute] string ediJobID, [FromQuery] string ediJobParaID)
        {
            await _service.DeleteSystemEDIPara(sysID, ediFlowID, ediJobID, ediJobParaID);
            return Ok();
        }

        [HttpGet]
        [Route("Job/{sysID}/{ediFlowID}/ByIdsProvider")]
        public async Task<IActionResult> QuerySystemEDIConByIdsProviderCons([FromRoute] string sysID, [FromRoute] string ediFlowID)
        {
            var result = await _service.GetSystemEDIConByIdsProviderCons(sysID, ediFlowID);
            return Ok(result);
        }

        [HttpGet]
        [Route("Job/{sysID}/{ediFlowID}/ConByIds")]
        public async Task<IActionResult> QuerySystemEDIConByIds([FromRoute] string sysID, [FromRoute] string ediFlowID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemEDIConByIds(sysID, ediFlowID, cultureID);
            return Ok(result);
        }
    }
}
