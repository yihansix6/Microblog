using System;
using System.Collections;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Microblog.Models;
using Microsoft.Ajax.Utilities;

namespace Microblog.Controllers
{
    public class TangController : Controller
    {
        MicroblogDBEntities db = new MicroblogDBEntities();
        // GET: Tang
        [Authorize]
        public ActionResult MyHomePage()
        {
            int id = Convert.ToInt32(User.Identity.Name);

            //换背景图
            HttpCookie cookie = new HttpCookie("Backgroundimg"+id);
            if (Request.Cookies["Backgroundimg"+id] != null)
            {
                TempData["url"] = Request.Cookies["Backgroundimg"+id][User.Identity.Name];
            }
                

            List<Microblog.Models.Messages> one = new List<Messages>();
            List<int> guanzhulist = new List<int>();
            var wcount = db.Relation.Where(r => r.user_id == id && r.user_guanzhu != null).ToList();
            foreach (var item in wcount)
            {
                guanzhulist.Add(item.Users.user_id);
            }
            foreach (var item in guanzhulist)
            {
                one.Add(db.Messages.Where(m => m.user_id == item).FirstOrDefault());
            }
            var myweibo = db.Messages.Where(m => m.user_id == id).ToList();
            foreach (var item in myweibo)
            {
                one.Add(item);
            }
            //var shuju = one.OrderByDescending(s => s.messages_time).ToList();

            var xinxi = db.Users.FirstOrDefault(x => x.user_id == id);
            ViewData["uname"] = xinxi.user_name; //用户名
            ViewData["uaddress"] = xinxi.Userinfo.userinfo_address;  //用户地址
            ViewData["uheadphoto"] = xinxi.Userinfo.user_headphoto;//用户头像
            ViewData["uId"] = id;

            var count = db.Messages.Where(m => m.user_id == id).ToList();
            ViewData["weibocount"] = count.Count;  //发微博的条数

            var fan = db.Relation.Select(r => r).ToList();
            int fansi = 0;
            int guanzhu = 0;
            ArrayList guanzhuid = new ArrayList();
            ArrayList fansiid = new ArrayList();
            foreach (var item in fan)
            {
                if (item.user_guanzhu == id && fansi <= 6)
                {
                    fansi++;
                    fansiid.Add(item.user_id);
                }
                if (item.user_id == id && guanzhu <= 6)
                {
                    guanzhu++;
                    guanzhuid.Add(item.user_guanzhu);
                }
            }
            List<Users> guanzhuHedaphoto = new List<Users>();
            List<Users> fansiHeadphoto = new List<Users>();
            var Alluser = db.Users.Select(u => u).ToList();
            foreach (var item in Alluser)
            {
                for (int i = 0; i < guanzhuid.Count; i++)
                {
                    if ((int)guanzhuid[i] == item.user_id)
                    {
                        guanzhuHedaphoto.Add(item);
                    }
                }
                for (int i = 0; i < fansiid.Count; i++)
                {
                    if ((int)fansiid[i] == item.user_id)
                    {
                        fansiHeadphoto.Add(item);
                    }
                }
            }
            ViewData["fansiHeadphoto"] = fansiHeadphoto;
            ViewData["guanzhuHedaphoto"] = guanzhuHedaphoto;
            ViewData["guanzhu"] = guanzhu;  //关注数量
            ViewData["fansi"] = fansi;  //粉丝数量

            return View(one);
        }

