﻿using System.Web.Mvc;

namespace B2PAP.Controllers
{
    public partial class HomeController
    {
        [HttpGet]
        public ActionResult GenericError()
        {
            return View();
        }
    }
}