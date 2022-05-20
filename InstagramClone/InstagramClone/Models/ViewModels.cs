using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstagramClone.Models
{
    public partial class imageVM
    {
        public string imagepath { get; set; }
        public HttpPostedFileBase imageFile { get; set; }
    }
    public class commentVM
    {
        public string Co { get; set; }
        public int CID { get; set; }
        public int UID { get; set; }
    }
    public class likeVM
    {
        public int userid { get; set; }
        public int image_id { get; set; }
    }
    public class LoginVM
    {
        public string email { get; set; }
        public string Passwords { get; set; }
    }
    public class RegisterVM
    {
        public string email { get; set; }
        public string Passwords { get; set; }
        public int? phone { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
    }
}