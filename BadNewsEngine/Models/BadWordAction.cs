using System;
namespace BadNewsEngine.Models
{
    [Flags]
    public enum BadWordAction
    {
        Create = 1,
        Update = 2,
        Delete = 4
    }
}
