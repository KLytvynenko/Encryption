using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FileCompressor.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return ShowUploadedFiles();
            }
            
            return View();
        }


        [Authorize]
        public ActionResult UploadFile()
        {
            ViewBag.Message = "Upload your file.";

            return View();
        }

        [HttpPost]
        public Task<ActionResult> UploadFile(string id)
        {
            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        // get a stream
                        var stream = fileContent.InputStream;
                        // and optionally write the file to disk
                        string fileName = Path.GetFileName(file) + Guid.NewGuid();
                        var path = Path.Combine(Server.MapPath("~/App_Data/Images"), fileName);
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                    }
                }
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                //return Json("Upload failed");
            }

            return RedirectToAction("ShowUploadedFiles");
        }

        public ActionResult ShowUploadedFiles()
        {
            DirectoryInfo directory = new DirectoryInfo(Server.MapPath(@"~\App_Data\Images"));
            var files = directory.GetFiles().ToList();

            return View("UploadFile", files);
        }

        public ActionResult ShowImage(string id)
        {
            var dir = Server.MapPath(@"~\App_Data\Images");
            var path = Path.Combine(dir, id);
            return base.File(path, "image/jpeg");
        }
    }
}