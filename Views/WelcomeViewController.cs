using AppKit;
using Foundation;
using ObjCRuntime;
using System.Diagnostics.CodeAnalysis;

namespace Balsamic.Views
{
    public partial class WelcomeViewController : NSViewController
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

            AppleIDTextField.Target = this;
            AppleIDTextField.Action = new Selector("ReturnPressed:");

            PasswordTextField.Target = this;
            PasswordTextField.Action = new Selector("ReturnPressed:");
        }

        [Action("ReturnPressed:")]
        [SuppressMessage("CodeQuality", "IDE0051")]
        private void ReturnPressed(NSTextField _)
        {
            System.Console.WriteLine("ReturnPressed");
        }

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
    }
}
