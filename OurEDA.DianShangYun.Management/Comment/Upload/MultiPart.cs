using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OurEDA.DianShangYun.Management.Comment.Upload
{
    public static class MultiPart
    {
        public static string RandomBoundary()
        {
            return string.Format("----------{0:N}", Guid.NewGuid());
        }
        public static string FormDataContentType(string boundary)
        {
            return "multipart/form-data; boundary=" + boundary;
        }
        public static Stream GetPostStream(Stream putStream, string fileName, NameValueCollection formData, string boundary)
        {
            Stream postDataStream = new MemoryStream();
            string formDataHeaderTemplate = string.Concat(new string[]
			{
				Environment.NewLine,
				"--",
				boundary,
				Environment.NewLine,
				"Content-Disposition: form-data; name=\"{0}\";",
				Environment.NewLine,
				Environment.NewLine,
				"{1}"
			});
            foreach (string key in formData.Keys)
            {
                byte[] formItemBytes = Encoding.UTF8.GetBytes(string.Format(formDataHeaderTemplate, key, formData[key]));
                postDataStream.Write(formItemBytes, 0, formItemBytes.Length);
            }
            string fileHeaderTemplate = string.Concat(new string[]
			{
				Environment.NewLine,
				"--",
				boundary,
				Environment.NewLine,
				"Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"",
				Environment.NewLine,
				"Content-Type: application/octet-stream",
				Environment.NewLine,
				Environment.NewLine
			});
            byte[] fileHeaderBytes = Encoding.UTF8.GetBytes(string.Format(fileHeaderTemplate, "file", fileName));
            postDataStream.Write(fileHeaderBytes, 0, fileHeaderBytes.Length);
            byte[] buffer = new byte[1024];
            int bytesRead;
            while ((bytesRead = putStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                postDataStream.Write(buffer, 0, bytesRead);
            }
            putStream.Close();
            byte[] endBoundaryBytes = Encoding.UTF8.GetBytes(string.Concat(new string[]
			{
				Environment.NewLine,
				"--",
				boundary,
				"--",
				Environment.NewLine
			}));
            postDataStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            return postDataStream;
        }
        public static Stream GetPostStream(string filePath, NameValueCollection formData, string boundary)
        {

            Stream postDataStream = new MemoryStream();
            string formDataHeaderTemplate = string.Concat(new string[]
			{
				Environment.NewLine,
				"--",
				boundary,
				Environment.NewLine,
				"Content-Disposition: form-data; name=\"{0}\";",
				Environment.NewLine,
				Environment.NewLine,
				"{1}"
			});
            foreach (string key in formData.Keys)
            {
                byte[] formItemBytes = Encoding.UTF8.GetBytes(string.Format(formDataHeaderTemplate, key, formData[key]));
                postDataStream.Write(formItemBytes, 0, formItemBytes.Length);
            }
            FileInfo fileInfo = new FileInfo(filePath);
            string fileHeaderTemplate = string.Concat(new string[]
			{
				Environment.NewLine,
				"--",
				boundary,
				Environment.NewLine,
				"Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"",
				Environment.NewLine,
				"Content-Type: application/octet-stream",
				Environment.NewLine,
				Environment.NewLine
			});
            byte[] fileHeaderBytes = Encoding.UTF8.GetBytes(string.Format(fileHeaderTemplate, "file", fileInfo.FullName));
            postDataStream.Write(fileHeaderBytes, 0, fileHeaderBytes.Length);
            FileStream fileStream = fileInfo.OpenRead();
            byte[] buffer = new byte[1024];
            int bytesRead;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                postDataStream.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();
            byte[] endBoundaryBytes = Encoding.UTF8.GetBytes(string.Concat(new string[]
			{
				Environment.NewLine,
				"--",
				boundary,
				"--",
				Environment.NewLine
			}));
            postDataStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            return postDataStream;
        }
        public static CallRet MultiPost(string url, NameValueCollection formData, string fileName)
        {
            string boundary = MultiPart.RandomBoundary();
            WebRequest webRequest = WebRequest.Create(url);
            webRequest.Method = "POST";
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            FileInfo fileInfo = new FileInfo(fileName);
            using (FileStream fileStream = fileInfo.OpenRead())
            {
                Stream postDataStream = MultiPart.GetPostStream(fileStream, fileName, formData, boundary);
                webRequest.ContentLength = postDataStream.Length;
                Stream reqStream = webRequest.GetRequestStream();
                postDataStream.Position = 0L;
                byte[] buffer = new byte[1024];
                int bytesRead;
                while ((bytesRead = postDataStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    reqStream.Write(buffer, 0, bytesRead);
                }
                postDataStream.Close();
                reqStream.Close();
            }
            CallRet result;
            try
            {
                using (HttpWebResponse response = webRequest.GetResponse() as HttpWebResponse)
                {
                    result = Client.HandleResult(response);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                result = new CallRet(HttpStatusCode.BadRequest, e);
            }
            return result;
        }
        public static CallRet MultiPost(string url, NameValueCollection formData, Stream inputStream)
        {
            string boundary = MultiPart.RandomBoundary();
            WebRequest webRequest = WebRequest.Create(url);
            webRequest.Method = "POST";
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            Stream postDataStream = MultiPart.GetPostStream(inputStream, formData["FileName"], formData, boundary);
            webRequest.ContentLength = postDataStream.Length;
            Stream reqStream = webRequest.GetRequestStream();
            postDataStream.Position = 0L;
            byte[] buffer = new byte[1024];
            int bytesRead;
            while ((bytesRead = postDataStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                reqStream.Write(buffer, 0, bytesRead);
            }
            postDataStream.Close();
            reqStream.Close();
            CallRet result;
            try
            {
                using (HttpWebResponse response = webRequest.GetResponse() as HttpWebResponse)
                {
                    result = Client.HandleResult(response);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                result = new CallRet(HttpStatusCode.BadRequest, e);
            }
            return result;
        }
    }
}
