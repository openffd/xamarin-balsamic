using AppKit;
using Foundation;

namespace Balsamic.Views
{
    [Register("MessagePopover")]
    public class MessagePopover : NSPopover
    {
        private string _message = "";
        public string Message
        {
            get => _message;
            set {
                _message = value;
                ((MessageViewController)ContentViewController).Message = value;
            }
        }

        public MessagePopover(string message = "")
        {
            _message = message;
            Animates = true;
            Behavior = NSPopoverBehavior.Transient;
            ContentViewController = new MessageViewController(message);
        }
    }
}
