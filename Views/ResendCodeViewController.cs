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
            SetupResendCodeBoxSubviews();
            SetupUsePhoneNumberBoxSubviews();
            SetupMoreOptionsBoxSubviews();
        }

        private void SetupResendCodeBoxSubviews()
        {
            ResendCodeBox.AddGestureRecognizer(new NSClickGestureRecognizer(this, Selector.ResendCode_));

            ResendCodeImageView.Image = Image.ResendCode;
            ResendCodeImageView.RefusesFirstResponder = true;

            SetupHeaderTextField(ResendCodeHeaderTextField, String.ResendCode.Header);
            SetupDescriptionTextField(ResendCodeDescriptionTextField, String.ResendCode.Description);
        }

        private void SetupUsePhoneNumberBoxSubviews()
        {
            UsePhoneNumberBox.AddGestureRecognizer(new NSClickGestureRecognizer(this, Selector.UsePhoneNumber_));

            UsePhoneNumberImageView.Image = Image.UsePhoneNumber;
            UsePhoneNumberImageView.RefusesFirstResponder = true;

            SetupHeaderTextField(UsePhoneNumberHeaderTextField, String.UsePhoneNumber.Header);
            SetupDescriptionTextField(UsePhoneNumberDescriptionTextField, String.UsePhoneNumber.Description);
        }

        private void SetupMoreOptionsBoxSubviews()
        {
            MoreOptionsBox.AddGestureRecognizer(new NSClickGestureRecognizer(this, Selector.MoreOptions_));

            MoreOptionsImageView.Image = Image.MoreOptions;
            MoreOptionsImageView.RefusesFirstResponder = true;

            SetupHeaderTextField(MoreOptionsHeaderTextField, String.MoreOptions.Header);
            SetupDescriptionTextField(MoreOptionsDescriptionTextField, String.MoreOptions.Description);
        }

        private void SetupHeaderTextField(NSTextField headerTextField, string stringValue)
        {
            headerTextField.StringValue = stringValue;
            headerTextField.RefusesFirstResponder = true;
            headerTextField.Font = Font.Header;
            headerTextField.TextColor = Color.Header;
        }

        private void SetupDescriptionTextField(NSTextField descriptionTextField, string stringValue)
        {
            descriptionTextField.StringValue = stringValue;
            descriptionTextField.RefusesFirstResponder = true;
            descriptionTextField.Font = Font.Description;
            descriptionTextField.TextColor = Color.Description;
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
