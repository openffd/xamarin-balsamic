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
            Initialize();
        }

        public SingleDigitTextField(System.IntPtr handle) : base(handle)
        {
            Initialize();
        }

        void Initialize()
        {
            Cell = new CenteredTextFieldCell();
            Alignment = NSTextAlignment.Center;
            BackgroundColor = NSColor.Clear;
            Bezeled = true;
            Bordered = true;
            Editable = true;
            StringValue = string.Empty;
            Font = NSFont.FromFontName("HelveticaNeue-Bold", 24);
            Formatter = new SingleDigitFormatter();
        }
    }

    public class SingleDigitFormatter : NSFormatter
    {
        public SingleDigitFormatter() {}

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

        public override bool IsPartialStringValid(
            ref string partialString,
            out NSRange proposedSelRange,
            string origString,
            NSRange origSelRange,
            out string error)
        {
            error = null;
            proposedSelRange = new NSRange(0, 1);
            switch (partialString.Length)
            {
                case 0:
                    partialString = string.Empty;
                    return false;
                case 1:
                    return int.TryParse(partialString, out _);
                default:
                    partialString = partialString.Substring(partialString.Length - 1);
                    return false;
            }
        }
    }
}
