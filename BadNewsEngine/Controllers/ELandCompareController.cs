using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using BadNewsEngine.Service;
using BadNewsEngine.Models;
using System.Web;
using BadNewsEngine.ViewModels;

namespace BadNewsEngine.Controllers
{
    public class ELandCompareController : ApiController
    {
        /// <summary>
        /// 與億藍比對結果使用 主要是測試比較使用
        /// </summary>
        /// <returns></returns>
        public List<JiebaResult> Get()
        {
            HttpContext.Current.Server.ScriptTimeout = 300000;
            var eLandCompares = ELandDataHelper.LoadELandCompare();
            var jiebaResults = new List<JiebaResult>();

            foreach (var item in eLandCompares)
            {
                var names = JiebaHelper.GetName(JiebaHelper.PairDistinct(JiebaHelper.PosTagging(item.Content)));

                var jiebaResult = new JiebaResult() {Uid = item.Uid, Names = names, Content = item.Content};

                //以下寫法不要常用 很花資源 懶人測試用
//                foreach (var nameItem in names)
//                {
//                    BadNewsHelper.CreteNameParse(item.Uid, nameItem);
//                }

                jiebaResults.Add(jiebaResult);
            }

            return jiebaResults;
        }

        /*
         *INSERT INTO [eLand].[dbo].[ELandNlp]
                ([Uid], [Content])
                SELECT TOP(100)
                [Uid],
                [Content]
                FROM [eLand].[dbo].[ELandPageContent]
                ORDER BY PostTime DESC
         * 
         */
    }
}