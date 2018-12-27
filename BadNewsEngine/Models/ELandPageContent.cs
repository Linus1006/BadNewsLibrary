using System;
using System.ComponentModel;

namespace BadNewsEngine.Models
{
    public class ELandPageContent
    {

        //意藍資料庫
        [DisplayName("Uid")]
        public Guid Uid { get; set; }
        [DisplayName("意藍資料id")]
        public string ELandIdentify { get; set; }
        [DisplayName("來源類ID")]
        public string TypeId { get; set; }
        [DisplayName("來源類名稱")]
        public string Type { get; set; }
        [DisplayName("來源子類別ID")]
        public string SubTypeId { get; set; }
        [DisplayName("來源子類別名稱")]
        public string SubType { get; set; }
        [DisplayName("主文的ID")]
        public string MainPageId { get; set; }
        [DisplayName("標題或摘要")]
        public string Title { get; set; }
        [DisplayName("內容類型主文/回文")]
        public string ContentType { get; set; }
        [DisplayName("作者ID/名稱")]
        public string Author { get; set; }
        [DisplayName("來源網址")]
        public string PageUrl { get; set; }
        [DisplayName("貼文時間")]
        public DateTime PostTime { get; set; }
        [DisplayName("文章內容")]
        public string Content { get; set; }

        /*
        USE eLand
        CREATE TABLE [dbo].[ELandPageContent]( 
        [Uid] [uniqueidentifier] NOT NULL, --文章識別ID 自動產生GUID
        [ELandIdentify] [varchar](36) NOT NULL, --意藍的id
        [TypeId] [varchar](16) NOT NULL, --來源類ID
        [Type] [nvarchar](50) NOT NULL, --來源類名稱
        [SubTypeId] [varchar](16) NOT NULL, --來源子類別ID
        [SubType] [nvarchar](128) NOT NULL, --來源子類別名稱
        [MainPageId] [varchar](36) NOT NULL, --主文ID 回文類需要
        [Title] [nvarchar](300) NOT NULL, --標題或摘要
        [ContentType] [varchar](16) NOT NULL, --內容類型主文/回文
        [Author] [nvarchar](128) NULL, --作者ID/名稱
        [PageUrl] [varchar](max) NOT NULL, --來源網址
        [PostTime] [datetime] NULL, --貼文時間
        [Content] [nvarchar](max) NULL, --文章內容) 
        )

        CREATE INDEX PostTime on eLand.dbo.ELandPageContent(PostTime)
        */
    }
}
