﻿using AppKit;
using Foundation;

namespace Balsamic.Views
{
    [Register("SingleDigitTextField")]
    public class SingleDigitTextField : NSTextField
    {
        public bool HasContent() => StringValue.Length > 0;

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