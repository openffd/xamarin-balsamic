using AppKit;
using Foundation;
using IndexedTextFields = System.Collections.Generic.IEnumerable<(Balsamic.Views.SingleDigitTextField textField, int index)>;
using System.Linq;
using System.Collections.Generic;

namespace Balsamic.Views
{
    sealed partial class TwoFactorAuthWindowController : NSWindowController
    {
        IndexedTextFields IndexedTextFields => new List<SingleDigitTextField> {
            CodePart1TextField, CodePart2TextField, CodePart3TextField, CodePart4TextField, CodePart5TextField, CodePart6TextField
        }.Indexed();

        bool AreAllTextFieldsSet => IndexedTextFields.All(item => item.textField.HasContent());

        static NSPopover InitPopover() => new NSPopover() {
            Animates = true,
            Behavior = NSPopoverBehavior.Transient,
            ContentViewController = new ResendCodeViewController()
        };
        readonly System.Lazy<NSPopover> _lazyPopover = new System.Lazy<NSPopover>(InitPopover);
        NSPopover ResendCodePopover { get => _lazyPopover.Value; }

        #region Constructors

        public TwoFactorAuthWindowController(System.IntPtr handle) : base(handle) {}

        [Export("initWithCoder:")]
        public TwoFactorAuthWindowController(NSCoder coder) : base(coder) {}

        public TwoFactorAuthWindowController() : base("TwoFactorAuthWindow") {}

        #endregion

        internal new TwoFactorAuthWindow Window => (TwoFactorAuthWindow)base.Window;

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            SetupSubviewsUponVisibility();
        }

        void SetupSubviewsUponVisibility()
        {
            ContinueButton.Enabled = AreAllTextFieldsSet;
            ContinueButton.KeyEquivalent = String.KeyEquivalent.Return.String();

            foreach (var (textField, index) in IndexedTextFields)
            {
                textField.Delegate = this;
                textField.StringValue = string.Empty;
                textField.Tag = index;
            }
        }

        #region IBActions

        partial void Cancel(NSButton _)
        {
            Window.SheetParent.EndSheet(Window, NSModalResponse.Cancel);
        }

        partial void Continue(NSButton _)
        {
            System.Console.WriteLine("ContinueButton");
        }

        partial void ResendCode(NSButton button)
        {
            ResendCodePopover.Show(button.Bounds, button, NSRectEdge.MinYEdge);
        }

        #endregion
    }
}
