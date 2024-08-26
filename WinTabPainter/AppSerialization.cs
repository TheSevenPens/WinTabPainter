// References:
// https://github.com/DennisWacom/WintabControl/tree/master/WintabControl
// https://github.com/DennisWacom/InkPlatform/tree/master/WintabDN
// https://developer-docs.wacom.com/docs/icbt/windows/wintab/wintab-basics/
// https://www.nuget.org/packages/WacomSolutionPartner.WintabDotNet
// https://github.com/Wacom-Developer/wacom-device-kit-windows/tree/master/Wintab%20TiltTest

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Windows.Forms;
using WintabDN.Structs;

namespace WinTabPainter
{
    public partial class FormWinTabPainterApp : Form
    {
        int max_rec_packets = 200 * 60 * 2; // allocate enough for 2 minute at 200 reports per second

        List<WintabDN.Structs.WintabPacket> recorded_packets;

        RecStatusEnum RecStat = RecStatusEnum.NotRecording;

        RecStatusEnum old_rec_stat;
        int? old_rec_count;

        private void ToggleRecordingPackets()
        {
            if (this.RecStat == RecStatusEnum.NotRecording)
            {
                StartRecording();
            }
            else
            {
                StopRecording();
            }
        }

        private void StopRecording()
        {
            this.RecStat = RecStatusEnum.NotRecording;
            this.buttonRec.BackColor = System.Drawing.Color.White;
            this.UpdateRecStatus();
        }

        private void StartRecording()
        {
            this.RecStat = RecStatusEnum.Recording;
            this.buttonRec.BackColor = System.Drawing.Color.Red;
            this.UpdateRecStatus();
        }

        private void RecordPacket(WintabPacket wintab_pkt)
        {
            if (this.recorded_packets.Count >= this.max_rec_packets)
            {
                this.RecStat = RecStatusEnum.NotRecording;
            }
            else
            {
                this.recorded_packets.Add(wintab_pkt);
            }
            this.UpdateRecStatus();
        }

        private void SavePackets()
        {
            var sfd = new SaveFileDialog();
            sfd.FileName = "Untitled.WinTab.json";
            sfd.DefaultExt = ".WinTab.json";
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var dr = sfd.ShowDialog();

            if (dr != DialogResult.OK)
            {
                return;
            }

            string mydocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var pr = new PacketRecording(this.recorded_packets);

            pr.Save(sfd.FileName);
        }


        private void LoadPackets()
        {
            var ofd = new OpenFileDialog();
            ofd.DefaultExt = ".WinTab.json";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var dr = ofd.ShowDialog();

            if (dr != DialogResult.OK)
            {
                return;
            }
            var options = new JsonSerializerOptions();
            options.IncludeFields = true;
            options.WriteIndented = true;

            var loaded_recording = PacketRecording.FromFile(ofd.FileName);
            this.recorded_packets = new List<WintabPacket>();
            foreach (var packet in loaded_recording.Packets)
            {
                this.recorded_packets.Add(packet.ToPacket());
                this.UpdateRecStatus();
            }
        }

        private void ClearRecording()
        {
            if (this.RecStat == RecStatusEnum.Recording)
            {
                this.RecStat = RecStatusEnum.NotRecording;
                this.UpdateRecStatus();
            }

            this.recorded_packets.Clear();
            this.UpdateRecStatus();
        }


        private void ReplayPackets()
        {
            if (this.RecStat == RecStatusEnum.Recording)
            {
                // do nothing - app is in the middle of recording
                return;
            }

            if (this.recorded_packets.Count < 1)
            {
                // do nothing - there is nothing to replay
                return;
            }

            this.EraseCanvas();
            foreach (var packet in this.recorded_packets)
            {
                var paint_data = new Painting.PaintData(packet, this.tabsession.TabletInfo, paint_settings, Screen_loc_to_canvas_loc);

                HandlePainting(paint_data);
                this.old_paintdata = paint_data;
            }
        }



        private void UpdateRecStatus()
        {
            if (HelperMethods.UpdatesOld(this.recorded_packets.Count, this.old_rec_count))
            {
                this.label_RecCount.Text = this.recorded_packets.Count.ToString();
                this.old_rec_count = this.recorded_packets.Count;

            }

            if (HelperMethods.UpdatesOld(this.RecStat, this.old_rec_stat))
            {
                this.buttonRec.Text = this.RecStat.ToString();
                this.old_rec_stat = this.RecStat;

            }

        }

    }


}