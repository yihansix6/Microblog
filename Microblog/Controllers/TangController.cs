using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Microblog.Models;

namespace Microblog.Controllers
{
    public class TangController : Controller
    {
        MicroblogDBEntities db = new MicroblogDBEntities();
        // GET: Tang
        public ActionResult MyHomePage()
        {
            //var shuju = db.Messages.OrderByDescending(m => m.messages_time).ToList();//需要修改：关注人的微博
            
            int id = 1;  //拟id

            List<Microblog.Models.Messages> shuju = new List<Messages>();
            List<int> guanzhulist = new List<int>();
            var wcount = db.Relation.Where(r => r.user_id == id && r.user_guanzhu != null).ToList();
            foreach (var item in wcount)
            {
                guanzhulist.Add(item.Users.user_id);
            }
            foreach (var item in guanzhulist)
            {
                shuju.Add(db.Messages.Where(m => m.user_id == item).FirstOrDefault());
            }
            var myweibo = db.Messages.Where(m => m.user_id == id).ToList();
            foreach (var item in myweibo)
            {
                shuju.Add(item);
            }
            shuju.OrderByDescending(s => s.messages_time);

            var xinxi = db.Users.FirstOrDefault(x => x.user_id == id);
            ViewData["uname"] = xinxi.user_name; //用户名
            ViewData["uaddress"] = xinxi.Userinfo.userinfo_address;  //用户地址
            ViewData["uheadphoto"] = xinxi.Userinfo.user_headphoto;

            var count = db.Messages.Where(m => m.user_id == id).ToList();
            ViewData["weibocount"] = count.Count;  //发微博的条数
            var fan = db.Relation.Where(r => r.user_id == id).ToList();

            int guanzhu = 0;
            int fansi = 0;
            ArrayList guanzhuid = new ArrayList();
            ArrayList fansiid = new ArrayList();
            foreach (var item in fan)
            {
                if (item.user_guanzhu != null)
                {
                    guanzhu++;
                    guanzhuid.Add(item.user_guanzhu);
                }
                if (item.user_byid != null)
                {
                    fansi++;
                    fansiid.Add(item.user_byid);
                }
            }
            List<Users> guanzhuHedaphoto = new List<Users>();
            List<Users> fansiHeadphoto = new List<Users>();
            var Alluser = db.Users.Select(u => u).ToList();
            foreach (var item in Alluser)
            {
                for (int i = 0; i < guanzhuid.Count; i++)
                {
                    if (item.user_id == (int)guanzhuid[i])
                    {
                        
                        guanzhuHedaphoto.Add(item);
                    }
                    if (guanzhuHedaphoto.Count >= 6)
                    {
                        break;
                    }
                }
                for (int i = 0; i < fansiid.Count; i++)
                {
                    if (item.user_id == (int)fansiid[i])
                    {
                        
                        fansiHeadphoto.Add(item);
                    }
                    if (fansiHeadphoto.Count >= 6)
                    {
                        break;
                    }
                }
                
            }
            ViewData["fansiHeadphoto"] = fansiHeadphoto;
            ViewData["guanzhuHedaphoto"] = guanzhuHedaphoto;

            ViewData["guanzhu"] = guanzhu;  //关注数量
            ViewData["fansi"] = fansi;  //粉丝数量


            return View(shuju);
        }

