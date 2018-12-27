using System;
using BadNewsEngine.Models;
using BadNewsEngine.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace BadNewsEngine.Service
{
    public static class BadNewsHelper
    {
        /// <summary>
        /// Loads the bad word.
        /// </summary>
        /// <returns>The bad word.</returns>
        internal static List<BadWord> LoadBadWord(DateTime? startDate = null)
        {
            var sqlStr = @"SELECT * FROM [eLand].[dbo].[BadWord]";
            
            if (startDate != null)
            {
                sqlStr = sqlStr + $@" WHERE ModifyTime >= @startDate";
            }
            
            using (var conn = DbHelper.OpenConnection())
            {
               return conn.Query<BadWord>(sqlStr,new {startDate}).ToList();
            }
        }

        /// <summary>
        /// Creates the bad word.
        /// </summary>
        /// <returns>The bad word.</returns>
        /// <param name="word">Word.</param>
        internal static int CreateBadWord(string word)
        {
            var sqlStr = @"INSERT INTO eLand.dbo.BadWord
                                (Uid, Word, BadWordAction, ModifyTime)
                         VALUES(@Uid, @Word, @Action, @ModifyTime) ";

            using (var conn = DbHelper.OpenConnection("eLandDB"))
            {
                return conn.Execute(sqlStr,
                    new BadWord()
                    {
                        Uid = Guid.NewGuid(),
                        Word = word,
                        Action = BadWordAction.Create,
                        ModifyTime = DateTime.Now
                    });
            }
        }

        /// <summary>
        /// 找負面新聞
        /// </summary>
        /// <returns>回傳View Model.</returns>
        /// <param name="name">Name.</param>
        public static List<BadNewsResult> FindBadNews(string name = "葉柏廷")
        {
            //須調整為join
            var sqlStr = $@"SELECT a.Uid, a.NewsId, a.Name, b.PageUrl, b.Title, a.BadWords, a.Locations, b.PostTime
                                FROM BadNewsLibrary as a LEFT JOIN ELandPageContent as b ON a.NewsId = b.Uid
                                WHERE Name = @name";

            using (var conn = DbHelper.OpenConnection("eLandDB"))
            {
                return conn.Query<BadNewsResult>(sqlStr, new {name}).ToList();
            }
        }


        public static void CreteNameParse(Guid uid, string name)
        {
            
            var sqlStr = @"INSERT INTO eLand.dbo.NameParse
                                (Uid, Name)
                         VALUES(@uid, @name) ";

            using (var conn = DbHelper.OpenConnection("eLandDB"))
            {
                conn.Execute(sqlStr,new { uid, name});
            }
            
        }


        public static List<BadNewsResult> FindBadNewsFromES(string name = "葉柏廷")
        {
            //TODO:新增從ES尋找負面新聞
            //整合測試過程的程式修改

            var sqlStr = $@"SELECT a.Uid, a.NewsId, a.Name, b.PageUrl, b.Title, a.BadWords, a.Locations, b.PostTime
                                FROM BadNewsLibrary as a LEFT JOIN ELandPageContent as b ON a.NewsId = b.Uid
                                WHERE Name = @name";

            using (var conn = DbHelper.OpenConnection("eLandDB"))
            {
                return conn.Query<BadNewsResult>(sqlStr, new { name }).ToList();
            }
        }


    }
}