using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OurEDA.DianShangYun.Management.Comment.Upload
{
    public class CallRet : EventArgs
    {
        public HttpStatusCode StatusCode
        {
            get;
            protected set;
        }
        public Exception Exception
        {
            get;
            protected set;
        }
        public string Response
        {
            get;
            protected set;
        }
        public CallRet(HttpStatusCode statusCode, string response)
        {
            this.StatusCode = statusCode;
            this.Response = response;
        }
        public CallRet(HttpStatusCode statusCode, Exception e)
        {
            this.StatusCode = statusCode;
            this.Exception = e;
        }
        public CallRet(CallRet ret)
        {
            this.StatusCode = ret.StatusCode;
            this.Exception = ret.Exception;
            this.Response = ret.Response;
        }
    }
}
