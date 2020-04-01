﻿using Foundation;
using AppKit;

namespace Balsamic.Views
{
    public partial class WelcomeViewController : NSViewController
    {
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

        void Initialize() {}

        #endregion

        public new NSView View => base.View;

        public string StoryboardIdentifier => Class.ToString();

        #region IBActions

        partial void Exit(NSButton sender)
        {
            sender.Window.Close();
        }

        partial void ForgotPassword(NSButton _)
        {
            NSUrl url = new NSUrl($"{"https://"}iforgot.apple.com/password/verify/appleid");
            NSWorkspace.SharedWorkspace.OpenUrl(url);
        }

        #endregion
    }
}
