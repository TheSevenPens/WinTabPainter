namespace SevenUtils.WinTab;
public class TabletInfo
{
    public WinTabDN.Structs.WintabAxis XAxis {  private set; get ;}
    public WinTabDN.Structs.WintabAxis YAxis { private set; get; }
    public int MaxPressure {  private set; get ;}
    public string Name { private set; get; }

    public bool TiltSupport { private set; get; }


    public void Initialize()
    {
        this.XAxis = WinTabDN.CWintabInfo.GetTabletAxis(WinTabDN.Enums.EAxisDimension.AXIS_X);
        this.YAxis = WinTabDN.CWintabInfo.GetTabletAxis(WinTabDN.Enums.EAxisDimension.AXIS_Y);
        this.MaxPressure = WinTabDN.CWintabInfo.GetMaxPressure();
        this.Name = WinTabDN.CWintabInfo.GetDeviceInfo();

        bool b;
        var arr = WinTabDN.CWintabInfo.GetDeviceOrientation(out b);
        this.TiltSupport = b;
    }
}
