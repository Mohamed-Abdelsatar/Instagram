using InstagramClone.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using InstagramClone.viewModels;

namespace InstagramClone.Controllers
{
    public class ImagesController : Controller
    {
        DatabaseEntities db = new DatabaseEntities();

        // GET: Images
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult camera()
        {
            int UserId = Convert.ToInt32(Session["id"]);
            if (UserId == 0)
            {
                RedirectToAction("login", "Account");
            }
            return View();
        }

        [HttpGet]
        public ActionResult addus(int id)
        {
            var c = db.images.Include(x => x.comments).Include(a => a.User).Include(n => n.user_image_like).Include(n => n.user_image_dilike).Where(m => m.userid == id);
            ViewBag.d = id;
            return PartialView(c);
        }
        public ActionResult addimages()
        {
            return View();
        }
        [HttpPost]
        public ActionResult addimages(HttpPostedFileBase img)
        {
            img.SaveAs(Server.MapPath("~/Images/" + img.FileName));
            int userid = (int)Session["id"];
            image im = new image();
            im.imagepath = "~/Images/" + img.FileName;
            im.imageFile = img;
            im.userid = userid;
            db.images.Add(im);
            db.SaveChanges();
            ModelState.Clear();
            return View();
        }



        [HttpPost]
        public ActionResult like(likeVM v)
        {

            user_image_like u = new user_image_like();
            u.userid = v.userid;
            u.image_id = v.image_id;
            db.user_image_like.Add(u);
            db.SaveChanges();
            ModelState.Clear();
            if ((bool)Session["isOutside"])
            {
                return RedirectToAction("UserProfile", "Account", new { Id = (int)Session["OutsideUserId"] });
            }
            else
            {
                return RedirectToAction("MyProfile", "Account");
            }

        }
        [HttpPost]
        public ActionResult Dislike(likeVM v)
        {

            user_image_dilike u = new user_image_dilike();
            u.userid = v.userid;
            u.image_id = v.image_id;
            db.user_image_dilike.Add(u);
            db.SaveChanges();
            ModelState.Clear();
            if ((bool)Session["isOutside"])
            {
                return RedirectToAction("UserProfile", "Account", new { Id = (int)Session["OutsideUserId"] });
            }
            else
            {
                return RedirectToAction("MyProfile", "Account");
            }

        }
        [HttpPost]
        public ActionResult PN(viewModels.commentVM v)
        {

            comment c = new comment();

            c.comment1 = v.Co;
            c.image_id = v.CID;
            c.userid = (int)Session["id"];
            db.comments.Add(c);
            db.SaveChanges();
            ModelState.Clear();
            if ((bool)Session["isOutside"])
            {
                return RedirectToAction("UserProfile", "Account", new { Id = (int)Session["OutsideUserId"] });
            }
            else
            {
                return RedirectToAction("MyProfile", "Account");
            }
        }




    }
}