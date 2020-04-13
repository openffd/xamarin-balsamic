namespace Balsamic
{
    static class InputValidation
    {
        internal static bool IsValidEmail(this string input)
        {
            try
            {
                var _ = new System.Net.Mail.MailAddress(input);
                return true;
            }
            catch (System.Exception) { return false; }
        }
    }
}
