using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TRAININGAP.Controllers.Leave
{
    public partial class PracticeController
    {
        [HttpGet]
        //[AuthorizationActionFilter]
        public ActionResult Leave() { return View(); }
    }
}