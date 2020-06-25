using AppKit;
using Foundation;

namespace Balsamic.Views
{
    [Register("SingleDigitTextField")]
    internal sealed class SingleDigitTextField : NSTextField
    {
        internal bool HasContent()
        {
            return StringValue.Length > 0;
        }

        public SingleDigitTextField()
        {
            Initialize();
        }

        public SingleDigitTextField(System.IntPtr handle) : base(handle)
        {
            Initialize();
        }

        private void Initialize()
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
