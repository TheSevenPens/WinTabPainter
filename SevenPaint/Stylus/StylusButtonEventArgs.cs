namespace SevenPaint.Stylus
{
    public class StylusButtonEventArgs : EventArgs
    {
        public int ButtonId { get; }
        public bool IsPressed { get; }
        public string ButtonName { get; }

        public int ButtonState { get; }

        public StylusButtonEventArgs(int buttonId, bool isPressed, string buttonName, int buttonState)
        {
            ButtonId = buttonId;
            IsPressed = isPressed;
            ButtonName = buttonName;
            ButtonState = buttonState;
        }
    }
}
