using WintabDN;

namespace WinTabPainter
{
    public class TabletInfo
    {
        public WintabAxis XAxis;
        public WintabAxis YAxis;
        public int MaxPressure = -1;
        public string Device;

        public void Initialize()
        {
            this.XAxis = CWintabInfo.GetTabletAxis(EAxisDimension.AXIS_X);
            this.YAxis = CWintabInfo.GetTabletAxis(EAxisDimension.AXIS_Y);
            this.MaxPressure = CWintabInfo.GetMaxPressure();
            this.Device = CWintabInfo.GetDeviceInfo();
        }

    }
}