        [Authorize]
        public ActionResult MyMicroblog()
        {
            int id = Convert.ToInt32(User.Identity.Name);

            //换背景图
            HttpCookie cookie = new HttpCookie("Backgroundimg"+id);
            if (Request.Cookies["Backgroundimg"+id] != null)
            {
                TempData["url"] = Request.Cookies["Backgroundimg"+id][User.Identity.Name];
            }

            var shuju = db.Messages.Where(m => m.user_id == id).OrderByDescending(o => o.messages_time).ToList();
            var info = db.Users.Where(u => u.user_id == id).FirstOrDefault();
            ViewData["mname"] = info.user_name;
            ViewData["maddress"] = info.Userinfo.userinfo_address;
            ViewData["mHeadphoto"] = info.Userinfo.user_headphoto;
            ViewData["mId"] = id;

            var count = db.Messages.Where(m => m.user_id == id).ToList();
            ViewData["weibocount"] = count.Count;  //发微博的条数

            var fan = db.Relation.Select(r => r).ToList();
            int fansi = 0;
            int guanzhu = 0;
            ArrayList guanzhuid = new ArrayList();
            ArrayList fansiid = new ArrayList();
            foreach (var item in fan)
            {
                if (item.user_guanzhu == id && fansi <= 6)
                {
                    fansi++;
                    fansiid.Add(item.user_id);
                }
                if (item.user_id == id && guanzhu <= 6)
                {
                    guanzhu++;
                    guanzhuid.Add(item.user_guanzhu);

                }
            }
            List<Users> guanzhuHedaphoto = new List<Users>();
            List<Users> fansiHeadphoto = new List<Users>();
            var Alluser = db.Users.Select(u => u).ToList();
            foreach (var item in Alluser)
            {
                for (int i = 0; i < guanzhuid.Count; i++)
                {
                    if ((int)guanzhuid[i] == item.user_id)
                    {

                        guanzhuHedaphoto.Add(item);
                    }
                }
                for (int i = 0; i < fansiid.Count; i++)
                {
                    if ((int)fansiid[i] == item.user_id)
                    {
                        fansiHeadphoto.Add(item);
                    }
                }
            }
            ViewData["fansiHeadphoto"] = fansiHeadphoto;
            ViewData["guanzhuHedaphoto"] = guanzhuHedaphoto;
            ViewData["guanzhu"] = guanzhu;  //关注数量
            ViewData["fansi"] = fansi;  //粉丝数量

            return View(shuju);
        }

        public ActionResult TaMicroblog(int id)
        {
            //换背景图
            HttpCookie cookie = new HttpCookie("Backgroundimg"+id);
            if (Request.Cookies["Backgroundimg"+id] != null)
            {
                TempData["url"] = Request.Cookies["Backgroundimg"+id][id.ToString()];
            }

            var shuju = db.Messages.Where(m => m.user_id == id).OrderByDescending(o => o.messages_time).ToList();
            var info = db.Users.Where(u => u.user_id == id).FirstOrDefault();
            ViewData["mname"] = info.user_name;
            ViewData["maddress"] = info.Userinfo.userinfo_address;
            ViewData["mHeadphoto"] = info.Userinfo.user_headphoto;
            ViewData["Tid"] = id;

            if (User.Identity.Name == null || User.Identity.Name == "") { }
            else
            {
                int userid = Convert.ToInt32(User.Identity.Name);
                ViewData["isGuanzhu"] = false;
                var isguanzhu = db.Relation.Where(r => r.user_id == userid).ToList();
                foreach (var item in isguanzhu)
                {
                    if (item.user_guanzhu == id)
                    {
                        ViewData["isGuanzhu"] = true;
                        break;
                    }
                }
            }
            var count = db.Messages.Where(m => m.user_id == id).ToList();
            ViewData["weibocount"] = count.Count;  //发微博的条数

            var fan = db.Relation.Select(r => r).ToList();
            int fansi = 0;
            int guanzhu = 0;
            ArrayList guanzhuid = new ArrayList();
            ArrayList fansiid = new ArrayList();
            foreach (var item in fan)
            {
                if (item.user_guanzhu == id && fansi <= 6)
                {
                    fansi++;
                    fansiid.Add(item.user_id);
                }
                if (item.user_id == id && guanzhu <= 6)
                {
                    guanzhu++;
                    guanzhuid.Add(item.user_guanzhu);

                }
            }
            List<Users> guanzhuHedaphoto = new List<Users>();
            List<Users> fansiHeadphoto = new List<Users>();
            var Alluser = db.Users.Select(u => u).ToList();
            foreach (var item in Alluser)
            {
                for (int i = 0; i < guanzhuid.Count; i++)
                {
                    if ((int)guanzhuid[i] == item.user_id)
                    {

                        guanzhuHedaphoto.Add(item);
                    }
                }
                for (int i = 0; i < fansiid.Count; i++)
                {
                    if ((int)fansiid[i] == item.user_id)
                    {
                        fansiHeadphoto.Add(item);
                    }
                }
            }
            ViewData["fansiHeadphoto"] = fansiHeadphoto;
            ViewData["guanzhuHedaphoto"] = guanzhuHedaphoto;
            ViewData["guanzhu"] = guanzhu;  //关注数量
            ViewData["fansi"] = fansi;  //粉丝数量

            return View(shuju);
        }

