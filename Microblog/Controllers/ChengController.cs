using Microblog.Verification_code_class;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Microblog.Models;

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
        public ActionResult Loginshitu()
        {
            return View();
        }


        /// <summary>
        /// 注册
        /// </summary>
        /// <returns>视图</returns>
        ///
        public ActionResult Register()
        {
            return View();
        }
        /// <summary>
        /// 提供验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult VerifyCode()
        {
            string verifyCode = string.Empty;
            Bitmap bitmap = VerifyCodeHelper.CreateVerifyCode(out verifyCode);

            #region 缓存Key 
            Cache cache = new Cache();
            // 先用当前类的全名称拼接上字符串 “verifyCode” 作为缓存的key
            var verifyCodeKey = $"{this.GetType().FullName}_verifyCode";
            cache.Remove(verifyCodeKey);
            cache.Insert(verifyCodeKey, verifyCode);
            #endregion

            MemoryStream memory = new MemoryStream();
            bitmap.Save(memory, ImageFormat.Gif);
            return File(memory.ToArray(), "image/gif");
        }
        [HttpPost]
        public ActionResult Login(string verifyCode, Users model, string querenmima,string yanzhengma)
        {
            // 第一步检验验证码
            // 从缓存获取验证码作为校验基准  
            // 先用当前类的全名称拼接上字符串 “verifyCode” 作为缓存的key
            Cache cache = new Cache();
            var verifyCodeKey = $"{this.GetType().FullName}_verifyCode";
            object cacheobj = cache.Get(verifyCodeKey);
            if (yanzhengma==cacheobj.ToString())
            {
          return RedirectToAction("Loginshitu","Cheng");
            }
            else
            {
                return View("Register");
            }
            //if (cacheobj == null)
            //{
            //    return Json(new
            //    {
            //        Success = false,
            //        Message = "验证码已失效"
            //    });
            //}//不区分大小写比较验证码是否正确
            //else if (!(cacheobj.ToString().Equals(verifyCode, StringComparison.CurrentCultureIgnoreCase)))
            //{
            //    return Json(new
            //    {
            //        Success = false,
            //        Message = "验证码错误"
            //    });
            //}
            //cache.Remove(verifyCodeKey);
            ////...接下来再进行账号密码比对等登录操作
            //return View("Register");
        }


        /// <summary>
        /// 账号设置
        /// </summary>
        /// <returns>视图</returns>
        /// 
        public ActionResult AccountSettings()
        {
            return View();
        }

    }
}