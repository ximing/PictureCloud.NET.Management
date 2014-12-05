using OurEDA.DEV.Web.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OurEDA.DianShangYun.Management.Areas.Admin.Controllers
{
    public class AppsController : MyBaseController
    {
        // GET: Admin/Apps
        public ActionResult Index()
        {
            return View();
        }
    }
}