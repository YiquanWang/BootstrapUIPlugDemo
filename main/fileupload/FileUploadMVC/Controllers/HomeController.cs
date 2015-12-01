using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace fine_upload.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult FineUploader()
        {
            return View();
        }

        public ActionResult Uploadify()
        {
            return View();
        }

        public ActionResult PdfShow()
        {
            return View();
        }

    }
}
