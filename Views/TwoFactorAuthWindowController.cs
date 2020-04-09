using AppKit;
using Foundation;
using IndexedTextFields = System.Collections.Generic.IEnumerable<(Balsamic.Views.SingleDigitTextField textField, int index)>;
using System.Linq;
using System.Collections.Generic;

namespace Balsamic.Views
{
    public partial class TwoFactorAuthWindowController : NSWindowController
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

            ContinueButton.Enabled = AreAllTextFieldsSet;
            ContinueButton.KeyEquivalent = "\r";

            foreach (var (textField, index) in IndexedTextFields)
            {
                textField.Delegate = this;
                textField.StringValue = string.Empty;
                textField.Tag = index;
            }
        }

        public new TwoFactorAuthWindow Window => (TwoFactorAuthWindow)base.Window;

        #region IBActions

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

        #endregion
    }
}
