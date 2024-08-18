// References:
// https://github.com/DennisWacom/WintabControl/tree/master/WintabControl
// https://github.com/DennisWacom/InkPlatform/tree/master/WintabDN
// https://developer-docs.wacom.com/docs/icbt/windows/wintab/wintab-basics/
// https://www.nuget.org/packages/WacomSolutionPartner.WintabDotNet
// https://github.com/Wacom-Developer/wacom-device-kit-windows/tree/master/Wintab%20TiltTest

namespace WinTabPainter
{
    public partial class FormWinTabPainterApp
    {
        public struct SerPacket
        {
            public uint pkContext;
            public uint pkStatus;
            public uint pkTime;
            public uint pkChanged;
            public uint pkSerialNumber;
            public uint pkCursor;
            public uint pkButtons;
            public int pkX;
            public int pkY;
            public int pkZ;
            public uint pkNormalPressure;
            public uint pkTangentPressure;
            public int orAzimuth;
            public int orAltitude;
            public int orTwist;

            public SerPacket()
            {
                // do nothing
            }

            public SerPacket(WintabDN.Structs.WintabPacket pkt)
            {
                this.pkContext = pkt.pkContext;
                this.pkStatus = pkt.pkStatus;
                this.pkTime = pkt.pkTime;
                this.pkChanged = pkt.pkChanged;
                this.pkSerialNumber = pkt.pkSerialNumber;
                this.pkCursor = pkt.pkCursor;
                this.pkButtons = pkt.pkButtons;
                this.pkX = pkt.pkX;
                this.pkY = pkt.pkY;
                this.pkZ = pkt.pkZ;
                this.pkNormalPressure = pkt.pkNormalPressure;
                this.pkTangentPressure = pkt.pkTangentPressure;
                this.orAzimuth = pkt.pkOrientation.orAzimuth;
                this.orAltitude = pkt.pkOrientation.orAltitude;
                this.orTwist = pkt.pkOrientation.orTwist;
            }

            public WintabDN.Structs.WintabPacket ToPacket()
            {
                var p = new WintabDN.Structs.WintabPacket();
                p.pkContext = this.pkContext;
                p.pkStatus = this.pkStatus;
                p.pkTime = this.pkTime;
                p.pkChanged = this.pkChanged;
                p.pkSerialNumber = this.pkSerialNumber;
                p.pkCursor = this.pkCursor;
                p.pkButtons = this.pkButtons;
                p.pkX = this.pkX;
                p.pkY = this.pkY;
                p.pkZ = this.pkZ;
                p.pkNormalPressure = this.pkNormalPressure;
                p.pkTangentPressure = this.pkTangentPressure;
                p.pkOrientation.orAzimuth = this.orAzimuth;
                p.pkOrientation.orAltitude = this.orAltitude;
                p.pkOrientation.orTwist = this.orTwist;
                return p;
            }

        }
    }
}
