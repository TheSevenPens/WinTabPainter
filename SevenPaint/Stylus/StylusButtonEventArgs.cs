namespace SevenPaint.Stylus
{
    public class StylusButtonEventArgs : EventArgs
    {
        public int ButtonId { get; }
        public bool IsPressed { get; }
        public string ButtonName { get; }

        public SevenLib.WinTab.Utils.StylusButtonState ButtonState { get; }

        public StylusButtonEventArgs(int buttonId, bool isPressed, string buttonName, SevenLib.WinTab.Utils.StylusButtonState buttonState)
        {
            ButtonId = buttonId;
            IsPressed = isPressed;
            ButtonName = buttonName;
            ButtonState = buttonState;
        }
    }
}
