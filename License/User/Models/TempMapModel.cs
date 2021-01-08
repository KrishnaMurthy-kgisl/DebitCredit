using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfferLetter.Models
{
    public class TempMapModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("Title")]
        public string Title { get; set; }
        [BsonElement("Template")]
        public string Template { get; set; }
        [BsonElement("Mappingcolumn")]
        public string Mappingcolumn { get; set; }
    }
}