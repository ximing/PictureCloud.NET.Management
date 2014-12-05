using MongoDB.Driver;
using OurEDA.DianShangYun.Management.Areas.Admin.Models;
using OurEDA.DianShangYun.Management.Areas.Admin.Models.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver.Linq;
using OurEDA.DEV.Web.Comment;
using MongoDB.Bson;

namespace OurEDA.DianShangYun.Management.Service
{
    public class BucketService
    {
        protected readonly MongoClient _instance;
        protected readonly MongoServer _server;
        private readonly MongoDatabase dbContext;
        private readonly MongoCollection collection;
        private readonly MongoCollection usercollection;
        public BucketService()
        {
            var credential = MongoCredential.CreateMongoCRCredential("PictureCloud", "YOUR_MONGO_USER", "YOUR_MONGO_PASSWORD");

            var settings = new MongoClientSettings
            {
                Server = new MongoServerAddress("mongo.oureda.net", 27017),
                Credentials = new[] { credential }
            };

            var mongoClient = new MongoClient(settings);
            //_instance = new MongoClient("mongodb://YOUR_MONGO_USER:YOUR_MONGO_PASSWORD@mongo.oureda.net");
            _server = mongoClient.GetServer();
            dbContext = _server.GetDatabase("PictureCloud");
            collection = dbContext.GetCollection<Bucket>("Bucket");
            usercollection = dbContext.GetCollection<User>("User");
        }

        public List<Bucket> GetBugects()
        {
            return collection.AsQueryable<Bucket>().Where(a => a.UserId == MyUserContext.Current.MyUser.ID).ToList();
        }
        public ReturnModelService InsertBucket(Bucket bucket)
        {
            ReturnModelService rms = new ReturnModelService() { Ok = false };
            if (this.FindBucketByName(bucket.Name))
            {
                rms.Msg = "Name重复";
                return rms;
            }
            var res = collection.Insert<Bucket>(bucket);
            if (res.Ok)
            {
                rms.Ok = true;
                return rms;
            }
            else
            {
                rms.Msg = res.ErrorMessage;
                return rms;
            }
        }

        public bool FindBucketByName(string name)
        {
            var res = collection.AsQueryable<Bucket>().FirstOrDefault(a => a.Name == name);
            if (res == null)
            {
                return false;
            }
            return true;
        }

        public Bucket FindBucketById(string id)
        {
            var oid = ObjectId.Empty;
            ObjectId.TryParse(id, out oid);
            if (oid==ObjectId.Empty)
            {
                return null;
            }
            var res = collection.AsQueryable<Bucket>().FirstOrDefault(a => a.ID==oid);
            return res;
        }
    }
}