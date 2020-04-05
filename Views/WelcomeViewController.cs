using AppKit;
using Foundation;
using L2FAWindowController = System.Lazy<Balsamic.Views.TwoFactorAuthWindowController>;
using ObjCRuntime;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Balsamic.Views
{
    public partial class WelcomeViewController : NSViewController, INSTextFieldDelegate
    {
        private static TwoFactorAuthWindowController Init2FAWindowController() => new TwoFactorAuthWindowController();

        public string StoryboardIdentifier => Class.ToString();
        
        private readonly L2FAWindowController _lazy2FAWindowController = new L2FAWindowController(Init2FAWindowController);
        private TwoFactorAuthWindowController TwoFactorAuthWindowController => _lazy2FAWindowController.Value;

        public NSWorkspace Workspace { get; private set; }

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

        void Initialize()
        {
            Workspace = NSWorkspace.SharedWorkspace;
        }

        #endregion

        public new NSView View => base.View;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            AppleIDTextField.Delegate = this;
            PasswordTextField.Delegate = this;
        }

        [Action("TextMoved:")]
        [SuppressMessage(null, "IDE0051")]
        private void TextMoved(NSTextField _) {}

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

        private void AttemptSignin()
        {
            if (!AppleIDTextField.StringValue.IsValidEmail())
            {
                ShowInvalidEmailError();
            }

            Show2FAWindow();
        }

        private void Show2FAWindow()
        {
            View.Window.BeginSheet(TwoFactorAuthWindowController.Window, (response) => {});
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
