namespace WinTab.Utils;

public class TabletInfo
{
    public WinTab.Structs.WintabAxis XAxis {  private set; get ;}
    public WinTab.Structs.WintabAxis YAxis { private set; get; }
    public int MaxPressure {  private set; get ;}
    public string Name { private set; get; }

    public bool TiltSupport { private set; get; }


    public void Initialize()
    {
        this.XAxis = WinTab.CWintabInfo.GetTabletAxis(WinTab.Enums.EAxisDimension.AXIS_X);
        this.YAxis = WinTab.CWintabInfo.GetTabletAxis(WinTab.Enums.EAxisDimension.AXIS_Y);
        this.MaxPressure = WinTab.CWintabInfo.GetMaxPressure();
        this.Name = WinTab.CWintabInfo.GetDeviceInfo();

        bool b;
        var arr = WinTab.CWintabInfo.GetDeviceOrientation(out b);
        this.TiltSupport = b;
    }
}
