using InstagramClone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InstagramClone.Controllers
{
    public class FriendsController : Controller
    {
        DatabaseEntities db = new DatabaseEntities();
        public ActionResult sendFriendRequest(int User_Id)
        {
            Friendship f = db.Friendships.Where(n => n.User_Id == User_Id).FirstOrDefault();
            string myId = Session["id"] + "$";
            if (f.Friend_Requests != null)
            {
                //checks if already sent a friend request
                if (f.Friend_Requests.Contains(myId))
                {
                    ViewBag.error = "You already sent a follow request";
                }
                //adds friend reqeust if not sent
                else
                {
                    f.Friend_Requests += Session["id"] + "$";
                    db.SaveChanges();
                }
            }
            else
            {
                //first friend request in the table
                f.Friend_Requests += Session["id"] + "$";
                db.SaveChanges();
            }
            return RedirectToAction("MyProfile", "Account");

        }

        public ActionResult acceptFriendRequest(int User_Id)
        {
            int id = (int)Session["id"];
            Friendship f = db.Friendships.Where(n => n.User_Id == id).FirstOrDefault();
            f.Friend_Requests = f.Friend_Requests.Replace(User_Id + "$", "");
            f.Friends += User_Id + "$";
            f = db.Friendships.Where(n => n.User_Id == User_Id).FirstOrDefault();
            f.Friends += id + "$";
            db.SaveChanges();
            return RedirectToAction("MyProfile", "Account");
        }

        public ActionResult rejectFriendRequest(int User_Id)
        {
            int id = (int)Session["id"];
            Friendship f = db.Friendships.Where(n => n.User_Id == id).FirstOrDefault();
            f.Friend_Requests = f.Friend_Requests.Replace(User_Id + "$", "");
            db.SaveChanges();
            return RedirectToAction("MyProfile", "Account");
        }

        public ActionResult removeFriend(int User_Id)
        {
            int id = (int)Session["id"];
            Friendship f = db.Friendships.Where(n => n.User_Id == id).FirstOrDefault();
            f.Friends = f.Friends.Replace(User_Id + "$", "");

            f = db.Friendships.Where(n => n.User_Id == User_Id).FirstOrDefault();
            f.Friends = f.Friends.Replace(id + "$", "");
            db.SaveChanges();

            return RedirectToAction("MyProfile", "Account");
        }

        public ActionResult viewFriends()
        {
            int id = (int)Session["id"];
            List<User> Friends = new List<User>();
            Friendship f = db.Friendships.Where(n => n.User_Id == id).FirstOrDefault();
            if (f.Friends == null)
            {
                ViewBag.error = "You are alone";
                return View();
            }
            string[] Friends_Ids = f.Friends.Split('$');
            for (int i = 0; i < Friends_Ids.Length; i++)
            {
                if (Friends_Ids[i] != "")
                {
                    int Friend_Id = Convert.ToInt32(Friends_Ids[i]);
                    User u = db.Users.Where(n => n.id == Friend_Id).FirstOrDefault();
                    Friends.Add(u);
                }

            }
            return View(Friends);
        }

        public ActionResult viewFriendRequests()
        {
            int id = (int)Session["id"];
            List<User> Friends = new List<User>();
            Friendship f = db.Friendships.Where(n => n.User_Id == id).FirstOrDefault();
            if (f.Friend_Requests == null)
            {
                ViewBag.error = "You are unwanted";
                return View();
            }
            string[] Friends_Ids = f.Friend_Requests.Split('$');
            for (int i = 0; i < Friends_Ids.Length; i++)
            {
                if (Friends_Ids[i] != "")
                {
                    int Friend_Id = Convert.ToInt32(Friends_Ids[i]);
                    User u = db.Users.Where(n => n.id == Friend_Id).FirstOrDefault();
                    Friends.Add(u);
                }

            }
            return View(Friends);
        }


    }
}