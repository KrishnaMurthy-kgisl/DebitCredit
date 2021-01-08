using OfferLetter.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using OfferLetter.Models;
using MongoDB.Driver.Builders;
using MongoDB.Driver;

namespace OfferLetter.Controllers
{
    public class TempMapController : Controller
    {
        MongoContext _dbContext;
        public TempMapController()
        {
            _dbContext = new MongoContext();
        }
        // GET: TempMap
        public ActionResult Index()
        {
            try
            {
                var TempDetails = _dbContext._database.GetCollection<TempMapModel>("tempmap").FindAll().ToList();
                return View(TempDetails);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: TempMap/Details/5
        public ActionResult Details(string id)
        {
            var ID = Query<TempMapModel>.EQ(p => p.Id, new ObjectId(id));
            var TempDetails = _dbContext._database.GetCollection<TempMapModel>("tempmap").FindOne(ID);
            return View(TempDetails);
        }

        // GET: TempMap/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TempMap/Create
        [HttpPost]
        public ActionResult Create(TempMapModel tempMapModel)
        {
            try
            {
                // TODO: Add insert logic here
                var document = _dbContext._database.GetCollection<BsonDocument>("tempmap");
                var query = Query.And(Query.EQ("name", tempMapModel.Template), Query.EQ("Mappingcolumn", tempMapModel.Mappingcolumn), Query.EQ("Title", tempMapModel.Title));

                var count = document.FindAs<TempMapModel>(query).Count();


                if (count == 0)
                {
                    var result = document.Insert(tempMapModel);
                }
                else
                {
                    TempData["Message"] = "Tempalte ALready Exist";
                    return View(tempMapModel);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                return View();
            }
        }

        // GET: TempMap/Edit/5
        public ActionResult Edit(string id)
        {
            try
            {
                var document = _dbContext._database.GetCollection<TempMapModel>("tempmap");

                var Tempcount = document.FindAs<TempMapModel>(Query.EQ("_id", new ObjectId(id))).Count();

                if (Tempcount > 0)
                {
                    var TempObjectid = Query<TempMapModel>.EQ(p => p.Id, new ObjectId(id));

                    var TempDetail = _dbContext._database.GetCollection<TempMapModel>("tempmap").FindOne(TempObjectid);

                    return View(TempDetail);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // POST: TempMap/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, TempMapModel tempMapModel)
        {
            try
            {
                tempMapModel.Id = new ObjectId(id);
                //Mongo Query  
                var tempObjectid = Query<TempMapModel>.EQ(p => p.Id, new ObjectId(id));
                // Document Collections  
                var collection = _dbContext._database.GetCollection<TempMapModel>("tempmap");
                // Document Update which need Id and Data to Update  
                var result = collection.Update(tempObjectid, Update.Replace(tempMapModel), UpdateFlags.None);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TempMap/Delete/5
        public ActionResult Delete(string id)
        {
            var ID = Query<TempMapModel>.EQ(p => p.Id, new ObjectId(id));
            var TempDetails = _dbContext._database.GetCollection<TempMapModel>("tempmap").FindOne(ID);
            return View(TempDetails);
        }

        // POST: TempMap/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, TempMapModel tempMapModel)
        {
            try
            {
                //Mongo Query  
                var tempObjectid = Query<TempMapModel>.EQ(p => p.Id, new ObjectId(id));
                // Document Collections  
                var collection = _dbContext._database.GetCollection<TempMapModel>("tempmap");
                // Document Delete which need ObjectId to Delete Data   
                var result = collection.Remove(tempObjectid, RemoveFlags.Single);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
