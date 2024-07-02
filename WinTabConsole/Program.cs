using System;
using WintabDN;



namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleWindow.QuickEditMode(true);
            System.Console.WriteLine("START WINTAB CONSOLE");
            var f = new TabletSession();
            System.Console.WriteLine("STARTING");
            f.Start();
            while (true)
            {
                uint num_pkts_received = 0;
                var pkts = f.wintab_data.GetDataPackets(50, true, ref num_pkts_received);
                if (num_pkts_received != 0)
                {
                    System.Console.WriteLine("Packets received = {0}", num_pkts_received);
                    foreach (var pkt in pkts)
                    {
                        Console.WriteLine("({0},{1}",pkt.pkX, pkt.pkY);
                        //Console.WriteLine("({0},{1} | {2}", pkt.pkX, pkt.pkY, pkt.pkButtons);
                    }
                    f.wintab_data.FlushDataPackets(50);

                }

            }
            f.Stop();

        }
    }
}