using AppKit;
using CoreGraphics;
using System;

namespace Balsamic.Views
{
    [Foundation.Register("CustomToolbarItem")]
    internal sealed class CustomToolbarItem : NSToolbarItem
    {
        public nfloat CustomHeight { get; set; }
        public nfloat CustomWidth { get; set; }

        public override CGSize MinSize
        {
            get => new CGSize(CustomWidth, CustomHeight);
            set => CustomWidth = value.Width;
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }
    }
}
