using AppKit;

namespace Balsamic.Views
{
    [Foundation.Register("CenteredTextField")]
    sealed class CenteredTextField : NSTextField
    {
        public CenteredTextField(CoreGraphics.CGRect frame, string message = "") : base(frame)
        {
            Cell = new CenteredTextFieldCell();
            Alignment = NSTextAlignment.Center;
            BackgroundColor = NSColor.Clear;
            Bezeled = false;
            Bordered = false;
            Editable = false;
            Selectable = false;
            StringValue = message;
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }
    }
}
