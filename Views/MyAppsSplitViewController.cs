using AppKit;
using Foundation;
using System;

namespace Balsamic.Views
{
    public partial class MyAppsSplitViewController : NSSplitViewController
    {
        #region Constructors

        public MyAppsSplitViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        [Export("initWithCoder:")]
        public MyAppsSplitViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        public MyAppsSplitViewController() : base("MyAppsSplitView", NSBundle.MainBundle)
        {
            Initialize();
        }

        void Initialize() {}

        #endregion
    }
}
