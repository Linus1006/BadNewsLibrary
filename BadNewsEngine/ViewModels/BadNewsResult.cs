using System;


namespace BadNewsEngine.ViewModels
{
    public class BadNewsResult
    {
		[DisplayName("負面資料庫id")]
		public Guid Uid { get; set; }

		[DisplayName("億藍資料id")]
        public Guid NewsId { get; set; }

		[DisplayName("姓名")]
        public string Name { get; set; }

		[DisplayName("可能區域")]
        public string Locations { get; set; }

		[DisplayName("負面字")]
        public string BadWords { get; set; }

		[DisplayName("標題")]
		public string Title { get; set; }

        [DisplayName("來源網址")]
        public string PageUrl { get; set; }

        [DisplayName("貼文時間")]
        public DateTime PostTime { get; set; }

        [DisplayName("文章內容")]
        public string Content { get; set; }

    }
}
