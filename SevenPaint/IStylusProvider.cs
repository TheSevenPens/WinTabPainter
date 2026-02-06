namespace SevenPaint
{
    public interface IStylusProvider
    {
        event Action<DrawInputArgs> InputDown;
        event Action<DrawInputArgs> InputMove;
        event Action<DrawInputArgs> InputUp;

        bool IsActive { get; set; }
        void Open();
        void Close();
    }
}
