using MongoDB.Bson;
using MongoDB.Driver;
using OurEDA.DianShangYun.Management.Areas.Admin.Models.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver.Linq;

namespace OurEDA.DianShangYun.Management.Service
{
    public class CatalogueService
    {
        protected readonly MongoClient _instance;
        protected readonly MongoServer _server;
        private readonly MongoDatabase dbContext;
        private readonly MongoCollection collection;
        private readonly MongoCollection usercollection;
        private readonly MongoCollection catalogueCollection;
        public CatalogueService()
        {
            var credential = MongoCredential.CreateMongoCRCredential("PictureCloud", "YOUR_MONGO_USER", "YOUR_MONGO_PASSWORD");

            var settings = new MongoClientSettings
            {
                Server = new MongoServerAddress("mongo.oureda.net", 27017),
                Credentials = new[] { credential }
            };

            var mongoClient = new MongoClient(settings);
            _server = mongoClient.GetServer();
            dbContext = _server.GetDatabase("PictureCloud");
            collection = dbContext.GetCollection<Bucket>("Bucket");
            usercollection = dbContext.GetCollection<User>("User");
            catalogueCollection = dbContext.GetCollection<Catalogue>("Catalogue");
        }
        public List<Catalogue> GetCataloguesByBucketId(ObjectId id)
        {
            //ObjectId id = ObjectId.Empty;
            //ObjectId.TryParse(bucketId, out id);
            //if (id == ObjectId.Empty)
            //{
            //    return null;
            //}
            var v = catalogueCollection.AsQueryable<Catalogue>().Where(a=>a.BucketId==id).ToList();
            return v;
        }
    }
}