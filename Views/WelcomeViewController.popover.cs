using AppKit;
using Foundation;
using ObjCRuntime;
using System.Diagnostics.CodeAnalysis;

namespace Balsamic.Views
{
    internal partial class WelcomeViewController
    {
        private readonly Selector closePopover = new Selector("closePopover");

        private static MessagePopover InitializePopover()
        {
            return new MessagePopover();
        }

        private readonly System.Lazy<MessagePopover> _lazyPopover = new System.Lazy<MessagePopover>(InitializePopover);

        private MessagePopover MessagePopover => _lazyPopover.Value;

        [Export("closePopover")]
        [SuppressMessage(null, "IDE0051")]
        private void ClosePopover()
        {
            if (MessagePopover.Shown)
                MessagePopover.Close();
        }

        private void ShowPopover(string message, NSView positioningView)
        {
            MessagePopover.Message = message;
            MessagePopover.Show(positioningView.Bounds, positioningView, NSRectEdge.MaxXEdge);
        }

        private void ShowInvalidEmailError()
        {
            AppKitFramework.NSBeep();

            ClosePopover();

            CancelPreviousPerformRequest(this, closePopover, this);
            ShowPopover(Balsamic.String.ErrorMessage.InvalidEmail.String(), AppleIDTextField);
            PerformSelector(closePopover, this, 2.5);
        }
    }
}
