using System.ComponentModel;

namespace Balsamic
{
    static class String
    {
        internal enum KeyEquivalent
        {
            [Description("\r")]
            Return,
        }

        internal enum ErrorMessage
        {
            [Description("⚠️ Not a valid email")]
            InvalidEmail,
        }
    }
}
