using cn.OurEDA.DianShangYunApi.Upload;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OurEDA.DianShangYun.Management.Comment.Upload
{
    public class IOClient
    {
        private static NameValueCollection getFormData(string filename, PutExtra extra)
        {
            NameValueCollection formData = new NameValueCollection();
            formData["FileName"] = filename;
            formData["MimeType"] = extra.MimeType;
            if (extra != null)
            {
                if (extra.Params != null)
                {
                    foreach (KeyValuePair<string, string> pair in extra.Params)
                    {
                        formData[pair.Key] = pair.Value;
                    }
                }
            }
            return formData;
        }
        public PutRet Put(string filename, Stream putStream, PutExtra extra)
        {
            
            if (!putStream.CanRead)
            {
                throw new Exception("putStream error");
            }
            if (string.IsNullOrEmpty(filename))
            {
                throw new Exception("filename为空");
            }
            NameValueCollection formData = IOClient.getFormData(filename, extra);
            PutRet result;
            try
            {
                CallRet callRet = MultiPart.MultiPost(Config.UP_HOST, formData, putStream);
                PutRet ret = new PutRet(callRet);
                result = ret;
            }
            catch (Exception e)
            {
                PutRet ret = new PutRet(new CallRet(HttpStatusCode.BadRequest, e));
                result = ret;
            }
            return result;
        }

        public PutRet Put(string filename, HttpPostedFileBase file, PutExtra extra)
        {
            
            if (!file.InputStream.CanRead)
            {
                throw new Exception("putStream error");
            }
            if (string.IsNullOrEmpty(filename))
            {
                throw new Exception("filename为空");
            }
            var ext = file.FileName.Substring(file.FileName.LastIndexOf('.') + 1).ToLower();
            NameValueCollection formData = IOClient.getFormData(string.Format("{0}.{1}", filename, ext), extra);
            PutRet result;
            try
            {
                CallRet callRet = MultiPart.MultiPost(Config.UP_HOST, formData, file.InputStream);
                PutRet ret = new PutRet(callRet);
                result = ret;
            }
            catch (Exception e)
            {
                PutRet ret = new PutRet(new CallRet(HttpStatusCode.BadRequest, e));
                result = ret;
            }
            return result;
        }

        public PutRet PutImageWithThumb(string filename, HttpPostedFileBase file, 
            PutExtra extra,ThumbnailSize thumbnailSize)
        {
            if (!file.InputStream.CanRead)
            {
                throw new Exception("putStream error");
            }
            if (string.IsNullOrEmpty(filename))
            {
                throw new Exception("filename为空");
            }
            var ext = file.FileName.Substring(file.FileName.LastIndexOf('.') + 1).ToLower();
            NameValueCollection formData = IOClient.getFormData(string.Format("{0}.{1}", filename, ext), extra);
            formData.Add("Quality", thumbnailSize.Quality.ToString());
            formData.Add("Mode", (thumbnailSize.Mode).ToString());
            formData.Add("thumbheight", thumbnailSize.Height.ToString());
            formData.Add("thumbwidth", thumbnailSize.Width.ToString());
            formData.Add("makethumb", "yes");
            PutRet result;
            try
            {
                CallRet callRet = MultiPart.MultiPost(Config.UP_HOST, formData, file.InputStream);
                PutRet ret = new PutRet(callRet);
                result = ret;
            }
            catch (Exception e)
            {
                PutRet ret = new PutRet(new CallRet(HttpStatusCode.BadRequest, e));
                result = ret;
            }
            return result;
        }

    }
}
