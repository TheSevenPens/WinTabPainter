namespace SevenLib.WinTab.Tablet;

public class TabletInfo
{
    public SevenLib.WinTab.Structs.WintabAxis XAxis {  private set; get ;}
    public SevenLib.WinTab.Structs.WintabAxis YAxis { private set; get; }
    public int MaxPressure {  private set; get ;}
    public string Name { private set; get; }

    public bool TiltSupport { private set; get; }


    public void Initialize()
     {
         this.XAxis = SevenLib.WinTab.CWintabDevice.GetTabletAxis(SevenLib.WinTab.Enums.EAxisDimension.AXIS_X);
         this.YAxis = SevenLib.WinTab.CWintabDevice.GetTabletAxis(SevenLib.WinTab.Enums.EAxisDimension.AXIS_Y);
         this.MaxPressure = SevenLib.WinTab.CWintabDevice.GetMaxPressure();
         this.Name = SevenLib.WinTab.CWintabDevice.GetDeviceInfo();

         bool b;
         var arr = SevenLib.WinTab.CWintabDevice.GetDeviceOrientation(out b);
         this.TiltSupport = b;
     }
}
