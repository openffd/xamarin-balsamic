using Foundation;
using AppKit;

namespace Balsamic.Views
{
    public partial class WelcomeView : NSView
    {
        #region Constructors

        public WelcomeView(System.IntPtr handle) : base(handle)
        {
            Initialize();
        }

        [Export("initWithCoder:")]
        public WelcomeView(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        void Initialize()
        {
        }

        #endregion
    }
}
