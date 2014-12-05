using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurEDA.DEV.Web.Extension
{
    public interface IAuthCookie
    {
        int UserExpiresHours { get; set; }
        string UserName { get; set; }
        string UserToken { get; set; }
        string UserEmail { get; set; }
        string UserImage { get; set; }
    }
}
