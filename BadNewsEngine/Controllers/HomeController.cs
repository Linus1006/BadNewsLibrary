using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using BadNewsEngine.Service;

namespace BadNewsEngine.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var mvcName = typeof(Controller).Assembly.GetName();
            //var isMono = Type.GetType("Mono.Runtime") != null;

            //ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
            //ViewData["Runtime"] = isMono ? "Mono" : ".NET";

            return View();
        }

        [HttpPost]
        public ActionResult Index(string name)
        {
            //TODO增加輸入檢核
            if (name == null) return View();

            ViewBag.name = name;
            var badNews = BadNewsHelper.FindBadNews(name).GroupBy(n => n.Title)
                .ToDictionary(m => m.Key, m => m.ToList()).Select(o => o.Value.FirstOrDefault()
                ).ToList().OrderByDescending(p => p.PostTime);


            return View(badNews);
        }
    }
}