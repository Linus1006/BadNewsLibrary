using System;
using System.Collections.Generic;

namespace BadNewsEngine.ViewModels
{
    public class JiebaResult
    {
        
        public Guid Uid { get; set; }

        public string Content { get; set; }
        
        public List<string> Names{ get; set; }
    }
}