using AppKit;
using Foundation;
using ObjCRuntime;
using System.Linq;
using System.Collections.Generic;
using IndexedTextFields = System.Collections.Generic.IEnumerable<(Balsamic.Views.SingleDigitTextField textField, int index)>;

namespace Balsamic.Views
{
    public partial class TwoFactorAuthWindowController : NSWindowController, INSTextFieldDelegate, INSControlTextEditingDelegate
    {
        private IndexedTextFields IndexedTextFields => new List<SingleDigitTextField> {
            CodePart1TextField, CodePart2TextField, CodePart3TextField, CodePart4TextField, CodePart5TextField, CodePart6TextField
        }.Indexed();

        private bool AreAllTextFieldsSet => IndexedTextFields.All(item => item.textField.HasContent());

        #region Constructors

        public TwoFactorAuthWindowController(System.IntPtr handle) : base(handle) {}

        [Export("initWithCoder:")]
        public TwoFactorAuthWindowController(NSCoder coder) : base(coder) {}

        public TwoFactorAuthWindowController() : base("TwoFactorAuthWindow") {}

        #endregion

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            ContinueButton.Enabled = false;

            foreach (var (textField, index) in IndexedTextFields)
            {
                textField.Tag = index;
                textField.Delegate = this;
            }
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
            try
            {
                HandleControlTextDidChange(notification);
            }
            finally
            {
                ContinueButton.Enabled = AreAllTextFieldsSet;
            }
        }

        private void HandleControlTextDidChange(NSNotification notification)
        {
            var textField = (NSTextField)notification.Object;
            if (textField.StringValue.Length == 0 && textField.Tag != 0)
            {
                Window.SelectKeyViewPrecedingView(textField);
                return;
            }

            if (textField.Tag < IndexedTextFields.Count() - 1)
                Window.SelectKeyViewFollowingView(textField);
        }

        [Export("control:textView:doCommandBySelector:")]
        public bool DoCommandBySelector(NSControl control, NSTextView _, Selector selector)
        {
            var textField = (NSTextField)control;
            if (selector.Equals(new Selector("deleteBackward:")))
            {
                HandleDeleteBackward(textField);
                ContinueButton.Enabled = AreAllTextFieldsSet;
                return true;
            }
            else if (selector.Equals(new Selector("insertTab:")))
            {
                return textField.Tag >= IndexedTextFields.Count() - 1;
            }
            return false;
        }

        private void HandleDeleteBackward(NSTextField textField)
        {
            if (textField.StringValue.Length == 0)
            {
                if (textField.Tag == 0)
                    return;

                Window.SelectKeyViewPrecedingView(textField);
            }

            textField.StringValue = string.Empty;
        }
    }
}
