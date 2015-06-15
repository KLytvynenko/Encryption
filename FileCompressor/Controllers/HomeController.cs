using Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FileCompressor.Controllers
{
    public class HomeController : Controller
    {
        EncryptionService encryptionService = new EncryptionService();
        string _imagePath = "~/App_Data/Images";

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
        public ActionResult UploadFile(string id)
        {
            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent!= null && fileContent.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(fileContent.FileName);
                        var path = Path.Combine(Server.MapPath(_imagePath), fileName);
                        //TODO: write your keyUserId
                        encryptionService.EncryptFile("keyUserId", fileContent.InputStream, path);
                    }
                }
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            return RedirectToAction("ShowUploadedFiles");
        }

        public ActionResult ShowUploadedFiles()
        {
            DirectoryInfo directory = new DirectoryInfo(Server.MapPath(_imagePath));
            IEnumerable<FileInfo> files = directory.GetFiles();
            return View("UploadFile", files);
        }

        //On proof of concept version we can only encrypt image file
        public ActionResult ShowImage(string id)
        {
            var dir = Server.MapPath(_imagePath);
            var path = Path.Combine(dir, id);
            return File(encryptionService.DecryptFile(path).ToArray(), "image/jpeg");
        }
    }
}