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
using System.Web.Security;
using System.Collections;
using System.Data;
using Microblog.umodel;
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
        //登录功能处理
        [HttpPost]
        public ActionResult weiboguangchang(string phone, string pass)
        {
            //var zhi = db.Users.Where(a => a.user_email == phone && a.user_password == pass).ToList();

            if (phone != "" && pass != "" && piaoju(phone, pass))
            {
                return Content("<script>alert('登录成功');window.location.href='/Tang/MyHomePage';</script>");
            }
            else
            {
                return Content("<script>alert('账户密码错误');window.location.href='/Cheng/weiboguangchang';</script>");
            }

        }


        //创建身份票据
        public bool piaoju(string phone, string pass)
        {
            Users zhi = db.Users.Where(a => a.user_email == phone && a.user_password == pass).FirstOrDefault();
            if (zhi == null)
            {
                return false;
            }
            else
            {
                //创建身份票据
                FormsAuthentication.SetAuthCookie(zhi.user_id.ToString(), false);
                return true;
            }
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

                    return Content("<script>alert('注册成功');window.location.href='/Cheng/weiboguangchang';</script>");

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
        [Authorize]
        public ActionResult AccountSettings()
        {
            int id = Convert.ToInt32(User.Identity.Name);
            //初始化页面数据
            var shuju = db.Userinfo.FirstOrDefault(a => a.user_id == id);
            return View(shuju);
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
        [Authorize]
        [HttpPost]
        public ActionResult AccountSettings(Userinfo model, string sheng, string shi, string user_name, string userinfo_realname)
        {
            model.user_id = Convert.ToInt32(User.Identity.Name);

            //初始化页面数据
            var shujuer = db.Userinfo.FirstOrDefault(a => a.user_id == model.user_id);
            //初始化页面数据
            var shuju = db.Userinfo.FirstOrDefault(a => a.user_id == model.user_id);

            if (sheng != null || shi != null)
            {
                var shengfen = db.province.Where(a => a.code == sheng).FirstOrDefault();
                var shiming = db.city.Where(a => a.code == shi).FirstOrDefault();
                model.userinfo_address = shengfen.name + shiming.name;
            }



            //昵称不为空
            if (user_name != "")
            {
                var zhi = db.Users.Find(model.user_id);
                zhi.user_name = user_name;
                db.Entry(zhi).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                //db.Configuration.ValidateOnSaveEnabled = true;
                var xiugai = db.Userinfo.Where(a => a.user_id == model.user_id).ToList();
                if (ModelState.IsValid)
                {
                    if (xiugai.Count > 0)
                    {
                        var canshu = db.Userinfo.Where(a => a.user_id == model.user_id).FirstOrDefault();

                        canshu.userinfo_realname = model.userinfo_realname;
                        canshu.userinfo_address = model.userinfo_address;
                        canshu.userinfo_sex = model.userinfo_sex;
                        canshu.userinfo_birthday = model.userinfo_birthday;
                        canshu.userinfo_intro = model.userinfo_intro;
                        canshu.userinfo_qqnumber = model.userinfo_qqnumber;


                        //db.Set<实体模型>().AsNoTracking().FirstOrDefault(p => p.x == x)意思是找到这条数据，然后清除SaveChanges()缓存不会报错
                        //db.Set<Userinfo>().AsNoTracking().FirstOrDefault(m => m.user_id == 1);

                        // db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return Content("<script>alert('保存成功');window.location.href='/Cheng/AccountSettings';</script>");
                    }
                    else
                    {
                        return Content("<script>alert('保存失败');window.location.href='/Cheng/AccountSettings';</script>");
                    }
                }
                else
                {

                    return View(shujuer);
                }
            }

            if (ModelState.IsValid)
            {
                var xiugai = db.Userinfo.Where(a => a.user_id == model.user_id).ToList();

                if (xiugai.Count > 0)
                {
                    var canshu = db.Userinfo.Where(a => a.user_id == model.user_id).FirstOrDefault();

                    canshu.userinfo_realname = model.userinfo_realname;
                    canshu.userinfo_address = model.userinfo_address;
                    canshu.userinfo_sex = model.userinfo_sex;
                    canshu.userinfo_birthday = model.userinfo_birthday;
                    canshu.userinfo_intro = model.userinfo_intro;
                    canshu.userinfo_qqnumber = model.userinfo_qqnumber;


                    //db.Set<实体模型>().AsNoTracking().FirstOrDefault(p => p.x == x)意思是找到这条数据，然后清除SaveChanges()缓存不会报错
                    db.Set<Userinfo>().AsNoTracking().FirstOrDefault(m => m.user_id == 1);

                    //db.Entry(model).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();

                    return Content("<script>alert('保存成功');window.location.href='/Cheng/AccountSettings';</script>");
                }
                else
                {
                    return Content("<script>alert('保存失败');window.location.href='/Cheng/AccountSettings';</script>");
                }
            }

            return View(shujuer);
        }
        //分布修改
        [Authorize]
        public ActionResult xiugai()
        {

            return View();
        }
        [HttpPost]
        [Authorize]
        //处理修改密码
        public ActionResult xiugai(Users model)
        {
            model.user_id = Convert.ToInt32(User.Identity.Name);


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
        public ActionResult panduanyunamima(string dangqirenmima)
        {
            int id = Convert.ToInt32(User.Identity.Name);
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
        //职业信息
        public ActionResult zhiyexinxi()
        {
            return View();
        }
        //微博广场
        public ActionResult weiboguangchang()
        {
            //加载使用微博的人
            //Take()指定返回几条数据
            List<Users> zhi = db.Users.Include("Userinfo").Take(9).ToList();
            List<Users> zhier = db.Users.Include("Userinfo").OrderByDescending(a => a.user_id).Take(9).ToList();
            ViewData["shiyongweiboderen"] = zhier;
            //查找关注度
            //string sql = "select a.user_name,a.user_id,COUNT(b.user_guanzhu) renshu from Users a,Relation b where a.user_id=b.user_id group by a.user_id,a.user_name order by COUNT(b.user_byid) desc";
            string sql = "select b.user_guanzhu,count(a.user_id) fansicount from Users a,Relation b where a.user_id=b.user_id group by b.user_guanzhu order by fansicount desc";
            //返回记录
            var yuju = db.Database.SqlQuery<guanzhulei>(sql).Take(10).ToList();
            //ArrayList shuzu = new ArrayList();
            Dictionary<Users, int> fansiD = new Dictionary<Users, int>();
            foreach (var item in yuju)
            {
                var fansi = db.Users.Where(u => u.user_id == item.user_guanzhu).FirstOrDefault();
                fansiD.Add(fansi, item.fansicount);
            }
            //List<Users> zong = new List<Users>();
            //foreach (var item in fansiD)
            //{
            //    zong.Add(item.Key);
            //}
            
            //foreach (var item in yuju)
            //{
            //    shuzu.Add(item.user_id);
            //    shuzu.Add(item.user_name);
            //    shuzu.Add(item.renshu);
            //}
            ViewData["guanzhushulian"] = fansiD;

            //微博信息
            var shuju = db.Messages.OrderByDescending(m => m.messages_time).ToList();
            ViewData["shuju"] = shuju;

            return View(zhi);
        }
        [Authorize]
        //修改微博邮箱
        public ActionResult xiugaiyouxiang(int id)
        {
            var zhi = db.Users.FirstOrDefault(a => a.user_id == id);

            return View(zhi);
        }
        //处理微博邮箱修改
        [HttpPost]
        public ActionResult xiugaiyouxiang(string user_email, string youxiang, int user_id)
        {
            if (ModelState.IsValid)
            {


                var shuju = db.Users.Where(a => a.user_email == youxiang).ToList();
                if (shuju.Count == 0)
                {
                    //Users zhi = db.Users.FirstOrDefault(a => a.user_email == youxiang);
                    //int id =(int)zhi.user_id;
                    db.usp_xiugaier(youxiang, user_id);
                    db.SaveChanges();
                    return Content("<script>alert('修改成功');window.location.href='/Cheng/AccountSettings';</script>");
                }
                else
                {
                    return Content("<script>alert('该用户已存在');window.location.href='/Cheng/AccountSettings';</script>");
                }

            }


            return View();
        }
        //修改头像
        [Authorize]
        public ActionResult xiugaitouxiang()
        {
            //正式做的时候在这个地方获取身份票据取得id
            return View();
        }
        [HttpPost]
        //处理头像上传
        public ActionResult tupianshanchuan(HttpPostedFileBase user_headphoto)
        {


            //设置保存路径
            string lujing = "/resource/" + user_headphoto.FileName;
            //把传过来的文件保存在自定义路径
            user_headphoto.SaveAs(Server.MapPath(lujing));
            return Content(lujing);

        }
        //处理修改头像
        [HttpPost]
        public ActionResult xiugaitouxiang(HttpPostedFileBase user_headphoto)
        {
            //设置保存路径
            string lujing = "/resource/" + user_headphoto.FileName;
            int id = Convert.ToInt32(User.Identity.Name); ;

            var shuju = db.Userinfo.FirstOrDefault(a => a.user_id == id);
            shuju.user_headphoto = lujing;
            db.SaveChanges();

            return Content("<script>alert('修改成功');window.location.href='/Cheng/xiugaitouxiang';</script>");
        }
        //实时更新局部信息
        public ActionResult _zhengzaishuo()
        {
            return View();
        }
        //退出方法
        public ActionResult tuichu()
        {
            FormsAuthentication.SignOut();//注销
            return Content("<script>alert('退出成功');window.location.href='/Cheng/weiboguangchang';</script>");
        }
        //背景主题上传
        [Authorize]
        public ActionResult zhuti()
        {
            return View();
        }
        //处理背景预览
        [HttpPost]
        public ActionResult bejingshangchuan(HttpPostedFileBase user_headphoto)
        {

            //设置保存路径
            string lujing = "/resource/" + user_headphoto.FileName;
            //把传过来的文件保存在自定义路径
            user_headphoto.SaveAs(Server.MapPath(lujing));
            return Content(lujing);

        }
        //处理背景上传
        
        [HttpPost]
        public ActionResult zhuti(HttpPostedFileBase user_headphoto)
        {
            //设置保存路径
            string lujing = "/resource/" + user_headphoto.FileName;
            int id = Convert.ToInt32(User.Identity.Name);

            var shuju = db.Userinfo.FirstOrDefault(a => a.user_id == id);
            shuju.resources_themeurl = lujing;
            db.SaveChanges();

            //使用cookie
            HttpCookie cookie = new HttpCookie("Backgroundimg"+ User.Identity.Name);
            cookie.Expires = DateTime.MaxValue;
            cookie.Values.Add(User.Identity.Name,lujing);
 
             Response.AppendCookie(cookie);
            return Content("lujing");
        }
    }
}