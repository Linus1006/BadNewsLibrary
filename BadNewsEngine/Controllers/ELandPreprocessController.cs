using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using BadNewsEngine.Service;
using BadNewsEngine.Models;
using BadNewsEngine.ViewModels;
using System.Web;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BadNewsEngine.Controllers
{
    public class ELandPreprocessController : ApiController
    {
        /// <summary>
        /// Get the specified startDate and endDate.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="startDate">Start date.</param>
        /// <param name="endDate">End date.</param>
        public string Get(DateTime startDate, DateTime endDate)
        {
            //紀錄執行時間
            HttpContext.Current.Server.ScriptTimeout = 600000;
            Stopwatch sw = new Stopwatch();
            sw.Reset();
            sw = Stopwatch.StartNew();

            try
            {
                ELandDataHelper.PreProcess(startDate, endDate);
                //每日批次測試
                //ELandDataHelper.PreProcessDaily();
            }
            catch (Exception e)
            {
                return e.ToString();
            }

            sw.Stop();
            long ms = sw.ElapsedMilliseconds;
            return $@"負面資料庫資料預處理從{startDate}至{endDate}:共處理{ms / 1000}秒";
        }
    }
}