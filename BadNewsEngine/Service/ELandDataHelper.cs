using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using BadNewsEngine.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using NLog;

namespace BadNewsEngine.Service
{
    internal class ELandDataHelper
    {
        //整合測試的程式修改
		private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 執行ETL工程，將CRV的原始資料進行清洗有用的欄位至資料庫
        /// </summary>
        /// <returns>完成的筆數</returns>
        /// <param name="topNumber">要新增的前幾筆.</param>
        internal static int ETL(int? topNumber = null)
        {

            var sqlLoad = @"SELECT " + ((topNumber == null) ? "" : $@"TOP({topNumber})") + @"
                s1 as ELandIdentify, 
                s2 as TypeId,
                s3 as Type,
                s4 as SubTypeId,
                s5 as SubType,
                s6 as Title,
                s7 as Author,
                s8 as PostTime,
                s9 as Content,
                s10 as PageUrl,
                s16 as MainPageId,
                s18 as ContentType

                FROM [eLand].[dbo].[rawData] WHERE s18 = 'main' AND s3 <> 'facebook粉絲團' AND s3 <> '痞客邦 PIXNET'";

            var sqlCreate = @"INSERT INTO eLand.dbo.ELandPageContent
                                (Uid, ELandIdentify, TypeId, Type, SubTypeId, SubType, MainPageId, Title, ContentType, Author, PageUrl, PostTime, Content)
                         VALUES(newid() ,@ELandIdentify, @TypeId, @Type, @SubTypeId, @SubType, @MainPageId, @Title, @ContentType, @Author, @PageUrl, @PostTime, @Content);";

            using (var conn = DbHelper.OpenConnection("eLandDB"))
            {
                var eLandPageContents = conn.Query<ELandPageContent>(sqlLoad);
                return conn.Execute(sqlCreate, eLandPageContents);

            }

        }

        /// <summary>
        /// 執行ETL工程，將CRV的原始資料進行清洗有用的欄位至資料庫(DB直接做掉)
        /// </summary>
        /// <returns>The etl.</returns>
        /// <param name="topNumber">Top number.</param>
        internal static int FastEtl(int? topNumber = null)
        {
            //整合測試又發現錯誤，平常心～
			var s1lStr = @"INSERT INTO eLand.dbo.ELandPageContent
                (PageUrl, Uid, ELandIdentify, TypeId, Type, SubTypeId, SubType, MainPageId, Title, ContentType, Author, PostTime, Content)
                SELECT distinct s10 as PageUrl,
                newid(),
                s1 as ELandIdentify, 
                s2 as TypeId,
                s3 as Type,
                s4 as SubTypeId,
                s5 as SubType,
                s16 as MainPageId,
                s6 as Title,
                s18 as ContentType,
                s7 as Author,
                CONVERT([datetime],s8),
                s9 as Content
                FROM [eLand].[dbo].[rawData] WHERE s18 = 'main' AND s3 <> 'facebook粉絲團' AND s3 <> '痞客邦 PIXNET'";

            using (var conn = DbHelper.OpenConnection("eLandDB"))
            {

                return conn.Execute(s1lStr);

            }
        }

        /// <summary>
        /// 資料預處理，並建立負面資料庫的資料
        /// </summary>
        /// <returns>The process.</returns>
        /// <param name="startDate">Start date.</param>
        /// <param name="endDate">End date.</param>
		internal static int PreProcess(DateTime startDate, DateTime endDate)
        {
			//Log處理 請調整為上線模式
			logger.Trace($@"開始進行特定資料 從{startDate}到{endDate} 的預處理");
	        
	        //TODO 增加取用負面字
	        var badWords = BadNewsHelper.LoadBadWord();

            //資料的處理
			return CreateBadNewsLibrary( GenBadNewsLibrary(badWords,startDate, endDate));
        }
	    
