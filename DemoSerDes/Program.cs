using System;
using System.Text.Json;

namespace MyApp
{

    internal class Program
    {
        static void Main(string[] args)
        {

            var options = new JsonSerializerOptions();
            options.IncludeFields = true;
            options.WriteIndented = true;

            var pkt1 = new WintabDN.Structs.WintabPacket();

            string content1 = JsonSerializer.Serialize(pkt1, options);


            var pkt2 = JsonSerializer.Deserialize<WintabDN.Structs.WintabPacket>(content1, options);
        }
    }
}