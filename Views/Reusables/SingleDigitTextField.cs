using AppKit;
using Foundation;

namespace Balsamic.Views
{
    public interface ISingleDigitTextFieldDelegate : INSTextFieldDelegate
    {
        void TextFieldDidDelete();
    }

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
            Font = NSFont.FromFontName("HelveticaNeue-Bold", 24);
            Formatter = new SingleDigitFormatter();
            StringValue = string.Empty;
        }
    }
}
