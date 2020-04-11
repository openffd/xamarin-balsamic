using AppKit;
using Foundation;
using ObjCRuntime;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Balsamic.Views
{
    sealed partial class ResendCodeViewController : NSViewController, INSGestureRecognizerDelegate
    {
        #region Constructors

        public ResendCodeViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        [Export("initWithCoder:")]
        public ResendCodeViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        public ResendCodeViewController() : base("ResendCodeView", NSBundle.MainBundle)
        {
            Initialize();
        }

        void Initialize() {}

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            SetupResendCodeBox();
            SetupUsePhoneNumberBox();
            SetupMoreOptionsBox();
        }

        private void SetupResendCodeBox()
        {
            ResendCodeBox.AddGestureRecognizer(new NSClickGestureRecognizer(this, new Selector("ResendCode:")));
            ResendCodeImageView.RefusesFirstResponder = true;
            ResendCodeHeaderTextField.RefusesFirstResponder = true;
            ResendCodeDescriptionTextField.RefusesFirstResponder = true;
        }

        private void SetupUsePhoneNumberBox()
        {
            UsePhoneNumberBox.AddGestureRecognizer(new NSClickGestureRecognizer(this, new Selector("UsePhoneNumber:")));
            UsePhoneNumberImageView.RefusesFirstResponder = true;
            UsePhoneNumberHeaderTextField.RefusesFirstResponder = true;
            UsePhoneNumberDescriptionTextField.RefusesFirstResponder = true;
        }

        private void SetupMoreOptionsBox()
        {
            MoreOptionsBox.AddGestureRecognizer(new NSClickGestureRecognizer(this, new Selector("MoreOptions:")));
            MoreOptionsImageView.RefusesFirstResponder = true;
            MoreOptionsHeaderTextField.RefusesFirstResponder = true;
            MoreOptionsDescriptionTextField.RefusesFirstResponder = true;
        }

        #endregion

        public new ResendCodeView View => (ResendCodeView)base.View;

        [Export("ResendCode:")]
        [SuppressMessage(null, "IDE0051")]
        private void ResendCode(NSClickGestureRecognizer recognizer)
        {
            if (recognizer.State != NSGestureRecognizerState.Ended)
                return;
        }

        [Export("UsePhoneNumber:")]
        [SuppressMessage(null, "IDE0051")]
        private void UsePhoneNumber(NSClickGestureRecognizer recognizer)
        {
            if (recognizer.State != NSGestureRecognizerState.Ended)
                return;
        }

        [Export("MoreOptions:")]
        [SuppressMessage(null, "IDE0051")]
        private void MoreOptions(NSClickGestureRecognizer recognizer)
        {
            if (recognizer.State != NSGestureRecognizerState.Ended)
                return;
        }

        [Export("gestureRecognizer:shouldBeRequiredToFailByGestureRecognizer:")]
        public bool ShouldRequireFailure(NSGestureRecognizer gestureRecognizer, NSGestureRecognizer otherGestureRecognizer)
        {
            return false;
        }
    }
}
