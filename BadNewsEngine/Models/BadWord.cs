using System;
namespace BadNewsEngine.Models
{
    public class BadWord
    {
		[DisplayName("負面字id")]
        public Guid Uid{ get; set; }

	    [DisplayName("負面字")] public string Word { get; set; }
		[DisplayName("動作")]
        public BadWordAction Action{ get; set; }
		[DisplayName("最後修改時間")]
        public DateTime ModifyTime{ get; set; }
    }

    /*
     CREATE TABLE eLand.dbo.BadWord( 
        [Uid] [uniqueidentifier] NOT NULL, --負面字識別ID 自動產生GUID
        [Word] [varchar](16) NOT NULL, --負面字
        [BadWordAction] [int] NOT NULL, --動作
        [ModifyTime] [datetime] NULL, --最後修改時間
        )
    */
}
