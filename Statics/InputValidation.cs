using System.Net.Mail;

namespace Balsamic
{
    internal static class InputValidation
    {
        internal static bool IsValidEmail(this string input)
        {
            try
            {
                MailAddress _ = new MailAddress(input);
                return true;
            }
            catch (System.Exception) { return false; }
        }
    }
}
