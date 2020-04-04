using System.ComponentModel;

namespace Balsamic
{
    public static class String
    {
        public enum ErrorMessage
        {
            [Description("⚠️ Not a valid email")]
            InvalidEmail = 0
        }
    }
}
