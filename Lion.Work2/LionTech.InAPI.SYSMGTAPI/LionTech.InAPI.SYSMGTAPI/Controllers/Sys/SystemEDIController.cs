using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LionTech.InAPI.SYSMGTAPI.Controllers.Sys
{
    [Authorize]
    [ApiController]
    [Route("v1/SystemEDI")]
    public partial class SystemEDIController: ControllerBase
    {
        private readonly ISysEDIService _service;

        public SystemEDIController(ISysEDIService service)
        {
            _service = service;
        }
    }
}
