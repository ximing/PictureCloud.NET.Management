using OurEDA.DianShangYun.Management.Areas.Admin.Models.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OurEDA.DianShangYun.Management.Areas.Admin.Models.Buckt
{
    public class BucketManagementModel
    {
        public Bucket Bucket { get; set; }
        public List<Catalogue> Catalogues { get; set; }
    }
}