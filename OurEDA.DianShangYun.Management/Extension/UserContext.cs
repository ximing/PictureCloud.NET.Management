using Newtonsoft.Json;
using OurEDA.DianShangYun.Management.Areas.Admin.Models.MongoDB;
using OurEDA.DianShangYun.Management.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Yeanzhi.Framework.Utility;
using Yeanzhi.System;
using Yeanzhi.System.Account;

namespace OurEDA.DEV.Web.Extension
{
    public class UserContext
    {
        protected IAuthCookie authCookie;

        public UserContext(IAuthCookie authCookie)
        {
            this.authCookie = authCookie;
        }

        public virtual string KeyPrefix
        {
            get
            {
                return "MyUser_";
            }
        }

        public User MyUser
        {
            get
            {
                if (String.IsNullOrEmpty(authCookie.UserToken))
                    return null;
                if (Caching.Get(string.Format("{0}{1}", KeyPrefix, authCookie.UserToken)) == null)
                {
                    var funcres = new Func<User>(() =>
                    {
                        AuthService auth = new AuthService();
                        var user = auth.GetUserByTokenAndEmai(authCookie.UserEmail, authCookie.UserToken);
                        if (user==null)
                        {
                            throw new Exception("非法操作，试图通过网站修改Cookie取得用户信息！");
                        }
                        else
                        {
                            return user;
                        }
                    });
                    Caching.Set(string.Format("{0}{1}", KeyPrefix, authCookie.UserToken), funcres());
                }
                return (User)Caching.Get(string.Format("{0}{1}", KeyPrefix, authCookie.UserToken));
            }
        }

        public void UserRemove()
        {
            Caching.Remove(string.Format("{0}{1}", KeyPrefix, authCookie.UserToken));
        }
    }
}