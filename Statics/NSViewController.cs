using AppKit;
using System.Linq;

namespace Balsamic
{
    internal static class ViewController
    {
        internal static bool HasChildViewController(this NSViewController viewController)
        {
            return viewController.ChildViewControllers.Count() > 0;
        }

        internal static void RemoveFirstChildViewController(this NSViewController viewController)
        {
            if (!viewController.HasChildViewController())
                return;

            viewController.RemoveChildViewController(0);
            viewController.View.Subviews.First().RemoveFromSuperview();
        }
    }
}
