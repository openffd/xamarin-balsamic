using Foundation;
using AppKit;

namespace Balsamic.Views
{
    public partial class WelcomeViewController : NSViewController
    {
        public string StoryboardIdentifier => Class.ToString();

        public new NSView View => base.View;

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

        #region IBActions

        partial void Exit(NSButton sender)
        {
            sender.Window.Close();
        }

        partial void ForgotPassword(NSButton _)
        {
            Workspace.OpenUrl(NSUrl.ForgotPassword);
        }

        #endregion
    }
}
