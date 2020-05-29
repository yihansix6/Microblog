using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Microblog.Controllers
{
    public class TangController : Controller
    {
        // GET: Tang
        public ActionResult MyMicroblog()
        {
            return View();
        }
    }
}