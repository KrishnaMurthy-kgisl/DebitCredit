using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OfferLetter.Models
{
    public class ReportModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("Title")]
        public string Title { get; set; }
        public SelectList Titles { get; set; }
        public string selectedTitle { get; set; }

        [BsonElement("JOBDESCRIPTION")]
        public string JOBDESCRIPTION { get; set; }
        public SelectList JOBDESCRIPTIONS { get; set; }
        public string selectedJob { get; set; }

    }
    public class EmpDet
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("REGID")]
        public string REGID { get; set; }
        [BsonElement("EMPNAME")]
        public string EMPNAME { get; set; }
        [BsonElement("JOBDESCRIPTION")]
        public string JOBDESCRIPTION { get; set; }
        [BsonElement("SALARY")]
        public SALARYDET SALARY { get; set; }
        [BsonElement("DOJ")]
        public string DOJ { get; set; }
        [BsonElement("EMAIL")]
        public string EMAIL { get; set; }
        [BsonElement("MOBILE")]
        public string MOBILE { get; set; }
    }
    public class SALARYDET
    {
        [BsonElement("BP")]
        public string BP { get; set; }
        [BsonElement("HRA")]
        public string HRA { get; set; }
        [BsonElement("ALLOWANCE")]
        public string ALLOWANCE { get; set; }
    }
}