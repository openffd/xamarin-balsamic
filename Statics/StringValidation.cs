namespace Balsamic
{
    public static class StringValidation
    {
        public static bool IsValidEmail(this string input)
        {
            try
            {
                var _ = new System.Net.Mail.MailAddress(input);
                return true;
            }
            catch (System.FormatException) { return false; }
        }
    }
}
