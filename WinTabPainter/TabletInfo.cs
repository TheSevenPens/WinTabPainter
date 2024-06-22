namespace WinTabPainter
{
    public class TabletInfo
    {
        public WintabDN.WintabAxis XAxis;
        public WintabDN.WintabAxis YAxis;
        public int MaxPressure = -1;
        public string DeviceName;

        public void Initialize()
        {
            this.XAxis = WintabDN.CWintabInfo.GetTabletAxis(WintabDN.EAxisDimension.AXIS_X);
            this.YAxis = WintabDN.CWintabInfo.GetTabletAxis(WintabDN.EAxisDimension.AXIS_Y);
            this.MaxPressure = WintabDN.CWintabInfo.GetMaxPressure();
            this.DeviceName = WintabDN.CWintabInfo.GetDeviceInfo();
        }
    }
}
