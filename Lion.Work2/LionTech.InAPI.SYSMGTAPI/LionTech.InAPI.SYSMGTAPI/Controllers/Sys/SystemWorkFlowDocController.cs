using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/SystemWorkFlowDoc")]
    public class SystemWorkFlowDocController : ControllerBase
    {
        private readonly ISysWorkFlowDocumentService _service;
        public SystemWorkFlowDocController(ISysWorkFlowDocumentService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}")]
        public async Task<ActionResult> QuerySystemWorkFlowDocuments([FromRoute] string sysID, [FromRoute] string wfFlowID, [FromRoute] string wfFlowVer, [FromRoute] string wfNodeID, string cultureID)
        {
            var result = await _service.GetSystemWorkFlowDocuments(sysID, wfFlowID, wfFlowVer, wfNodeID, cultureID);
            return Ok(new
            {
                SystemWorkFlowDocuments = result
            });
        }

        [HttpGet]
        [Route("{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}/{wfDocSeq}")]
        public async Task<ActionResult> QuerySystemWorkFlowDocumentDetail([FromRoute] string sysID, [FromRoute] string wfFlowID, [FromRoute] string wfFlowVer, [FromRoute] string wfNodeID, [FromRoute] string wfDocSeq)
        {
            var result = await _service.GetSystemWorkFlowDocumentDetail(sysID, wfFlowID, wfFlowVer, wfNodeID, wfDocSeq);
            return Ok(new
            {
                SystemWorkFlowDocumentDetail = result
            });
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> InsertSystemWorkFlowDocument(SystemWorkFlowDocumentDetail systemWorkFlowDocumentDetail)
        {
            await _service.InsertSystemWorkFlowDocument(systemWorkFlowDocumentDetail);
            return Ok();
        }

        [HttpPut]
        [Route("")]
        public async Task<ActionResult> EditSystemWorkFlowDocument(SystemWorkFlowDocumentDetail systemWorkFlowDocumentDetail)
        {
            await _service.EditSystemWorkFlowDocument(systemWorkFlowDocumentDetail);
            return Ok();
        }

        [HttpDelete]
        [Route("{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}/{wfDocSeq}")]
        public async Task<ActionResult> DeleteSystemWorkFlowDocument([FromRoute] string sysID, [FromRoute] string wfFlowID, [FromRoute] string wfFlowVer, [FromRoute] string wfNodeID, [FromRoute] string wfDocSeq)
        {
            var result = await _service.DeleteSystemWorkFlowDocument(sysID, wfFlowID, wfFlowVer, wfNodeID, wfDocSeq);
            return Ok(result);
        }
    }
}
