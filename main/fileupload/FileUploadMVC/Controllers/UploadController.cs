using System;
using System.IO;
using System.Web.Mvc;
using FileUploadMVC.Util;

namespace FineUploader
{
    public class UploadController : Controller
    {
        [HttpPost]
        public FineUploaderResult UploadFile(FineUpload upload)
        {
            // asp.net mvc will set extraParam1 and extraParam2 from the params object passed by Fine-Uploader
            try
            {
                string fileName = null;
                string filedir = Request.QueryString["action"];
                if (string.IsNullOrEmpty(filedir))
                {
                    filedir = "DefaultFile";
                }

                string tempDir = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string dir = Path.Combine(Request.PhysicalApplicationPath, "Upload", filedir, tempDir);

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                fileName = Path.GetFileName(upload.Filename);
                upload.SaveAs(Path.Combine(dir, fileName));
            
                return new FineUploaderResult(true, new { success = "true", filePath = "/Upload/" + filedir + "/" + tempDir + "/" + fileName, fileName = fileName });
            }
            catch (Exception ex)
            {
                return new FineUploaderResult(false, error: ex.Message);
            }
        }

        [HttpPost]
        public FineUploaderResult UploadPdf(FineUpload upload)
        {
            try
            {
                string fileName = null;
                string filedir = Request.QueryString["action"];
                if (string.IsNullOrEmpty(filedir))
                {
                    filedir = "DefaultFile";
                }

                string tempDir = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string dir = Path.Combine(Request.PhysicalApplicationPath, "Upload", filedir, tempDir);

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                fileName = Path.GetFileName(upload.Filename);
                upload.SaveAs(Path.Combine(dir, fileName));
                var absPath = "/Upload/" + filedir + "/" + tempDir + "/" + fileName;
                SwftoolsHelp.PDF2Swf(absPath, absPath + ".swf");

                return new FineUploaderResult(true, new { success = "true", filePath = absPath, fileName = fileName });
            }
            catch (Exception ex)
            {
                return new FineUploaderResult(false, error: ex.Message);
            }
        }
    }
}