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
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;

namespace Microblog.Controllers
{

    public class ChengController : Controller
    {
        MicroblogDBEntities db = new MicroblogDBEntities();

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
            Session["code"] = verifyCode;
            #endregion

            MemoryStream memory = new MemoryStream();
            bitmap.Save(memory, ImageFormat.Gif);
            return File(memory.ToArray(), "image/gif");
        }
        [HttpPost]
        public ActionResult Register(Users model)
        {
            model.user_time = DateTime.Now;
            //// 第一步检验验证码
            //// 从缓存获取验证码作为校验基准  
            //// 先用当前类的全名称拼接上字符串 “verifyCode” 作为缓存的key
            //Cache cache = new Cache();
            //var verifyCodeKey = $"{this.GetType().FullName}_verifyCode";
            //object cacheobj = cache.Get(verifyCodeKey);
            //MicroblogDBEntities3 db = new MicroblogDBEntities3();
            var zhi = db.Users.Where(a => a.user_email == model.user_email).Select(a => a.user_email).ToList();

            if (zhi.Count > 0)
            {

                return Content("<script>alert('该用户已存在');window.location.href='/Cheng/Register';</script>");
            }
            else
            {
                //ModelState.IsValid模型状态验证
                if (ModelState.IsValid)
                {
                    db.usp_zhucetianjia(model.user_email, model.user_password, model.user_name, model.user_time);
                    //db.Users.Add(model);
                    db.SaveChanges();

                    return Content("<script>alert('注册成功');window.location.href='/Cheng/Register';</script>");

                }
                else
                {
                    return View();
                }
            }

        }
        //验证码判断
        [HttpPost]
        public ActionResult CheckCode(string yanzhengma)
        {
            string code = Session["code"].ToString();
            yanzhengma = yanzhengma.ToUpper();
            //ToUpper转换大小写
            if (yanzhengma != code)
            {
                int zhi = 0;
                return Content(zhi.ToString());
            }
            else
            {
                int zhi = 1;
                return Content(zhi.ToString());
            }

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
        //省市级联
        public ActionResult GetProvice()
        {

            List<province> list = db.province.ToList();


            JavaScriptSerializer js = new JavaScriptSerializer();
            string jsonStr = js.Serialize(list);

            return Content(jsonStr);
        }
        //获取省Code
        public ActionResult GetCity(string id)
        {

            //获取省Code
            // string pCode = Request["pcode"].ToString();
            //string sql = string.Format("select * from city where provinceCode='{0}'", pCode);
            //DataTable dt = DBHelper.Select(sql);
            List<city> list = db.city.Where(c => c.provinceCode == id).ToList();

            JavaScriptSerializer js = new JavaScriptSerializer();
            string jsonStr = js.Serialize(list);
            return Content(jsonStr);
        }
        //个人处理信息保存
        [HttpPost]
        public ActionResult AccountSettings(Userinfo model, string sheng, string shi)
        {
            //完成时要传id进来 未完成
            model.user_id = 1;//零时id

            if (sheng != null && shi != null)
            {
                var shengfen = db.province.Where(a => a.code == sheng).FirstOrDefault();
                var shiming = db.city.Where(a => a.code == shi).FirstOrDefault();
                model.userinfo_address = shengfen.name + shiming.name;
            }

            if (ModelState.IsValid)
            {
                //db.Set<实体模型>().AsNoTracking().FirstOrDefault(p => p.x == x)意思是找到这条数据，然后清除SaveChanges()缓存不会报错
                db.Set<Userinfo>().AsNoTracking().FirstOrDefault(m => m.user_id == 1);

                db.Entry(model).State = System.Data.Entity.EntityState.Modified;


                db.SaveChanges();
                return Content("<script>alert('保存成功');window.location.href='/Cheng/AccountSettings';</script>");
            }
            return View();
        }
        //分布修改
        public ActionResult xiugai()
        {

            return View();
        }
        [HttpPost]
        //处理修改密码
        public ActionResult xiugai(Users model)
        {
            model.user_id = 1;//项目完成时要传id过来 未完成


            if (model.user_password.Length >= 6)
            {
                Users models = db.Users.FirstOrDefault(a => a.user_id == model.user_id);
                models.user_password = model.user_password;
                db.SaveChanges();
                return Content("<script>alert('修改成功');window.location.href='/Cheng/xiugai';</script>");
            }

            return View();


        }
        //判断原密码是否正确
        public ActionResult panduanyunamima(string dangqirenmima, int id)
        {
            Users zhi = db.Users.Where(a => a.user_id == id).FirstOrDefault();

            if (zhi.user_password == dangqirenmima)
            {
                return Content("0");
            }
            else
            {
                return Content("1");
            }

        }
    }
}