	    /// <summary>
	    /// 每日批次作業
	    /// </summary>
	    /// <returns></returns>
	    internal static int PreProcessDaily()
	    {
		    var startDate = new DateTime(1980,10,6);
		    var endDate = DateTime.Now;
		    //Log處理
		    logger.Trace($@"開始進行每日資料 從{startDate}到{endDate} 的預處理");
	        
		    //
		    var badWords = BadNewsHelper.LoadBadWord(endDate.AddDays(-1));

		    //資料的處理
		    return CreateBadNewsLibrary( GenBadNewsLibrary(badWords,startDate, endDate));
	    }
	    

        /// <summary>
        /// Gens the bad news library.
        /// </summary>
        /// <returns>The bad news library.</returns>
        /// <param name="startDate">Start date.</param>
        /// <param name="endDate">End date.</param>
		private static List<BadNewsLibrary> GenBadNewsLibrary(IEnumerable<BadWord> badWords, DateTime? startDate = null, DateTime? endDate = null){

			//SELECT * FROM eLand.dbo.ELandPageContent WHERE PostTime > CONVERT(datetime,'2017-07-01')
			var sqlLoad = @"SELECT * FROM eLand.dbo.ELandPageContent WHERE PostTime >= @start AND PostTime <= @end";
			var start = startDate ?? new DateTime(1980,10,6);
			var end = endDate ?? DateTime.Now.AddDays(-1);


            var eLandPageContents = new List<ELandPageContent>();
            var badNews = new List<BadNewsLibrary>();

            using (var conn = DbHelper.OpenConnection("eLandDB"))
            {
				eLandPageContents = conn.Query<ELandPageContent>(sqlLoad,new { start, end}).ToList();
            }

            foreach (var pageContent in eLandPageContents)
            {

                var cutWords = JiebaHelper.PairDistinct(JiebaHelper.PosTagging(pageContent.Content));
                //取出負面字資料
                //var badWords = BadNewsHelper.LoadBadWord();
                //確認文章有無負面字
                var pageBadWord = JiebaHelper.GetBadWord(cutWords.Select(n => n.Word), badWords.Select(m => m.Word));

                //確定此篇有我們定義的負面字
                if (pageBadWord.Any())  //文章中有負面字才做
                {
                    var names = JiebaHelper.GetName(cutWords);  //找出名字

                    if (names.Any())    //有姓名才做
                    {
                        var locations = JiebaHelper.GetLocation(cutWords);  //找出地方

                        names.ForEach(n =>
                        {
                            badNews.Add(new BadNewsLibrary() { Uid = Guid.NewGuid(), NewsId = pageContent.Uid, Name = n, BadWords = JsonConvert.SerializeObject(pageBadWord), Locations = JsonConvert.SerializeObject(locations)});

                        });

                    }
                }
            }
            
			//Log處理
			logger.Trace($@"已完成 從{startDate.ToString()}到{endDate.ToString()} 資料的預處理");
            return badNews;
            
		}
        
        /// <summary>
		/// 新增負面資料庫資(TODO 可以改為新刪修同一方法)
        /// </summary>
        /// <returns>The bad news library.</returns>
        /// <param name="badNews">Bad news.</param>
		private static int CreateBadNewsLibrary(IEnumerable<BadNewsLibrary> badNews){

			var sqlCreate = @"INSERT INTO eLand.dbo.BadNewsLibrary
                                (Uid, NewsId, Name, Locations, BadWords)
                        VALUES(@Uid, @NewsId, @Name, @Locations, @BadWords);";

			using (var conn = DbHelper.OpenConnection("eLandDB"))
            {
               
				return conn.Execute(sqlCreate, badNews);

            }

		}
	    
	    
	    internal static List<ELandPageContent> LoadELandCompare()
	    {
		    var sqlStr = @"SELECT * FROM [eLand].[dbo].[ELandNlp]";
            
		    using (var conn = DbHelper.OpenConnection())
		    {
			    return conn.Query<ELandPageContent>(sqlStr).ToList();
		    }
	    }


    }
}