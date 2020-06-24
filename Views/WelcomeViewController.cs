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
        internal NSWorkspace Workspace { get; private set; } = NSWorkspace.SharedWorkspace;

        static TwoFactorAuthWindowController Init2FAWindowController() => new TwoFactorAuthWindowController();
        readonly L2FAWindowController _lazy2FAWindowController = new L2FAWindowController(Init2FAWindowController);
        TwoFactorAuthWindowController TwoFactorAuthWindowController => _lazy2FAWindowController.Value;

        #region Constructors

        public WelcomeViewController(IntPtr handle) : base(handle)
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

        private void Initialize() {}

        #endregion

        internal new NSView View => base.View;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            AppIconImageView.Image = Image.AppLogo;
            AppIconImageView.ImageScaling = NSImageScale.ProportionallyUpOrDown;
            AppIconImageView.RefusesFirstResponder = true;

            SetupWelcomeTextField();
            SetupVersionTextField();
            SetupSigninHeaderTextField();
            SetupAppleIDTextField();
            SetupPasswordTextField();
            SetupForgotPasswordButton();
            SetupSigninButton();
        }

        void SetupWelcomeTextField()
        {
            WelcomeTextField.StringValue = NSBundle.MainBundle.GetName();
            WelcomeTextField.Font = Font.WelcomeTextField;
            WelcomeTextField.Alignment = NSTextAlignment.Center;
            WelcomeTextField.TextColor = NSColor.White;
        }

        void SetupVersionTextField()
        {
            VersionTextField.StringValue = String.AppBuildVersion;
            VersionTextField.Font = Font.VersionTextField;
            VersionTextField.Alignment = NSTextAlignment.Center;
            VersionTextField.TextColor = NSColor.FromWhite((nfloat)0.8, 1);
        }

        void SetupSigninHeaderTextField()
        {
            SigninHeaderTextField.StringValue = String.SigninInstructions;
            SigninHeaderTextField.Font = Font.SigninHeaderTextField;
            SigninHeaderTextField.Alignment = NSTextAlignment.Left;
            SigninHeaderTextField.TextColor = NSColor.White;
        }

        void SetupAppleIDTextField()
        {
            AppleIDTextField.Delegate = this;
            AppleIDTextField.PlaceholderString = String.Placeholder.AppleID;
            AppleIDTextField.Alignment = NSTextAlignment.Natural;
            AppleIDTextField.BezelStyle = NSTextFieldBezelStyle.Rounded;
            AppleIDTextField.Font = Font.AppleIDTextField;
            AppleIDTextField.Editable = true;
        }

        void SetupPasswordTextField()
        {
            PasswordTextField.Delegate = this;
            PasswordTextField.PlaceholderString = String.Placeholder.Password;
            PasswordTextField.Alignment = NSTextAlignment.Natural;
            PasswordTextField.BezelStyle = NSTextFieldBezelStyle.Rounded;
            PasswordTextField.Font = Font.PasswordTextField;
            PasswordTextField.Editable = true;
        }

        void SetupForgotPasswordButton()
        {
            ForgotPasswordButton.Title = String.ForgotPasswordButtonTitle;
            ForgotPasswordButton.Font = Font.ForgotPasswordButton;
            ForgotPasswordButton.BezelStyle = NSBezelStyle.Recessed;
            ForgotPasswordButton.SetButtonType(NSButtonType.MomentaryPushIn);
            ForgotPasswordButton.Bordered = false;
            ForgotPasswordButton.ContentTintColor = NSColor.SystemBlueColor;
        }

        void SetupSigninButton()
        {
            SigninButton.BezelStyle = NSBezelStyle.TexturedSquare;
            SigninButton.SetButtonType(NSButtonType.MomentaryPushIn);
            SigninButton.Bordered = true;
            SigninButton.Image = Image.GoForward;
            SigninButton.ImageScaling = NSImageScale.ProportionallyDown;
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
