using AppKit;
using Foundation;

namespace Balsamic.Views
{
    [Register("MessageViewController")]
    internal sealed class MessageViewController : NSViewController
    {
        internal string Message { get; set; } = string.Empty;

        #region Constructors

        public MessageViewController(System.IntPtr handle) : base(handle)
        {
            Initialize();
        }

        [Export("initWithCoder:")]
        public MessageViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        public MessageViewController(string message = "") : base(typeof(MessageViewController).ToString(), NSBundle.MainBundle)
        {
            Message = message;
            Initialize();
        }

        private void Initialize() {}

        #endregion

        internal new NSView View
        {
            get => base.View;
            set => base.View = value;
        }

        public override void LoadView()
        {
            View = new NSView(new CoreGraphics.CGRect(0, 0, 200, 60));
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            CenteredTextField textField = new CenteredTextField(View.Bounds, Message);
            View.AddSubview(textField);
        }
    }
}
