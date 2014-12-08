using OurEDA.DEV.Web.Comment;
using OurEDA.DianShangYun.Management.Areas.Admin.Models.Buckt;
using OurEDA.DianShangYun.Management.Areas.Admin.Models.MongoDB;
using OurEDA.DianShangYun.Management.Comment;
using OurEDA.DianShangYun.Management.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OurEDA.DianShangYun.Management.Areas.Admin.Controllers
{
    public class BucketController : MyBaseController
    {
        protected BucketService bs;
        protected CatalogueService cs;
        public BucketController()
        {
            bs = new BucketService();
            cs = new CatalogueService();
        }
        public ActionResult Index()
        {
            var res = bs.GetBugects();
            return View(res);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(string name)
        {
            var md5 = this.MyUser.ID.ToString() + DateTime.Now.Ticks.ToString() + name;
            Bucket bucket = new Bucket
            {
                IsPublic = true,
                Name = name,
                SECRET_KEY = GetMD5(md5),
                Size = 0,
                UserId = this.MyUser.ID,
                DownloadCurCount = 0,
                DownloadCurSize = 0,
                DownloadTotalSize = 10L * 1024 * 1024 * 1024
            };
            var res = bs.InsertBucket(bucket);
            if (res.Ok)
            {
                return RedirectPermanent("Index");
            }
            else
            {
                ViewBag.error = res.Msg;
                return View();
            }
            
        }


        public ActionResult Management(string id)
        {
            var res = bs.FindBucketById(id);
            if (res==null)
            {
                return HttpNotFound();
            }
            var cat = cs.GetCataloguesByBucketId(res.ID);
            
            BucketManagementModel bmm = new BucketManagementModel()
            {
                Bucket = res,
                Catalogues = cat
            };
            return View(bmm);
        }

        public ActionResult Config(string id)
        {
            var res = bs.FindBucketById(id);
            if (res == null)
            {
                return HttpNotFound();
            }
            return View(res);
        }

        public string DeleteFile(string id)
        {
            var ACCESS_KEY = Request.Form["ACCESS_KEY"];
            var SECRET_KEY = Request.Form["SECRET_KEY"];
            var dparams = new Dictionary<string, string>();
            dparams.Add("FileId", id);
            dparams.Add("ACCESS_KEY", ACCESS_KEY);
            dparams.Add("SECRET_KEY", SECRET_KEY);
            dparams.Add("UserToken", this.MyUser.ID.ToString());
            var res = HttpWebResponseUtility.CreatePostHttpResponse("http://delete.oureda.net", dparams, null, null, Encoding.UTF8, null);
            StreamReader sr = new StreamReader(res.GetResponseStream());
            return sr.ReadToEnd();
        }

        public  string GetMD5(string sDataIn)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytValue, bytHash;
            bytValue = System.Text.Encoding.UTF8.GetBytes(sDataIn);
            bytHash = md5.ComputeHash(bytValue);
            md5.Clear();
            string sTemp = "";
            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp += bytHash[i].ToString("X").PadLeft(2, '0');
            }
            return sTemp.ToLower();
        }
    }
}