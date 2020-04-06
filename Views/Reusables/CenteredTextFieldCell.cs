using AppKit;
using CoreGraphics;
using Foundation;
using System;

namespace Balsamic.Views
{
    [Register("CenteredTextFieldCell")]
    public class CenteredTextFieldCell : NSTextFieldCell
    {
        public CenteredTextFieldCell()
        {
            Font = NSFont.FromFontName("HelveticaNeue", 14);
        }

        public override void EditWithFrame(CGRect aRect, NSView inView, NSText editor, NSObject delegateObject, NSEvent theEvent)
        {
            base.EditWithFrame(AdjustFrame(aRect), inView, editor, delegateObject, theEvent);
        }

        public override void SelectWithFrame(CGRect aRect, NSView inView, NSText editor, NSObject delegateObject, nint selStart, nint selLength)
        {
            base.SelectWithFrame(AdjustFrame(aRect), inView, editor, delegateObject, selStart, selLength);
        }

        public override void DrawInteriorWithFrame(CGRect cellFrame, NSView inView)
        {
            base.DrawInteriorWithFrame(AdjustFrame(cellFrame), inView);
        }

        private CGRect AdjustFrame(CGRect frame)
        {
            var fontDelta = Font.Ascender - Font.Descender;
            var dy = Math.Floor(0.5 * (frame.Height - fontDelta));
            return frame.Inset(0, (nfloat)dy);
        }
    }
}
