using AppKit;
using Foundation;
using ObjCRuntime;

namespace Balsamic.Views
{
    public partial class TwoFactorAuthWindowController : NSWindowController, INSTextFieldDelegate, INSControlTextEditingDelegate
    {
        public TwoFactorAuthWindowController(System.IntPtr handle) : base(handle) {}

        [Export("initWithCoder:")]
        public TwoFactorAuthWindowController(NSCoder coder) : base(coder) {}

        public TwoFactorAuthWindowController() : base("TwoFactorAuthWindow") {}

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            CodePart1TextField.Delegate = this;
            CodePart1TextField.Tag = 1;

            CodePart2TextField.Delegate = this;
            CodePart2TextField.Tag = 2;

            CodePart3TextField.Delegate = this;
            CodePart3TextField.Tag = 3;

            CodePart4TextField.Delegate = this;
            CodePart4TextField.Tag = 4;

            CodePart5TextField.Delegate = this;
            CodePart5TextField.Tag = 5;

            CodePart6TextField.Delegate = this;
            CodePart6TextField.Tag = 6;
        }

        public new TwoFactorAuthWindow Window => (TwoFactorAuthWindow)base.Window;

        partial void Cancel(NSButton _)
        {
            Window.SheetParent.EndSheet(Window, NSModalResponse.Cancel);
        }

        partial void Continue(NSButton _)
        {
            System.Console.WriteLine("ContinueButton");
        }

        partial void ResendCode(NSButton _)
        {
            System.Console.WriteLine("ResendCodeButton");
        }

        [Export("controlTextDidChange:")]
        public void ControlTextDidChange(NSNotification notification)
        {
            var textField = (NSTextField)notification.Object;
            //switch (textField.Tag)
            //{
            //    case 1:
            //        if (textField.StringValue.Length == 0)
            //        {

            //        }
            //        break;
            //    case 6:
            //        break;
            //}

            if (textField.StringValue.Length == 0 && textField.Tag != 1)
            {
                Window.SelectKeyViewPrecedingView(textField);
                return;
            }

            if (textField.Tag != 6)
                Window.SelectKeyViewFollowingView(textField);
        }

        private void HandleDeleteBackward(NSTextField textField)
        {
            if (textField.StringValue.Length == 0)
            {
                if (textField.Tag == 1)
                    return;

                Window.SelectKeyViewPrecedingView(textField);
            }

            textField.StringValue = string.Empty;
        }

        [Export("control:textView:doCommandBySelector:")]
        public bool DoCommandBySelector(NSControl control, NSTextView _, Selector selector)
        {
            var textField = (NSTextField)control;
            if (selector.Equals(new Selector("deleteBackward:")))
            {
                HandleDeleteBackward(textField);
                return true;
            }
            else if (selector.Equals(new Selector("insertTab:")))
            {
                    if (textField.Tag == 6)
                    return true;
            }
            return false;
        }
    }
}
