using OurEDA.DEV.Web.Comment;
using OurEDA.DEV.Web.Extension;
using OurEDA.DianShangYun.Management.Areas.Admin.Models;
using OurEDA.DianShangYun.Management.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Yeanzhi.System.Account;

namespace OurEDA.DianShangYun.Management.Areas.Admin.Controllers
{
    public class AuthController : MyBaseController
    {
        [AuthorizeIgnore]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AuthorizeIgnore]
        public ActionResult Login(LoginModel loginModel)
        {
            var v = Request;
            AuthService auth = new AuthService();
            var user = auth.GetUserByEmaiAndPass(loginModel.email, loginModel.password);

            if (user==null)
            {
                //ModelState.AddModelError("error", ret.Msg);
                ViewBag.error = "email或password错误";
                return View(loginModel);
            }
            else
            {
                this.CookieContext.UserName = user.Name;
                this.CookieContext.UserEmail = user.Email;
                this.CookieContext.UserImage = user.ImageUrl;
                this.CookieContext.UserToken = user.ID.ToString();
                return RedirectToAction("Index", "User", new { area="Admin"});
            }
        }


        [AuthorizeIgnore]
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [AuthorizeIgnore]
        public ActionResult Register(string email,string password,string name)
        {
            AuthService auth = new AuthService();
            var user = new OurEDA.DianShangYun.Management.Areas.Admin.Models.MongoDB.User
            {
                Email = email,
                ImageUrl = "http://download.oureda.net/group1/M00/00/00/rAYhOVR--Y-AMZIaAAAr70pXJeQ934.png",
                Size = (long)10 * 1024 * 1024 * 1024 * 1024,
                Name = name,
                Password = password
            };
            var v = auth.InsertUser(user);
            if (v.Ok)
            {
                return RedirectToAction("Index", "User", new { Area="Admin"});
            }
            ViewBag.error = v.Msg;
            return View();
        }

        public ActionResult Logout()
        {
            this.UserContext.UserRemove();
            this.CookieContext.UserName = string.Empty;
            this.CookieContext.UserToken = string.Empty;
            return RedirectToAction("Login");
        }
    }
}