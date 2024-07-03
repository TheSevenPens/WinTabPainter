﻿using System;
using WintabDN;



namespace MyApp
{

    internal class Program
    {
        static WinTabUtils.TabletSession session;
        static void Main(string[] args)
        {
            ConsoleWindow.QuickEditMode(false);
            session = new WinTabUtils.TabletSession();
            session.PacketHandler = PacketHandler;
            session.Open(WinTabUtils.TabletContextType.System);

            while (true)
            {
                uint num_pkts_received = 0;
                var pkts = session.Data.GetDataPackets(50, true, ref num_pkts_received);
                if (num_pkts_received != 0)
                {
                    //System.Console.WriteLine("Packets received = {0}", num_pkts_received);
                    foreach (var pkt in pkts)
                    {
                        var button_info = new WinTabUtils.PenButtonPressChange(pkt.pkButtons);

                        if (button_info.Change != 0)
                        {

                            Console.WriteLine("XY={0},{1}, SP={2}, NP={3}, Z={4} , B={5}, B2={6}", pkt.pkX, pkt.pkY, pkt.SPACING, pkt.pkNormalPressure, pkt.pkZ, button_info, button_info.Change);
                        }
                        //Console.WriteLine("({0},{1} | {2}", pkt.pkX, pkt.pkY, pkt.pkButtons);
                    }
                    session.Data.FlushDataPackets(50);

                }

            }
            session.Close();

        }

        private static void WinTabPacketHandler(Object sender, WintabDN.MessageReceivedEventArgs args)
        {

            uint pktId = (uint)args.Message.WParam;
            var wintab_pkt = session.Data.GetDataPacket((uint)args.Message.LParam, pktId);

            if (wintab_pkt.pkContext == session.Context.HCtx)
            {
                Console.WriteLine("Packet");
                // collect all the information we need to start painting
            }
        }

        private static void PacketHandler( WintabDN.WintabPacket pkt)
        {
            Console.WriteLine("Packet");
        }

    }
}