using AppKit;
using Foundation;
using ObjCRuntime;
using System.Diagnostics.CodeAnalysis;

namespace Balsamic.Views
{
    public partial class WelcomeViewController : NSViewController, INSTextFieldDelegate
    {
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

            PasswordTextField.Delegate = this;
        }

        [Action("TextMoved:")]
        [SuppressMessage("CodeQuality", "IDE0051")]
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

        #endregion

        #region INSTextFieldDelegate

        [Export("controlTextDidEndEditing:")]
        public void EditingEnded(NSNotification notification)
        {
            var textMovement = notification.UserInfo.ObjectForKey((NSString)"NSTextMovement");
            var textMovementType = (NSTextMovement)((NSNumber)textMovement).Int32Value;
            if (textMovementType == NSTextMovement.Return)
            {
                System.Console.WriteLine("EditingEnded");
            }
        }

        #endregion
    }
}
