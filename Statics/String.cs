using System.ComponentModel;

namespace Balsamic
{
    static class String
    {
        internal enum ErrorMessage
        {
            [Description("⚠️ Not a valid email")]
            InvalidEmail = 0
        }
    }
}
