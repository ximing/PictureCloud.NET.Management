using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OurEDA.DianShangYun.Management.Comment.Upload
{
    public class Client
    {
        public virtual void SetAuth(HttpWebRequest request, Stream body)
        {
        }

        public CallRet Call(string url)
        {
            Console.WriteLine("Client.Post ==> URL: " + url);
            CallRet result;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                this.SetAuth(request, null);
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
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

        public static CallRet HandleResult(HttpWebResponse response)
        {
            HttpStatusCode statusCode = response.StatusCode;
            CallRet result;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string responseStr = reader.ReadToEnd();
                result = new CallRet(statusCode, responseStr);
            }
            return result;
        }
    }
}
