using System;

namespace Balsamic
{
    internal static class View
    {
        internal static void Rounded(this AppKit.NSView view, double cornerRadius = 4)
        {
            view.WantsLayer = true;
            view.CanDrawSubviewsIntoLayer = true;
            view.Layer.MasksToBounds = true;
            view.Layer.CornerRadius = (nfloat)cornerRadius;
        }
    }
}
