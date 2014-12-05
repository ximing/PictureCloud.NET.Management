using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Yeanzhi.Framework.Utility;

namespace OurEDA.DEV.Web.Extension
{
    public class CookieContext : IAuthCookie
    {

        public CookieContext()
        {
        }

        /// <summary>
        /// Cache或者Cookie的Key前缀
        /// </summary>
        public virtual string KeyPrefix
        {
            get
            {
                return "AdminContext_";
            }
        }

        public void Set(string key, string value, int expiresHours = 0)
        {
            if (expiresHours > 0)
                Cookie.Save(KeyPrefix + key, value, expiresHours);
            else
                Cookie.Save(KeyPrefix + key, value);
        }

        #region IAuthCookie
        private int userExpiresHours = 10;
        public virtual int UserExpiresHours
        {
            get
            {
                return userExpiresHours;
            }
            set
            {
                userExpiresHours = value;
            }
        }

        public virtual string UserName
        {
            get
            {
                return HttpUtility.UrlDecode(Cookie.GetValue(KeyPrefix + "UserName"));
            }
            set
            {
                Cookie.Save(KeyPrefix + "UserName", HttpUtility.UrlEncode(value), UserExpiresHours);
            }
        }

        #endregion


        public string UserToken
        {
            get
            {
                return (Cookie.GetValue(KeyPrefix + "UserToken"));
            }
            set
            {
                Cookie.Save(KeyPrefix + "UserToken", value.ToString(), UserExpiresHours);
            }
        }


        public string UserEmail
        {
            get
            {
                return HttpUtility.UrlDecode(Cookie.GetValue(KeyPrefix + "UserEmail"));
            }
            set
            {
                Cookie.Save(KeyPrefix + "UserEmail", HttpUtility.UrlEncode(value), UserExpiresHours);
            }
        }

        public string UserImage
        {
            get
            {
                return HttpUtility.UrlDecode(Cookie.GetValue(KeyPrefix + "UserImage"));
            }
            set
            {
                Cookie.Save(KeyPrefix + "UserImage", HttpUtility.UrlEncode(value), UserExpiresHours);
            }
        }
    }
}