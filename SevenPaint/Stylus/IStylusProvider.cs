namespace SevenPaint.Stylus
{
    public interface IStylusProvider
    {
        event Action<Stylus.DrawInputArgs> InputDown;
        event Action<Stylus.DrawInputArgs> InputMove;
        event Action<Stylus.DrawInputArgs> InputUp;

        bool IsActive { get; set; }
        void Open();
        void Close();
    }
}
