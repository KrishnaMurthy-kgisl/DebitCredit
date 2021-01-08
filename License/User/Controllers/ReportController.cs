using MongoDB.Bson;
using MongoDB.Driver.Builders;
using OfferLetter.App_Start;
using OfferLetter.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ZetPDF;
using ZetPDF.Drawing;
using ZetPDF.Drawing.Layout;
using ZetPDF.Pdf;
using ZetPDF.Pdf.IO;

namespace OfferLetter.Controllers
{
    public class ReportController : Controller
    {
        MongoContext _dbContext;
        public ReportController()
        {
            _dbContext = new MongoContext();
        }
        // GET: Report
        public ActionResult Index(ReportModel model)
        {


            var Titles = _dbContext._database.GetCollection("tempmap").Distinct("Title").ToList();

            List<SelectListItem> titleList = (from p in Titles.AsEnumerable()
                                              select new SelectListItem
                                              {
                                                  Text = p.ToString(),
                                                  Value = p.ToString().ToString()
                                              }).ToList();
            SelectList st = new SelectList(titleList, "Text", "Value");
            model.Titles = st;

            var jobList = _dbContext._database.GetCollection("joboffer").Distinct("JOBDESCRIPTION").ToList();
            List<SelectListItem> designationList = (from p in jobList.AsEnumerable()
                                                    select new SelectListItem
                                                    {
                                                        Text = p.ToString(),
                                                        Value = p.ToString().ToString()
                                                    }).ToList();

            st = new SelectList(designationList, "Text", "Value");
            model.JOBDESCRIPTIONS = st;
            return View(model);


        }

        // GET: Report/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Report/Create
        public ActionResult Create(ReportModel reportModel, FormCollection form)

        {

            var document = _dbContext._database.GetCollection<TempMapModel>("tempmap");
            var query = Query.And(Query.EQ("Title", reportModel.selectedTitle));
            var jobList = document.FindOne(query);

            var document1 = _dbContext._database.GetCollection<BsonDocument>("joboffer");
            var query1 = Query.And(Query.EQ("JOBDESCRIPTION", reportModel.selectedJob));
            var jobList1 = document1.Find(query1).ToList();
            string template = "";
            PdfDocument pdfdocument = new PdfDocument();
            pdfdocument.Info.Title = "Offer Letter";
            foreach (var r in jobList1)
            {
                template = jobList.Template;
                string temMap = jobList.Mappingcolumn;
                List<string> lst = new List<string>();
                lst.AddRange(jobList.Mappingcolumn.Replace("\r\n", "").Split(';'));
                foreach (string s in lst)
                {
                    List<string> temp = new List<string>();
                    temp.AddRange(s.Replace("[", "").Replace("]", "").Split(','));
                    if (temp[1].Contains("SALARY"))
                    {
                        string salarayDet = r.GetValue(temp[1]).ToString().Replace("\"", "").Replace("\\", "");
                        foreach (string t in salarayDet.Split(','))
                        {
                            template = template.Replace("[" + t.Split(':')[0] + "]", t.Split(':')[1]);
                        }
                    }
                    else
                        template = template.Replace("[" + temp[0] + "]", r.GetValue(temp[1]).ToString());
                }

                // Create an empty page
                PdfPage page = pdfdocument.AddPage();
                page.Size = PageSize.A4;
                // Get an XGraphics object for drawing
                XGraphics gfx = XGraphics.FromPdfPage(page);

                //XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);

                XFont font = new XFont("Times New Roman", 25, XFontStyle.BoldItalic);

                // Draw the text
                gfx.DrawString("KGiSL", font, XBrushes.Black,
                  new XRect(0, 30, page.Width, page.Height),
                  XStringFormats.TopCenter);

                font = new XFont("Times New Roman", 12, XFontStyle.Bold);
                XTextFormatter tf = new XTextFormatter(gfx);

                XRect rect = new XRect(40, 100, 500, 232);
                gfx.DrawRectangle(XBrushes.White, rect);
                //tf.Alignment = ParagraphAlignment.Left;
                tf.DrawString(template, font, XBrushes.Black, rect, XStringFormats.TopLeft);

                //// Create a font
                //font = new XFont("Times New Roman", 20, XFontStyle.Regular);

                //// Draw the text
                //gfx.DrawString(template, font, XBrushes.Black,
                //  new XRect(25, 70, 200, page.Height),
                //  XStringFormats.TopLeft);
            }

            // Save the document...
            const string filename = "D:\\HelloWorld_tempfile.pdf";
            pdfdocument.Save(filename);
            // ...and start a viewer.
            Process.Start(filename);
            byte[] fileBytes = GetFile(filename);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "OfferLetter.pdf");
        }
        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }

        // POST: Report/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Report/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Report/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Report/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Report/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
