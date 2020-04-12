using AppKit;
using Foundation;
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

        #endregion

        public new ResendCodeView View => (ResendCodeView)base.View;

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            SetupResendCodeBox();
            SetupUsePhoneNumberBox();
            SetupMoreOptionsBox();
        }

        private void SetupResendCodeBox()
        {
            ResendCodeBox.AddGestureRecognizer(new NSClickGestureRecognizer(this, Selector.ResendCode_));

            ResendCodeImageView.Image = Image.ResendCode;
            ResendCodeImageView.RefusesFirstResponder = true;

            ResendCodeHeaderTextField.StringValue = String.ResendCode.Header;
            ResendCodeHeaderTextField.RefusesFirstResponder = true;

            ResendCodeDescriptionTextField.StringValue = String.ResendCode.Description;
            ResendCodeDescriptionTextField.RefusesFirstResponder = true;
        }

        private void SetupUsePhoneNumberBox()
        {
            UsePhoneNumberBox.AddGestureRecognizer(new NSClickGestureRecognizer(this, Selector.UsePhoneNumber_));

            UsePhoneNumberImageView.Image = Image.UsePhoneNumber;
            UsePhoneNumberImageView.RefusesFirstResponder = true;

            UsePhoneNumberHeaderTextField.StringValue = String.UsePhoneNumber.Header;
            UsePhoneNumberHeaderTextField.RefusesFirstResponder = true;


            UsePhoneNumberDescriptionTextField.RefusesFirstResponder = true;
        }

        private void SetupMoreOptionsBox()
        {
            MoreOptionsBox.AddGestureRecognizer(new NSClickGestureRecognizer(this, Selector.MoreOptions_));

            MoreOptionsImageView.Image = Image.MoreOptions;
            MoreOptionsImageView.RefusesFirstResponder = true;

            MoreOptionsHeaderTextField.StringValue = String.MoreOptions.Header;
            MoreOptionsHeaderTextField.RefusesFirstResponder = true;

            MoreOptionsDescriptionTextField.StringValue = String.MoreOptions.Description;
            MoreOptionsDescriptionTextField.RefusesFirstResponder = true;
        }

        [Export("ResendCode:")]
        [SuppressMessage(null, "IDE0051")]
        private void ResendCode(NSClickGestureRecognizer recognizer)
        {
            if (recognizer.State != NSGestureRecognizerState.Ended)
                return;

            Console.WriteLine("ResendCode:");
        }

        [Export("UsePhoneNumber:")]
        [SuppressMessage(null, "IDE0051")]
        private void UsePhoneNumber(NSClickGestureRecognizer recognizer)
        {
            if (recognizer.State != NSGestureRecognizerState.Ended)
                return;

            Console.WriteLine("UsePhoneNumber:");
        }

        [Export("MoreOptions:")]
        [SuppressMessage(null, "IDE0051")]
        private void MoreOptions(NSClickGestureRecognizer recognizer)
        {
            if (recognizer.State != NSGestureRecognizerState.Ended)
                return;

            Console.WriteLine("MoreOptions:");
        }

        [Export("gestureRecognizer:shouldBeRequiredToFailByGestureRecognizer:")]
        public bool ShouldRequireFailure(NSGestureRecognizer gestureRecognizer, NSGestureRecognizer otherGestureRecognizer)
        {
            return false;
        }
    }
}
