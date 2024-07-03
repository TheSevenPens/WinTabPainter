using System;
using WintabDN;



namespace MyApp
{
    public enum ButtonPressStatus
    {
        NoPress,
        Down,
        Up
    }

    public enum ButtonType
    {
        Tip,
        LowerButton,
        UpperButton
    }

    public struct ButtonInfo
    {
        public readonly UInt32 PacketButtons ;
        public readonly UInt16 Id ;
        public readonly ButtonPressStatus PressStatus;
        public readonly ButtonType Type;

        public ButtonInfo(UInt32 pkt_button)
        {
            this.PacketButtons = pkt_button;
            this.Id = (UInt16)((pkt_button & 0x0000FFFF) >> 0);
            UInt16 press_status = (UInt16)((pkt_button & 0xFFFF0000) >> 16);

            if (press_status == 0)
            {
                this.PressStatus = ButtonPressStatus.NoPress;
            }
            else if (press_status == 1)
            {
                this.PressStatus = ButtonPressStatus.Up;
            }
            else if ( press_status == 2)
            {
                    this.PressStatus = ButtonPressStatus.Down;
            }
            else
            {
                    throw new System.ArgumentOutOfRangeException();
            }

            if (this.Id == 0)
            {
                this.Type = ButtonType.Tip;
            }
            else if (this.Id == 1)
            {
                this.Type = ButtonType.LowerButton;
            }
            else if (this.Id == 2)
            {
                this.Type = ButtonType.UpperButton;
            }
            else
            {
                throw new System.ArgumentOutOfRangeException();
            }

        }

        public override string ToString()
        {
            string s = string.Format("({0},{1})",this.Type, this.PressStatus);
            return s;
        }   

    }
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleWindow.QuickEditMode(false);
            System.Console.WriteLine("START WINTAB CONSOLE");
            var session = new TabletSession();
            System.Console.WriteLine("STARTING");
            session.Start();
            while (true)
            {
                uint num_pkts_received = 0;
                var pkts = session.wintab_data.GetDataPackets(50, true, ref num_pkts_received);
                if (num_pkts_received != 0)
                {
                    //System.Console.WriteLine("Packets received = {0}", num_pkts_received);
                    foreach (var pkt in pkts)
                    {
                        var button_info = new ButtonInfo(pkt.pkButtons);

                        if (button_info.PressStatus != 0)
                        {

                            Console.WriteLine("XY={0},{1}, SP={2}, NP={3}, Z={4} , B={5}, B2={6}", pkt.pkX, pkt.pkY, pkt.SPACING, pkt.pkNormalPressure, pkt.pkZ, button_info, button_info.PressStatus);
                        }
                        //Console.WriteLine("({0},{1} | {2}", pkt.pkX, pkt.pkY, pkt.pkButtons);
                    }
                    session.wintab_data.FlushDataPackets(50);

                }

            }
            session.Stop();

        }
    }
}