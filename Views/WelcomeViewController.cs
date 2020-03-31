using Foundation;
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

        public WelcomeViewController() : base("WelcomeView", NSBundle.MainBundle)
        {
            Initialize();
        }

        void Initialize() {}

        #endregion

        public new WelcomeView View => (WelcomeView)base.View;
    }
}