        [Authorize]
        public ActionResult GuanzhuList(int id)
        {
            var xinxi = db.Users.FirstOrDefault(x => x.user_id == id);
            ViewData["uname"] = xinxi.user_name; //用户名
            ViewData["uaddress"] = xinxi.Userinfo.userinfo_address;  //用户地址
            ViewData["uheadphoto"] = xinxi.Userinfo.user_headphoto;//用户头像
            ViewData["uId"] = id;
            TempData["lid"] = id;

            var count = db.Messages.Where(m => m.user_id == id).ToList();
            ViewData["weibocount"] = count.Count;  //发微博的条数

            var fan = db.Relation.Select(r => r).ToList();
            int fansi = 0;
            int guanzhu = 0;
            ArrayList guanzhuid = new ArrayList();
            ArrayList fansiid = new ArrayList();
            foreach (var item in fan)
            {
                if (item.user_guanzhu == id && fansi <= 6)
                {
                    fansi++;
                    fansiid.Add(item.user_id);
                }
                if (item.user_id == id && guanzhu <= 6)
                {
                    guanzhu++;
                    guanzhuid.Add(item.user_guanzhu);
                }
            }
            List<Users> guanzhuHedaphoto = new List<Users>();
            var Alluser = db.Users.Select(u => u).ToList();
            foreach (var item in Alluser)
            {
                for (int i = 0; i < guanzhuid.Count; i++)
                {
                    if ((int)guanzhuid[i] == item.user_id)
                    {
                        guanzhuHedaphoto.Add(item);
                    }
                }
            }
            ViewData["guanzhu"] = guanzhu;  //关注数量
            ViewData["fansi"] = fansi;  //粉丝数量
            guanzhuHedaphoto.Reverse();
            return View(guanzhuHedaphoto);
        }

        [Authorize]
        public ActionResult FansiList(int id)
        {
            var xinxi = db.Users.FirstOrDefault(x => x.user_id == id);
            ViewData["uname"] = xinxi.user_name; //用户名
            ViewData["uaddress"] = xinxi.Userinfo.userinfo_address;  //用户地址
            ViewData["uheadphoto"] = xinxi.Userinfo.user_headphoto;//用户头像
            ViewData["uId"] = id;

            var count = db.Messages.Where(m => m.user_id == id).ToList();
            ViewData["weibocount"] = count.Count;  //发微博的条数

            var fan = db.Relation.Select(r => r).ToList();
            int fansi = 0;
            int guanzhu = 0;
            ArrayList guanzhuid = new ArrayList();
            ArrayList fansiid = new ArrayList();
            foreach (var item in fan)
            {
                if (item.user_guanzhu == id && fansi <= 6)
                {
                    fansi++;
                    fansiid.Add(item.user_id);
                }
                if (item.user_id == id && guanzhu <= 6)
                {
                    guanzhu++;
                    guanzhuid.Add(item.user_guanzhu);
                }
            }
            List<Users> fansiHeadphoto = new List<Users>();
            var Alluser = db.Users.Select(u => u).ToList();
            foreach (var item in Alluser)
            {
                for (int i = 0; i < fansiid.Count; i++)
                {
                    if ((int)fansiid[i] == item.user_id)
                    {
                        fansiHeadphoto.Add(item);
                    }
                }
            }
            ViewData["guanzhu"] = guanzhu;  //关注数量
            ViewData["fansi"] = fansi;  //粉丝数量

            return View(fansiHeadphoto);
        }

        [Authorize]
        public ActionResult ProcessImage(HttpPostedFileBase img)
        {

            string fileName = img.FileName;
            string path = "/resource/" + fileName;
            img.SaveAs(Server.MapPath(path));
            return Content(path);
        }

        [Authorize]
        [ValidateInput(false)]
        public ActionResult Addweibo(string Text)
        {
            Messages model = new Messages();
            int id = Convert.ToInt32(User.Identity.Name);
            //int textinfonum = Text.IndexOf("img") + 3;
            //Text = Text.Insert(textinfonum, " class='weiboimg' ");
            model.messages_info = Text;
            model.messages_time = DateTime.Now;
            model.messages_collectnum = 0;
            model.messages_commentnum = 0;
            model.messages_transpondnum = 0;
            model.messages_key = "key" + DateTime.Now.ToString("yyyyMMddHHmmss");
            model.user_id = id;
            db.Messages.Add(model);
            db.SaveChanges();
            return Content("发布成功！");
        }

