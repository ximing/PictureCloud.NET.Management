using OurEDA.DEV.Web.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OurEDA.DEV.Web.Comment
{
    public class MyCookieContext : CookieContext
    {
        public static MyCookieContext Current
        {
            get
            {
                return new MyCookieContext();
            }
        }

        public override string KeyPrefix
        {
            get
            {
                return "DianShangYun" + "_MyContext_";
            }
        }
    }
}