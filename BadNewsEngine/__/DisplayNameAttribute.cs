using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadNewsEngine
{
    /*
     * namespace為ESUN的用意是ESUN的下面命名空間會以此Attribute為優先, 
     * 不會出現DisplayNameAttribute 同時定義於ESUN 與 System.ComponentModel的錯誤
     */

    /// <summary>顯示名稱</summary>
    /// <remarks>與System.ComponentModel.DisplayNameAttribute相同, 只是原本的DisplayNameAttribute無法用於Field上。</remarks>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class DisplayNameAttribute : System.ComponentModel.DisplayNameAttribute
    {
        public DisplayNameAttribute(string displayName) : base(displayName) { }
    }
}
