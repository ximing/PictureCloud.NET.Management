using cn.OurEDA.DianShangYunApi.Upload;
using Newtonsoft.Json;
using OurEDA.DianShangYun.Management.Areas.Admin.Models.MongoDB;
using OurEDA.DianShangYun.Management.Comment.Upload;
using OurEDA.DianShangYun.Management.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OurEDA.DianShangYun.Management.Comment;

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
            var vres = JsonConvert.DeserializeObject<res>(v.Response);
            if (vres.state=="200")
            {
                var cs = new CatalogueService();
                var cal = cs.GetCatalogueByPath(vres.message);
                var resvat = new resCat(cal);
                resvat.time = cal.LastUpdateTime.UnixTimeToDatetime().ToShortDateString();
                resvat.size = cal.FileSize.Format_FileSize();
                return JsonConvert.SerializeObject(new resResponse { message = resvat, state = "200" });
            }
            return v.Response;
        }

        public class res
        {
            public string message { get; set; }
            public string state { get; set; }
        }

        public class resResponse
        {
            public resCat message { get; set; }
            public string state { get; set; }
        }

        public class resCat : Catalogue
        {
            public resCat(Catalogue c)
            {
                BucketId = c.BucketId;
                this.FileLocalName = c.FileLocalName;
                this.FilemimeType = c.FilemimeType;
                this.FileServerName = c.FileServerName;
                this.FileSize = c.FileSize;
                this.ID = c.ID;
                this.LastUpdateTime = c.LastUpdateTime;
            }
            public string time { get; set; }
            public string size { get; set; }
        }
    }
}