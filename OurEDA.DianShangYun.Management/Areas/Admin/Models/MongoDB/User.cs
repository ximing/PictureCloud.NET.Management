using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OurEDA.DianShangYun.Management.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OurEDA.DianShangYun.Management.Areas.Admin.Models.MongoDB
{
    public class User
    {
        [BsonConstructor]
        public User()
        {
            CreateTime = DateTime.Now.DatetimeToUnixTime();
        }
        [BsonId]
        public virtual ObjectId ID { get; set; }
        public virtual long CreateTime { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string ImageUrl { get; set; }
        public long Size { get; set; }
    }
}