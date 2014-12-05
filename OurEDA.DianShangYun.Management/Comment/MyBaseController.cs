using OurEDA.DEV.Web.Extension;
using OurEDA.DianShangYun.Management.Areas.Admin.Models.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Yeanzhi.System;
using Yeanzhi.System.Account;

namespace OurEDA.DEV.Web.Comment
{
    public class MyBaseController : Controller
    {

        public MyBaseController()
        {
        }
        public User MyUser
        {
            get
            {
                return MyUserContext.Current.MyUser;
            }
        }
        public MyCookieContext CookieContext
        {
            get
            {
                return MyCookieContext.Current;
            }
        }

        public MyUserContext UserContext
        {
            get
            {
                return MyUserContext.Current;
            }
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var noAuthorizeAttributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AuthorizeIgnoreAttribute), false);
            if (noAuthorizeAttributes.Length > 0)
                return;
            if (MyUser == null)
            {
                filterContext.Result = RedirectToAction("Login", "Auth", new { area="Admin"});
            }
            base.OnActionExecuting(filterContext);
        }
    }
}