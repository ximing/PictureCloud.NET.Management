using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OurEDA.DianShangYun.Management.Areas.Admin.Models.MongoDB
{
    public class Catalogue
    {
        [BsonConstructor]
        public Catalogue()
        {
            LastUpdateTime = 0;
        }
        [BsonId]
        public virtual ObjectId ID { get; set; }
        public virtual long LastUpdateTime { get; set; }
        public ObjectId BucketId { get; set; }
        public string FileLocalName { get; set; }
        public string FileServerName { get; set; }
        public string FilemimeType { get; set; }
        public long FileSize { get; set; }
    }
}