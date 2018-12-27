using System;

namespace BadNewsEngine.Models
{
    public class BadNewsLibrary
    {
		public Guid Uid{ get; set; }
        
		public Guid NewsId{ get; set; }

		public string Name{ get; set; }

		public string Locations{ get; set; }

		public string BadWords{ get; set; }

        /*
        CREATE TABLE eLand.dbo.BadNewsLibrary( 
        [Uid] [uniqueidentifier] NOT NULL, --負面新聞紀錄ID 自動產生GUID
        [NewsId] [uniqueidentifier] NOT NULL, --億藍的資料關聯
        [Name] [varchar](32) NOT NULL, --姓名 Index
        [Locations] [varchar](max) NOT NULL, --地點群Index
        [BadWords] [varchar](max) NOT NULL, --負面字群 Index

        )

        CREATE INDEX Name on eLand.dbo.BadNewsLibrary(Name)

        */
    }
}
