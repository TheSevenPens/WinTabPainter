using System;


using System.Collections.Generic;
using System.Text.Json;
using System.Globalization;

namespace WinTabPainter
{
    public partial class FormWinTabPainterApp
    {
        public class PacketRecording
        {
            private int numPackets;
            private List<SerPacket> packets;


            public string App;
            public string AppVer;
            public string SaveDate;
            public int NumPackets { get => numPackets; set => numPackets = value; }
            public List<SerPacket> Packets { get => packets; set => packets = value; }

            public PacketRecording(List<SevenLib.WinTab.Structs.WintabPacket> pkts)
            {
                this.App = "WinTabPainter";
                this.AppVer = "1.0";
                this.SaveDate = DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture);
                this.packets = new List<SerPacket>();
                foreach (var p in pkts)
                {
                    this.packets.Add(new SerPacket(p));
                }
                this.numPackets = this.packets.Count;
            }

            public PacketRecording()
            {
                this.App = "WinTabPainter";
                this.AppVer = "1.0";
                this.SaveDate = DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture);
                this.numPackets =0;
            }

                static public PacketRecording FromFile(string filename)
            {
                var options = new JsonSerializerOptions();
                options.IncludeFields = true;
                options.WriteIndented = true;


                var JsonStr = System.IO.File.ReadAllText(filename);
                var loaded_recording = JsonSerializer.Deserialize<PacketRecording>(JsonStr, options);

                return loaded_recording;
            }

            public void Save(string filename)
            {
                var options = new JsonSerializerOptions();
                options.IncludeFields = true;
                options.WriteIndented = true;

                string content = JsonSerializer.Serialize(this, options);
                System.IO.File.WriteAllText(filename, content);
            }
        }
    }
}
