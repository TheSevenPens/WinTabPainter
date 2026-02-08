namespace SevenPaint.Stylus
{
    public class StylusButtonEventArgs : EventArgs
    {
        public int ButtonId { get; }
        public bool IsPressed { get; }
        public string ButtonName { get; }

        public StylusButtonEventArgs(int buttonId, bool isPressed, string buttonName)
        {
            ButtonId = buttonId;
            IsPressed = isPressed;
            ButtonName = buttonName;
        }
    }
}