        [Authorize]
        public ActionResult Delweibo(int id)
        {
            var model = db.Messages.Where(m => m.messages_id == id).FirstOrDefault();
            db.Messages.Remove(model);
            db.SaveChanges();
            return Content("<script>alert('删除成功');window.location.href='/Tang/MyHomePage';</script>");
        }

        [Authorize]
        public ActionResult MyDelweibo(int id)
        {
            var model = db.Messages.Where(m => m.messages_id == id).FirstOrDefault();
            db.Messages.Remove(model);
            db.SaveChanges();
            return Content("<script>alert('删除成功');window.location.href='/Tang/MyMicroblog';</script>");
        }

        [Authorize]
        public ActionResult TaDelweibo(int id)
        {
            var model = db.Messages.Where(m => m.messages_id == id).FirstOrDefault();
            db.Messages.Remove(model);
            db.SaveChanges();
            return Content("<script>alert('删除成功');window.location.href='/Tang/TaMicroblog';</script>");
        }

        [Authorize]
        public ActionResult ShouCang(int id)
        {
            int userid = Convert.ToInt32(User.Identity.Name);

            Collections model = new Collections();
            model.collections_time = DateTime.Now;
            model.messages_id = id;
            model.user_id = userid;
            db.Collections.Add(model);
            db.SaveChanges();

            //收藏次数加一
            var weibo = db.Messages.Where(m => m.messages_id == id).FirstOrDefault();
            weibo.messages_collectnum += 1;
            db.Entry(weibo).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Content("<script>alert('收藏成功');window.location.href='/Tang/MyHomePage';</script>");
        }
        public ActionResult MyShouCang(int id)
        {
            int userid = Convert.ToInt32(User.Identity.Name);

            Collections model = new Collections();
            model.collections_time = DateTime.Now;
            model.messages_id = id;
            model.user_id = userid;
            db.Collections.Add(model);
            db.SaveChanges();
            
            //收藏次数加一
            var weibo = db.Messages.Where(m => m.messages_id == id).FirstOrDefault();
            weibo.messages_collectnum += 1;
            db.Entry(weibo).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Content("<script>alert('收藏成功');window.location.href='/Tang/MyMicroblog';</script>");
        }

        [Authorize]
        public ActionResult TaShouCang(int id)
        {
            int userid = Convert.ToInt32(User.Identity.Name);

            Collections model = new Collections();
            model.collections_time = DateTime.Now;
            model.messages_id = id;
            model.user_id = userid;
            db.Collections.Add(model);
            db.SaveChanges();

            //收藏次数加一
            var weibo = db.Messages.Where(m => m.messages_id == id).FirstOrDefault();
            weibo.messages_collectnum += 1;
            db.Entry(weibo).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Content("<script>alert('收藏成功');window.location.href='/Tang/TaMicroblog';</script>");
        }

        [Authorize]
        public ActionResult AddPinglun(int messagesid, string pinglunwenben)
        {
            int id = Convert.ToInt32(User.Identity.Name);
            Comments model = new Comments();
            model.comments_info = pinglunwenben;
            model.comments_time = DateTime.Now;
            model.user_id = id;
            model.messages_id = messagesid;
            db.Comments.Add(model);
            db.SaveChanges();

            //评论次数加一
            var weibo = db.Messages.Where(m => m.messages_id == messagesid).FirstOrDefault();
            weibo.messages_commentnum += 1;
            db.Entry(weibo).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Content("<script>alert('评论成功');window.location.href='/Tang/MyHomePage';</script>");
        }

        [Authorize]
        public ActionResult MyAddPinglun(int messagesid, string pinglunwenben)
        {
            int id = Convert.ToInt32(User.Identity.Name);
            Comments model = new Comments();
            model.comments_info = pinglunwenben;
            model.comments_time = DateTime.Now;
            model.user_id = id;
            model.messages_id = messagesid;
            db.Comments.Add(model);
            db.SaveChanges();

            //评论次数加一
            var weibo = db.Messages.Where(m => m.messages_id == messagesid).FirstOrDefault();
            weibo.messages_commentnum += 1;
            db.Entry(weibo).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Content("<script>alert('评论成功');window.location.href='/Tang/MyMicroblog';</script>");
        }

