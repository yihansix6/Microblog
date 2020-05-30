using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Microblog.Controllers
{
    public class ChengController : Controller
    {
        // GET: Cheng
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <returns>视图</returns>
        public ActionResult Register()
        {
            return View();
        }
    }
}