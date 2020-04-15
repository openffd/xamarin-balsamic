using AppKit;
using Foundation;
using ObjCRuntime;
using System.Diagnostics.CodeAnalysis;

namespace Balsamic.Views
{
    partial class WelcomeViewController
    {
        readonly Selector closePopover = new Selector("closePopover");

        static MessagePopover InitializePopover() => new MessagePopover();
        readonly System.Lazy<MessagePopover> _lazyPopover = new System.Lazy<MessagePopover>(InitializePopover);
        MessagePopover MessagePopover => _lazyPopover.Value;

        [Export("closePopover")]
        [SuppressMessage(null, "IDE0051")]
        void ClosePopover()
        {
            if (!MessagePopover.Shown)
                return;

            MessagePopover.Close();
        }

        void ShowPopover(string message, NSView positioningView)
        {
            MessagePopover.Message = message;
            MessagePopover.Show(positioningView.Bounds, positioningView, NSRectEdge.MaxXEdge);
        }

        void ShowInvalidEmailError()
        {
            AppKitFramework.NSBeep();

            ClosePopover();

            NSObject.CancelPreviousPerformRequest(this, closePopover, this);
            ShowPopover(Balsamic.String.ErrorMessage.InvalidEmail.String(), AppleIDTextField);
            PerformSelector(closePopover, this, 2.5);
        }
    }
}
