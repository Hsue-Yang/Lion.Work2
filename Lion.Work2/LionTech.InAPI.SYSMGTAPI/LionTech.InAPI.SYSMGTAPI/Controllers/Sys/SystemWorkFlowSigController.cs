using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/SystemWorkFlowSig")]
    public class SystemWorkFlowSigController : ControllerBase
    {
        private readonly ISysWorkFlowSignatureService _service;

        public SystemWorkFlowSigController(ISysWorkFlowSignatureService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}")]
        public async Task<IActionResult> QuerySystemWorkFlowSignatures([FromRoute] string sysID, [FromRoute] string wfFlowID, [FromRoute] string wfFlowVer, [FromRoute] string wfNodeID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemWorkFlowSignatures(sysID, wfFlowID, wfFlowVer, wfNodeID, cultureID);
            return Ok(new
            {
                SystemWFSigList = result
            });
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> EditSystemWorkFlowNode([FromBody] SystemWFNode systemWFNode)
        {
            var result = await _service.EditSystemWorkFlowNode(systemWFNode);
            return Ok(new
            {
                ReturnSystemWFNode = result
            });
        }

        [HttpGet]
        [Route("Seqs/{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}")]
        public async Task<IActionResult> QuerySystemWorkFlowSignatureSeqs([FromRoute] string sysID, [FromRoute] string wfFlowID, [FromRoute] string wfFlowVer, [FromRoute] string wfNodeID, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemWorkFlowSignatureSeqs(sysID, wfFlowID, wfFlowVer, wfNodeID, cultureID);
            return Ok(new
            {
                SystemWFSigSeqList = result
            });
        }

        [HttpGet]
        [Route("Detail/{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}/{wfSigSeq}")]
        public async Task<IActionResult> QuerytSystemWorkFlowSignatureDetail([FromRoute] string sysID, [FromRoute] string wfFlowID, [FromRoute] string wfFlowVer, [FromRoute] string wfNodeID, [FromRoute] string wfSigSeq)
        {
            var result = await _service.GetSystemWorkFlowSignatureDetail(sysID, wfFlowID, wfFlowVer, wfNodeID, wfSigSeq);
            return Ok(new
            {
                SystemWFSigDetail = result
            });
        }

        [HttpGet]
        [Route("Roles/{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}")]
        public async Task<IActionResult> QuerytSystemRoleSignatures([FromRoute] string sysID, [FromRoute] string wfFlowID, [FromRoute] string wfFlowVer, [FromRoute] string wfNodeID, [FromQuery] string wfSigSeq, [FromQuery] string cultureID)
        {
            var result = await _service.GetSystemRoleSignatures(sysID, wfFlowID, wfFlowVer, wfNodeID, wfSigSeq, cultureID);
            return Ok(new
            {
                SystemRoleSigList = result
            });
        }

        [HttpPost]
        [Route("Detail")]
        public async Task<IActionResult> InsertSystemWorkFlowSignatureDetail([FromBody] SystemWFSignatureDetail systemWFSignatureDetail)
        {
            await _service.InsertSystemWorkFlowSignatureDetail(systemWFSignatureDetail);
            return Ok();
        }

        [HttpPut]
        [Route("Detail")]
        public async Task<IActionResult> EditSystemWorkFlowSignatureDetail([FromBody] SystemWFSignatureDetail systemWFSignatureDetail)
        {
            await _service.EditSystemWorkFlowSignatureDetail(systemWFSignatureDetail);
            return Ok();
        }

        [HttpDelete]
        [Route("Detail/{sysID}/{wfFlowID}/{wfFlowVer}/{wfNodeID}")]
        public async Task<IActionResult> DeleteSystemWorkFlowSignatureDetail([FromRoute] string sysID, [FromRoute] string wfFlowID, [FromRoute] string wfFlowVer, [FromRoute] string wfNodeID, [FromQuery] string wfSigSeq)
        {
            await _service.DeleteSystemWorkFlowSignatureDetail(sysID, wfFlowID, wfFlowVer, wfNodeID, wfSigSeq);
            return Ok();
        }
    }
}
