using AppKit;
using Foundation;
using ObjCRuntime;
using System.Diagnostics.CodeAnalysis;

namespace Balsamic.Views
{
    partial class WelcomeViewController
    {
        private static MessagePopover InitializePopover() => new MessagePopover();

        private readonly System.Lazy<MessagePopover> _lazyPopover = new System.Lazy<MessagePopover>(InitializePopover);

        private readonly Selector closePopover = new Selector("closePopover");

        public MessagePopover Popover { get => _lazyPopover.Value; }

        [Export("closePopover")]
        [SuppressMessage(null, "IDE0051")]
        public void ClosePopover()
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

        public void ShowInvalidEmailError()
        {
            AppKitFramework.NSBeep();

            ClosePopover();

            NSObject.CancelPreviousPerformRequest(this, closePopover, this);
            ShowPopover(String.ErrorMessage.InvalidEmail.String(), AppleIDTextField);
            PerformSelector(closePopover, this, 2.5);
        }
    }
}
