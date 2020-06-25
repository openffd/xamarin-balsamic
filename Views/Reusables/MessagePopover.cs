using AppKit;
using Foundation;

namespace Balsamic.Views
{
    [Register("MessagePopover")]
    internal sealed class MessagePopover : NSPopover
    {
        private string _message = string.Empty;

        internal string Message
        {
            get => _message;
            set {
                _message = value;
                ((MessageViewController)ContentViewController).Message = value;
            }
        }

        internal MessagePopover(string message = "")
        {
            _message = message;
            Animates = true;
            Behavior = NSPopoverBehavior.Transient;
            ContentViewController = new MessageViewController(message);
        }
    }
}
