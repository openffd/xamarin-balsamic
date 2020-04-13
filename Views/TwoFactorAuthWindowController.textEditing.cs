using AppKit;
using Foundation;
using ObjCRuntime;
using System.Linq;

namespace Balsamic.Views
{
    partial class TwoFactorAuthWindowController: INSTextFieldDelegate, INSControlTextEditingDelegate
    {
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

        void HandleControlTextDidChange(NSNotification notification)
        {
            var textField = (NSTextField)notification.Object;
            if (textField.StringValue.Length == 0 && textField.Tag != 0)
            {
                Window.SelectKeyViewPrecedingView(textField);
                return;
            }

            if (textField.Tag < IndexedTextFields.Count() - 1)
            {
                Window.SelectKeyViewFollowingView(textField);
            }
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

        void HandleDeleteBackward(NSTextField textField)
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
