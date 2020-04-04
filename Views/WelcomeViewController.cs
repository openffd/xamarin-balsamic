using AppKit;
using Foundation;
using ObjCRuntime;
using System.Diagnostics.CodeAnalysis;

namespace Balsamic.Views
{
    public partial class WelcomeViewController : NSViewController, INSTextFieldDelegate
    {
        private static MessagePopover InitializePopover() => new MessagePopover();
        private readonly System.Lazy<MessagePopover> _lazyPopover = new System.Lazy<MessagePopover>(InitializePopover);
        public MessagePopover Popover { get => _lazyPopover.Value; }

        public string StoryboardIdentifier => Class.ToString();

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

        [Export("closePopover")]
        [SuppressMessage(null, "IDE0051")]
        private void ClosePopover()
        {
            if (Popover.Shown)
            {
                Popover.Close();
            }
        }

        private void ShowPopover(string message, NSView positioningView)
        {
            Popover.Message = message;
            Popover.Show(positioningView.Bounds, positioningView, NSRectEdge.MaxXEdge);
        }

        private void AttemptSignin()
        {
            if (!AppleIDTextField.StringValue.IsValidEmail())
            {
                ClosePopover();

                var selector = new Selector("closePopover");
                NSObject.CancelPreviousPerformRequest(this, selector, this);
                ShowPopover(String.ErrorMessage.InvalidEmail.String(), AppleIDTextField);
                PerformSelector(selector, this, 2.5);
            }
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
