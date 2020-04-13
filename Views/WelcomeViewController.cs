using AppKit;
using Foundation;
using L2FAWindowController = System.Lazy<Balsamic.Views.TwoFactorAuthWindowController>;
using ObjCRuntime;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Balsamic.Views
{
    sealed partial class WelcomeViewController : NSViewController, INSTextFieldDelegate
    {
        internal string StoryboardIdentifier => Class.ToString();
        internal NSWorkspace Workspace { get; private set; }

        static TwoFactorAuthWindowController Init2FAWindowController() => new TwoFactorAuthWindowController();
        readonly L2FAWindowController _lazy2FAWindowController = new L2FAWindowController(Init2FAWindowController);
        TwoFactorAuthWindowController TwoFactorAuthWindowController => _lazy2FAWindowController.Value;

        #region Constructors

        public WelcomeViewController(System.IntPtr handle) : base(handle)
        {
            Initialize();
        }

        [Export("initWithCoder:")]
        public WelcomeViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        public WelcomeViewController() : base(typeof(WelcomeViewController).ToString(), NSBundle.MainBundle)
        {
            Initialize();
        }

        private void Initialize()
        {
            Workspace = NSWorkspace.SharedWorkspace;
        }

        #endregion

        internal new NSView View => base.View;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            AppleIDTextField.Delegate = this;
            PasswordTextField.Delegate = this;
        }

        [Action("TextMoved:")]
        [SuppressMessage(null, "IDE0051")]
        void TextMoved(NSTextField _) {}

        #region IBActions

        partial void Exit(NSButton sender)
        {
            sender.Window.Close();
        }

        partial void ForgotPassword(NSButton _)
        {
            Workspace.OpenUrl(Url.ForgotPassword);
        }

        partial void Signin(NSButton _)
        {
            AttemptSignin();
        }

        #endregion

        void AttemptSignin()
        {
            if (!AppleIDTextField.StringValue.IsValidEmail())
            {
                ShowInvalidEmailError();
                return;
            }

            Show2FAWindow();
        }

        void Show2FAWindow()
        {
            var windowController = new TwoFactorAuthWindowController();
            View.Window.BeginSheet(windowController.Window, _ => {});
        }

        #region INSTextFieldDelegate

        [Export("controlTextDidEndEditing:")]
        public void EditingEnded(NSNotification notification)
        {
            var textMovement = notification.UserInfo.ObjectForKey((NSString)"NSTextMovement");
            var textMovementType = (NSTextMovement)((NSNumber)textMovement).Int32Value;
            if (textMovementType == NSTextMovement.Return)
            {
                AttemptSignin();
            }
        }

        #endregion
    }
}
