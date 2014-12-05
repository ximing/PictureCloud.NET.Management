using MongoDB.Driver;
using OurEDA.DianShangYun.Management.Areas.Admin.Models.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using OurEDA.DianShangYun.Management.Areas.Admin.Models;

namespace OurEDA.DianShangYun.Management.Service
{
    public class AuthService
    {
        protected readonly MongoClient _instance;
        protected readonly MongoServer _server;
        private readonly MongoDatabase dbContext;
        private readonly MongoCollection collection;
        private readonly MongoCollection usercollection;
        public AuthService()
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
        public User GetUserByTokenAndEmai(string email, string UserToken)
        {
            ObjectId id = ObjectId.Empty;
            ObjectId.TryParse(UserToken, out id);
            if (id == ObjectId.Empty)
            {
                return null;
            }
            var v = usercollection.AsQueryable<User>().FirstOrDefault(a => a.Email == email && a.ID == id);
            return v;
        }

        public User GetUserByEmaiAndPass(string email, string password)
        {

            var v = usercollection.AsQueryable<User>().FirstOrDefault(a => a.Email == email && a.Password == password);
            return v;
        }

        public ReturnModelService InsertUser(User u)
        {
            ReturnModelService rms = new ReturnModelService(){Ok=false};
            if (this.FinUserByEmail(u.Email)!=null)
            {
                rms.Msg = "Email重复";
                return rms;
            }
            var res = usercollection.Insert<User>(u);
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

        public User FinUserByEmail(string email)
        {
            return usercollection.AsQueryable<User>().FirstOrDefault(a => a.Email == email);
        }
    }
}