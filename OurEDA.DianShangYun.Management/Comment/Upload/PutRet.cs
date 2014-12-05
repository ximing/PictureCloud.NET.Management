using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurEDA.DianShangYun.Management.Comment.Upload
{
    public class PutRet : CallRet
    {
        public string Hash
        {
            get;
            private set;
        }
        public string key
        {
            get;
            private set;
        }
        public PutRet(CallRet ret)
            : base(ret)
        {
            if (!string.IsNullOrEmpty(base.Response))
            {
                try
                {
                    this.Unmarshal(base.Response);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    base.Exception = e;
                }
            }
        }
        private void Unmarshal(string json)
        {
            try
            {
                Dictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                object tmp;
                if (dict.TryGetValue("hash", out tmp))
                {
                    this.Hash = (string)tmp;
                }
                if (dict.TryGetValue("key", out tmp))
                {
                    this.key = (string)tmp;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
