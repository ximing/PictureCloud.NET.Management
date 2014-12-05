using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurEDA.DianShangYun.Management.Comment.Upload
{
    public class PutExtra
    {
        public PutExtra(string MimeType)
        {
            this.MimeType = MimeType;
        }
        public Dictionary<string, string> Params
        {
            get;
            set;
        }
        public string MimeType
        {
            get;
            set;
        }
    }
}
