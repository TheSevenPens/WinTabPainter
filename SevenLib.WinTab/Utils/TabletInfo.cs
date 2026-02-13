namespace SevenLib.WinTab.Utils;

public class TabletInfo
{
    public SevenLib.WinTab.Structs.WintabAxis XAxis {  private set; get ;}
    public SevenLib.WinTab.Structs.WintabAxis YAxis { private set; get; }
    public int MaxPressure {  private set; get ;}
    public string Name { private set; get; }

    public bool TiltSupport { private set; get; }


    public void Initialize()
    {
        this.XAxis = SevenLib.WinTab.CWintabInfo.GetTabletAxis(SevenLib.WinTab.Enums.EAxisDimension.AXIS_X);
        this.YAxis = SevenLib.WinTab.CWintabInfo.GetTabletAxis(SevenLib.WinTab.Enums.EAxisDimension.AXIS_Y);
        this.MaxPressure = SevenLib.WinTab.CWintabInfo.GetMaxPressure();
        this.Name = SevenLib.WinTab.CWintabInfo.GetDeviceInfo();

        bool b;
        var arr = SevenLib.WinTab.CWintabInfo.GetDeviceOrientation(out b);
        this.TiltSupport = b;
    }
}
