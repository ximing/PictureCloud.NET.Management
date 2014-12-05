using cn.OurEDA.DianShangYunApi.Upload;
using Newtonsoft.Json;
using OurEDA.DianShangYun.Management.Comment.Upload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OurEDA.DianShangYun.Management.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToActionPermanent("Index", "User", new { Area = "Admin" });
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public string UploadBucketImage()
        {
            var postedfile = Request.InputStream;
            // 读取原始文件名
            var clientFile = Request.Files[0];
            var localFileName = string.IsNullOrEmpty(Request.Form["FileName"]) ? clientFile.FileName : Request.Form["FileName"];
            var ACCESS_KEY = Request.Form["ACCESS_KEY"];
            var SECRET_KEY = Request.Form["SECRET_KEY"];
            //Config.UP_HOST = "http://upload.oureda.net/UpFile";
            IOClient ioClient = new IOClient();
            PutExtra put = new PutExtra(clientFile.ContentType.ToString());
            put.Params = new Dictionary<string, string>();
            put.Params.Add("ACCESS_KEY", ACCESS_KEY);
            put.Params.Add("SECRET_KEY", SECRET_KEY);
            put.Params.Add("UserToken", OurEDA.DEV.Web.Comment.MyUserContext.Current.MyUser.ID.ToString());
            put.Params.Add("ContentType", clientFile.ContentType);
            put.Params.Add("ContentLength", clientFile.ContentLength.ToString());
            var v = ioClient.Put(localFileName, clientFile.InputStream, put);
            return v.Response;
        }
    }
}