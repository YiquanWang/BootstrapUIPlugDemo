using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using FileUploadWeb.Common;

namespace FileUploadWeb
{
    /// <summary>
    /// UploadPdf 的摘要说明
    /// </summary>
    public class UploadPdf : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                string fileName = null;
                string filedir = context.Request.QueryString["action"];
                string id = context.Request.QueryString["id"];
                if (string.IsNullOrEmpty(filedir))
                {
                    filedir = "DefaultFile";
                }

                string tempDir = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string dir = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "Upload", filedir, tempDir);

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                HttpFileCollection myfiles = HttpContext.Current.Request.Files;

                string absPath = "";

                if (myfiles.Count > 0)
                {
                    HttpPostedFile postedFile = myfiles[0];

                    fileName = Path.GetFileName(postedFile.FileName);
                    string fullPath = Path.Combine(dir, fileName);
                    absPath = "/Upload/" + filedir + "/" + tempDir + "/" + fileName;
                    postedFile.SaveAs(fullPath);
                    SwftoolsHelp.PDF2Swf(absPath, absPath + ".swf");
                }
                context.Response.Write(new JavaScriptSerializer().Serialize(new { success = "true", filePath = absPath, fileName = fileName }));
            }
            catch (Exception ex)
            {
                context.Response.Write(new JavaScriptSerializer().Serialize(new { error = ex.Message }));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}