        [Authorize]
        public ActionResult TaAddPinglun(int messagesid, string pinglunwenben)
        {
            int tid = db.Messages.Where(m => m.messages_id == messagesid).First().user_id;
            int id = Convert.ToInt32(User.Identity.Name);
            Comments model = new Comments();
            model.comments_info = pinglunwenben;
            model.comments_time = DateTime.Now;
            model.user_id = id;
            model.messages_id = messagesid;
            db.Comments.Add(model);
            db.SaveChanges();

            //评论次数加一
            var weibo = db.Messages.Where(m => m.messages_id == messagesid).FirstOrDefault();
            weibo.messages_commentnum += 1;
            db.Entry(weibo).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Content("<script>alert('评论成功');window.location.href='/Tang/TaMicroblog/"+tid+"';</script>");
        }

        [Authorize]
        public ActionResult AddZhuanfa(string zhuanfawenben,int messagesid)
        {
            Transpond model = new Transpond();
            model.transpond_info = zhuanfawenben;
            model.transpond_betime = DateTime.Now;
            model.messages_id = messagesid;
            int id = Convert.ToInt32(User.Identity.Name);
            model.transpond_users_id = id;
            db.Transpond.Add(model);
            db.SaveChanges();

            //转发次数加一
            var weibo = db.Messages.Where(m => m.messages_id == messagesid).FirstOrDefault();
            weibo.messages_transpondnum += 1;
            db.Entry(weibo).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Content("<script>alert('转发成功');window.location.href='/Tang/MyMicroblog';</script>");
        }

        [Authorize]
        public ActionResult AddGuanzhu(int id)
        {
            Relation model = new Relation();
            model.relation_time = DateTime.Now;
            int userid = Convert.ToInt32(User.Identity.Name);
            model.user_id = userid;
            model.user_guanzhu = id;
            db.Relation.Add(model);
            db.SaveChanges();
            return Content("<script>alert('关注成功');window.location.href='/Tang/TaMicroblog/"+id+"';</script>");
        }

        [Authorize]
        public ActionResult DelGuanzhu(int id)
        {
            int userid = Convert.ToInt32(User.Identity.Name);
            Relation model = db.Relation.Where(r => r.user_id == userid && r.user_guanzhu == id).FirstOrDefault();
            db.Relation.Remove(model);
            db.SaveChanges();
            return Content("<script>alert('取消关注成功');window.location.href='/Tang/TaMicroblog/" + id + "';</script>");
        }

        [Authorize]
        public ActionResult AddGuanzhuList(int id)
        {
            Relation model = new Relation();
            model.relation_time = DateTime.Now;
            model.user_id = Convert.ToInt32(User.Identity.Name);
            model.user_guanzhu = id;
            db.Relation.Add(model);
            db.SaveChanges();

            return Content("<script>alert('关注成功');window.location.href='/Tang/GuanzhuList/" + id + "';</script>");
        }

        [Authorize]
        public ActionResult DelGuanzhuList(int id)
        {
            int userid = Convert.ToInt32(User.Identity.Name);
            Relation model = db.Relation.Where(r => r.user_id == userid && r.user_guanzhu == id).FirstOrDefault();
            db.Relation.Remove(model);
            db.SaveChanges();
            return Content("<script>alert('取消关注成功');window.location.href='/Tang/GuanzhuList/" + id + "';</script>");
        }

        [Authorize]
        public ActionResult AddFansiList(int id)
        {
            Relation model = new Relation();
            model.relation_time = DateTime.Now;
            model.user_id = Convert.ToInt32(User.Identity.Name);
            model.user_guanzhu = id;
            db.Relation.Add(model);
            db.SaveChanges();
            return Content("<script>alert('关注成功');window.location.href='/Tang/FansiList/" + id + "';</script>");
        }

        [Authorize]
        public ActionResult DelFansiList(int id)
        {
            int userid = Convert.ToInt32(User.Identity.Name);
            Relation model = db.Relation.Where(r => r.user_id == userid && r.user_guanzhu == id).FirstOrDefault();
            db.Relation.Remove(model);
            db.SaveChanges();
            return Content("<script>alert('取消关注成功');window.location.href='/Tang/FansiList/" + id + "';</script>");
        }


    }
}