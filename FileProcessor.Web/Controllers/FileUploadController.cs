using FileProcessor.Web.UserService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileProcessor.Web.Controllers
{
    public class FileUploadController : Controller
    {
        UserServiceClient userServiceClient = new UserServiceClient();
        Record record = new Record();        
        public ActionResult Index()
        {            
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase File)
        {
            string userFilePath = ConfigurationManager.AppSettings["directory"].ToString();

            if (File != null)
            {
                var fileName = Path.GetFileName(File.FileName);
                if (!Path.GetExtension(File.FileName).Contains("txt"))
                {
                    TempData["errorMessage"] = "Dosya bir txt dosyası değildir!";                    
                    return View();
                }
                try
                {
                    File.SaveAs(Path.Combine(userFilePath, fileName));                    
                }
                catch
                {
                    TempData["errorMessage"] = "Dosya kaydederken bir hata oluştu!";                    
                    return View();
                }
                record.FilePath = Path.Combine(userFilePath, fileName);
                userServiceClient.InsertRecord(record);
                return RedirectToAction("Index");
            }
            TempData["errorMessage"] = "Belirtilen klasörde dosya yok!";            
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }               
    }
}