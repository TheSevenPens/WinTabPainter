namespace SevenPaint.Stylus
{
    public interface IStylusProvider
    {
        event Action<Stylus.StylusEventArgs> InputDown;
        event Action<Stylus.StylusEventArgs> InputMove;
        event Action<Stylus.StylusEventArgs> InputUp;

        bool IsActive { get; set; }
        void Open();
        void Close();
    }
}
