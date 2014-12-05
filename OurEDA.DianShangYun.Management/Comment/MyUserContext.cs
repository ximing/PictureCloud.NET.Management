using OurEDA.DEV.Web.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OurEDA.DEV.Web.Comment
{
    public class MyUserContext : UserContext
    {
        public MyUserContext()
            : base(MyCookieContext.Current)
        {
        }

        public MyUserContext(IAuthCookie authCookie)
            : base(authCookie)
        {
        }

        public static MyUserContext Current
        {
            get
            {
                return new MyUserContext();
            }
        }
    }
}