        public ActionResult MyMicroblog()
        {
            int id = 1; //拟id
            var shuju = db.Messages.Where(m => m.user_id == id).OrderByDescending(o => o.messages_time).ToList();
            var info = db.Users.Where(u => u.user_id == id).FirstOrDefault();
            ViewData["mname"] = info.user_name;
            ViewData["maddress"] = info.Userinfo.userinfo_address;
            ViewData["mHeadphoto"] = info.Userinfo.user_headphoto;



            var count = db.Messages.Where(m => m.user_id == id).ToList();
            ViewData["weibocount"] = count.Count;  //发微博的条数
            var fan = db.Relation.Where(r => r.user_id == id).ToList();
            int guanzhu = 0;
            int fansi = 0;
            ArrayList guanzhuid = new ArrayList();
            ArrayList fansiid = new ArrayList();
            foreach (var item in fan)
            {
                if (item.user_guanzhu != null)
                {
                    guanzhu++;
                    guanzhuid.Add(item.user_guanzhu);
                }
                if (item.user_byid != null)
                {
                    fansi++;
                    fansiid.Add(item.user_byid);
                }
            }
            List<Users> guanzhuHedaphoto = new List<Users>();
            List<Users> fansiHeadphoto = new List<Users>();
            var Alluser = db.Users.Select(u => u).ToList();
            foreach (var item in Alluser)
            {
                for (int i = 0; i < guanzhuid.Count; i++)
                {
                    if (item.user_id == (int)guanzhuid[i])
                    {

                        guanzhuHedaphoto.Add(item);
                    }
                    if (guanzhuHedaphoto.Count >= 6)
                    {
                        break;
                    }
                }
                for (int i = 0; i < fansiid.Count; i++)
                {
                    if (item.user_id == (int)fansiid[i])
                    {

                        fansiHeadphoto.Add(item);
                    }
                    if (fansiHeadphoto.Count >= 6)
                    {
                        break;
                    }
                }

            }
            ViewData["fansiHeadphoto"] = fansiHeadphoto;
            ViewData["guanzhuHedaphoto"] = guanzhuHedaphoto;

            ViewData["guanzhu"] = guanzhu;  //关注数量
            ViewData["fansi"] = fansi;  //粉丝数量



            return View(shuju);
        }

        public ActionResult ProcessImage(HttpPostedFileBase img)
        {

            string fileName = img.FileName;
            string path = "/resource/" + fileName;
            img.SaveAs(Server.MapPath(path));
            return Content(path);
        }

        [ValidateInput(false)]
        public ActionResult Addweibo(string Text)
        {
            Messages model = new Messages();
            model.user_id = 1;//拟id
            //int textinfonum = Text.IndexOf("img") + 3;
            //Text = Text.Insert(textinfonum, " class='weiboimg' ");
            model.messages_info = Text;
            model.messages_time = DateTime.Now;
            model.messages_collectnum = 0;
            model.messages_commentnum = 0;
            model.messages_transpondnum = 0;
            model.messages_key = "key" + DateTime.Now.ToString("yyyyMMddHHmmss");
            db.Messages.Add(model);
            db.SaveChanges();
            return Content("发布成功！");
        }

        public ActionResult Delweibo(int id)
        {
            var model = db.Messages.Where(m => m.messages_id == id).FirstOrDefault();
            db.Messages.Remove(model);
            db.SaveChanges();
            return RedirectToAction("MyHomePage");
        }
        public ActionResult MyDelweibo(int id)
        {
            var model = db.Messages.Where(m => m.messages_id == id).FirstOrDefault();
            db.Messages.Remove(model);
            db.SaveChanges();
            return RedirectToAction("MyMicroblog");
        }

        public ActionResult ShouCang(int id)
        {
            Collections model = new Collections();
            model.collections_time = DateTime.Now;
            model.user_id = 1;//拟id
            model.messages_id = id;
            db.Collections.Add(model);
            db.SaveChanges();
            return RedirectToAction("MyHomePage");;
        }
        public ActionResult MyShouCang(int id)
        {
            Collections model = new Collections();
            model.collections_time = DateTime.Now;
            model.user_id = 1;//拟id
            model.messages_id = id;
            db.Collections.Add(model);
            db.SaveChanges();

            //收藏次数加一
            var weibo = db.Messages.Where(m => m.messages_id == id).FirstOrDefault();
            weibo.messages_collectnum += 1;
            db.Entry(weibo).State = System.Data.Entity.EntityState.Modified;
            return RedirectToAction("MyMicroblog"); ;
        }

        public ActionResult AddPinglun(int messagesid, string pinglunwenben)
        {
            int id = 1; //拟id
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
            return RedirectToAction("MyHomePage");
        }

        public ActionResult AddZhuanfa(string zhuanfawenben,int messagesid)
        {
            Transpond model = new Transpond();
            model.transpond_info = zhuanfawenben;
            model.transpond_betime = DateTime.Now;
            model.messages_id = messagesid;
            int id = 1; //拟id
            model.transpond_users_id = id;
            db.Transpond.Add(model);
            db.SaveChanges();
            return RedirectToAction("MyMicroblog");
        }
    }
}