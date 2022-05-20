using InstagramClone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InstagramClone.Controllers
{
    public class AccountController : Controller
    {
        DatabaseEntities db = new DatabaseEntities();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult HomePage()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterVM RegisterDetails)
        {
            bool EmailExist = db.Users.Any(x => x.email == RegisterDetails.email);
            if (EmailExist)
            {
                ViewBag.EmailMessage = "This Email is already in use";
                return View();
            }
            bool PhoneExist = db.Users.Any(x => x.phone == RegisterDetails.phone);
            if (PhoneExist)
            {
                ViewBag.PhoneMessage = "This Phone Number is already in use";
                return View();
            }
            if (ModelState.IsValid)
            {
                User U = new User();
                U.First_Name = RegisterDetails.First_Name;
                U.Last_Name = RegisterDetails.Last_Name;
                U.email = RegisterDetails.email;
                U.phone = RegisterDetails.phone;
                U.Passwords = RegisterDetails.Passwords;
                db.Users.Add(U);
                db.SaveChanges();
                Friendship f = new Friendship();
                f.User_Id = U.id;
                db.Friendships.Add(f);
                db.SaveChanges();
                SaveUserSession(U);
                return RedirectToAction("Login");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginVM LoginDetails)
        {
            User u = db.Users.Where(n => n.email == LoginDetails.email && n.Passwords == LoginDetails.Passwords).FirstOrDefault();
            SaveUserSession(u);
            /*LoginVM LoginDetails*/
            bool UserExist = db.Users.Any(x => x.email == LoginDetails.email && x.Passwords == LoginDetails.Passwords);
            if (UserExist)
            {
                Session["UserID"] = db.Users.Single(x => x.email == LoginDetails.email).id;

                return RedirectToAction("MyProfile", "Account");
            }
            ViewBag.Message = "Wrong Email OR Password";
            return View();


        }
        public ActionResult LogOut()
        {
            Session["UserID"] = 0;
            return RedirectToAction("Login", "Account");
        }
        public ActionResult MyProfile()
        {
            Session["isOutside"] = false; // for liking

            int UserId = Convert.ToInt32(Session["UserID"]);
            if (UserId == 0)
            {
                RedirectToAction("login", "Account");
            }

            return View(db.Users.Find(UserId));
        }
        public ActionResult Edit()
        {
            int id = (int)Session["id"];
            User R = db.Users.Where(c => c.id == id).FirstOrDefault();
            return View(R);
        }


        [HttpPost]
        public ActionResult Edit(User u, HttpPostedFileBase imageFile)
        {
            
            int id = (int)Session["id"];
            User R = db.Users.Where(c => c.id == id).FirstOrDefault();
            R.First_Name = u.First_Name;
            R.Last_Name = u.Last_Name;
            R.Passwords = u.Passwords;
            R.email = u.email;
            R.bio = u.bio;
            if (imageFile != null)
            {
                imageFile.SaveAs(Server.MapPath("~/Images/ProfilePics/" + imageFile.FileName));
                R.profileimage = "~/Images/ProfilePics/" + imageFile.FileName;
            }
            SaveUserSession(R);
            db.SaveChanges();
            return View(R);
        }
        public ActionResult Search()
        {
            return View();
        }
        public ActionResult ListUsers(string keyword)
        {
            int UserId = Convert.ToInt32(Session["id"]);
            List<User> users = db.Users.Where(n => n.First_Name.Contains(keyword) || n.Last_Name.Contains(keyword) || n.email.Contains(keyword) || (n.First_Name + n.Last_Name).Contains(keyword)).ToList();
            if (users.Exists(r => r.id == UserId))
            {
                var itemToRemove = users.Single(r => r.id == UserId);
                users.Remove(itemToRemove);
            }

            if (users.Count <= 0) ViewBag.error = "User not found";
            return PartialView(users);
        }
        public ActionResult UserProfile(int id)
        {
            Session["isOutside"] = true;
            Session["OutsideUserId"] = id;

            int MyId = (int)Session["id"];
            Friendship f = db.Friendships.Where(n => n.User_Id == id).FirstOrDefault();
            if (f.Friends != null)
            {
                if (f.Friends.Contains(MyId + "$"))
                {
                    User u = db.Users.Where(n => n.id == id).FirstOrDefault();
                    ViewBag.viewable = true;
                    return View(u);
                }
            }
            ViewBag.viewable = false;
            return View();
        }


        //Helper Functions
        public void SaveUserSession(User u)
        {
            if (u != null)
            {
                Session["First_Name"] = u.First_Name;
                Session["Last_Name"] = u.Last_Name;
                Session["email"] = u.email;
                Session["phone"] = u.phone;
                Session["id"] = u.id;
                Session["Passwords"] = u.Passwords;
                Session["bio"] = u.bio;
                Session["profileimage"] = u.profileimage;
            }
        }
        public User getUserSession()
        {
            User u = new User();
            u.id = (int)Session["id"];
            u.email = Session["email"].ToString();
            u.Passwords = Session["Passwords"].ToString();
            u.phone = (int)Session["phone"];
            u.First_Name = Session["First_Name"].ToString();
            u.Last_Name = Session["Last_Name"].ToString();
            u.bio = Session["bio"].ToString();
            u.profileimage = Session["profileimage"].ToString();
       
            return u;
        }
    }
}