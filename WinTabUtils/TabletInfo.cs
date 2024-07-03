﻿namespace WinTabUtils;
public class TabletInfo
{
    public WintabDN.WintabAxis XAxis {  private set; get ;}
    public WintabDN.WintabAxis YAxis { private set; get; }
    public int MaxPressure {  private set; get ;}
    public string Name { private set; get; }

    public void Initialize()
    {
        this.XAxis = WintabDN.CWintabInfo.GetTabletAxis(WintabDN.EAxisDimension.AXIS_X);
        this.YAxis = WintabDN.CWintabInfo.GetTabletAxis(WintabDN.EAxisDimension.AXIS_Y);
        this.MaxPressure = WintabDN.CWintabInfo.GetMaxPressure();
        this.Name = WintabDN.CWintabInfo.GetDeviceInfo();
    }
}