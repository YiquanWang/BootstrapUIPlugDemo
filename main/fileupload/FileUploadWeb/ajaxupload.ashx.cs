using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace UploadImg
{
    /// <summary>
    /// ajaxupload 的摘要说明
    /// </summary>
    public class ajaxupload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                string fileName = null;
                string filedir = context.Request.QueryString["action"];
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
                if (myfiles.Count > 0)
                {
                    HttpPostedFile postedFile = myfiles[0];

                    fileName = Path.GetFileName(postedFile.FileName);
                    postedFile.SaveAs(Path.Combine(dir, fileName));
                }
                context.Response.Write(new JavaScriptSerializer().Serialize(new { success = "true", filePath = "/Upload/" + filedir + "/" + tempDir + "/" + fileName, fileName = fileName }));
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