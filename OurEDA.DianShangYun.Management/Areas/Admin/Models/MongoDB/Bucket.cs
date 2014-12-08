using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OurEDA.DianShangYun.Management.Comment;

namespace OurEDA.DianShangYun.Management.Areas.Admin.Models.MongoDB
{
    public class Bucket
    {
        [BsonConstructor]
        public Bucket()
        {
            CreateTime = DateTime.Now.DatetimeToUnixTime();//new DateTime(DateTime.Now.Ticks, DateTimeKind.Utc);
        }
        [BsonId]
        public virtual ObjectId ID { get; set; }
        public virtual long CreateTime { get; set; }
        public ObjectId UserId { get; set; }
        public string Name { get; set; }
        public string SECRET_KEY { get; set; }
        public bool IsPublic { get; set; }
        public long Size { get; set; }
        public long DownloadCurCount { get; set; }
        public long DownloadCurSize { get; set; }
        public long DownloadTotalSize { get; set; }
    }
}