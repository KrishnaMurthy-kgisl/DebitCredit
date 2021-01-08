using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace OfferLetter.App_Start
{
    public class MongoContext
    {
        MongoClient _client;
        MongoServer _server;
        public MongoDatabase _database;
        public MongoContext()        //constructor   
        {
            // Reading credentials from Web.config file   
            string MongoDatabaseName = ConfigurationManager.AppSettings["MongoDatabaseName"]; //offerl
            string MongoUsername = ConfigurationManager.AppSettings["MongoUsername"]; //demouser  
            string MongoPassword = ConfigurationManager.AppSettings["MongoPassword"]; //Pass@123  
            string MongoPort = ConfigurationManager.AppSettings["MongoPort"];  //27017  
            string MongoHost = ConfigurationManager.AppSettings["MongoHost"];  //localhost  

            // Creating credentials  
            var credential = MongoCredential.CreateCredential
                                        (MongoDatabaseName,
                             MongoUsername,
                             MongoPassword);

            //// Creating MongoClientSettings  
           
            var settings = new MongoClientSettings
            {
                Credentials = new[] { credential },
                Server = new MongoServerAddress(MongoHost, Convert.ToInt32(MongoPort))
            };
            _client = new MongoClient(settings);
            _server = _client.GetServer();
            _database = _server.GetDatabase(MongoDatabaseName);
        }
    }
}