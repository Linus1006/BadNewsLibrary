using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BadNewsEngine.Service;

namespace BadNewsEngine.Controllers
{
    public class MaintainController : Controller
    {
        /// <summary>
        /// 顯示負面字詞庫
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //營運出問題了ＱＱ
            var badWords = BadNewsHelper.LoadBadWord();

            return View(badWords);
        }

        /// <summary>
        /// 建立負面字詞庫
        /// </summary>
        /// <param name="badWord"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(string badWord)
        {
            BadNewsHelper.CreateBadWord(badWord);

            return RedirectToAction("Index");
        }
    }
}