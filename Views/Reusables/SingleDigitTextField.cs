using AppKit;
using Foundation;
using System.Linq;

namespace Balsamic.Views
{
    [Register("SingleDigitTextField")]
    public class SingleDigitTextField : NSTextField
    {
        public SingleDigitTextField()
        {
            Cell = new CenteredTextFieldCell();
            Alignment = NSTextAlignment.Center;
            BackgroundColor = NSColor.Clear;
            Bezeled = true;
            Bordered = true;
        }
    }

    public class SingleDigitFormatter : NSFormatter
    {
        public SingleDigitFormatter()
        {
        }

        public override string StringFor(NSObject value)
        {
            return value.ToString();
        }

        public override bool GetObjectValue(out NSObject obj, string str, out NSString error)
        {
            obj = (NSString)str;
            error = null;
            return true;
        }

        public override bool IsPartialStringValid(string partialString, out string newString, out NSString error)
        {
            newString = partialString;
            error = null;
            if (partialString.Length >= 1)
            {
                return false;
            }

            if (partialString.All(char.IsDigit))
            {
                return true;
            }

            newString = new string(partialString.Where(char.IsDigit).ToArray());
            return false;
        }

        //public override bool IsPartialStringValid(ref string partialString, out NSRange proposedSelRange, string origString, NSRange origSelRange, out string error)
        //{
        //    string newString = partialString;
        //    error = null;

        //    if (partialString.Length > 1)
        //    {
        //        return false;
        //    }

        //    if (partialString.All)
        //    {

        //    }
        //}

        //public override bool IsPartialStringValid(ref string partialString, out NSRange proposedSelRange, string origString, NSRange origSelRange, out NSString error)
        //{
        //    if (partialString.Length > 1)
        //    {
        //        return false;
        //    }

        //    return true;
        //}
    }
}
