using AppKit;
using Foundation;

namespace Balsamic.Views
{
    [Register("MessagePopover")]
    sealed class MessagePopover : NSPopover
    {
        string _message = "";
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
