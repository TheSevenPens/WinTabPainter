using System;
using WintabDN;



namespace MyApp
{

    internal class Program
    {
        static WinTabUtils.WinTabSession session;
        static void Main(string[] args)
        {
            ConsoleWindow.QuickEditMode(false);
            System.Console.WriteLine("START WINTAB CONSOLE");
            session = new WinTabUtils.WinTabSession();
            System.Console.WriteLine("STARTING");
            session.Start();
            session.Data.SetWTPacketEventHandler(WinTabPacketHandler);

            while (true)
            {
                uint num_pkts_received = 0;
                var pkts = session.Data.GetDataPackets(50, true, ref num_pkts_received);
                if (num_pkts_received != 0)
                {
                    //System.Console.WriteLine("Packets received = {0}", num_pkts_received);
                    foreach (var pkt in pkts)
                    {
                        var button_info = new WinTabUtils.PenButtonInfo(pkt.pkButtons);

                        if (button_info.PressStatus != 0)
                        {

                            Console.WriteLine("XY={0},{1}, SP={2}, NP={3}, Z={4} , B={5}, B2={6}", pkt.pkX, pkt.pkY, pkt.SPACING, pkt.pkNormalPressure, pkt.pkZ, button_info, button_info.PressStatus);
                        }
                        //Console.WriteLine("({0},{1} | {2}", pkt.pkX, pkt.pkY, pkt.pkButtons);
                    }
                    session.Data.FlushDataPackets(50);

                }

            }
            session.Stop();

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

    }
}