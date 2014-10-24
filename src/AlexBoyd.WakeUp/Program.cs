using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

public class Program
{
    public static void Main(String[] args)
    {
        if(args.Length != 1)
        {
            Console.WriteLine("Usage: wakeup.exe [mac address]");
            System.Environment.Exit(-1);
        }
            
        // strip non-hex characters
        var macAddress = new Regex("[^a-fA-F0-9]").Replace(args[0], "");  

        // build magic packet
        string hexMagicPacket = String.Concat(Enumerable.Repeat("FF", 6)) +
                                String.Concat(Enumerable.Repeat(macAddress, 16));
        
        // hex string to byte array
        byte[] magicPacket = Enumerable.Range(0, hexMagicPacket.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(hexMagicPacket.Substring(x, 2), 16))
                     .ToArray();

        // send packet to broadcast address at port 9
        UdpClient udpClient = new UdpClient();
        udpClient.Connect(IPAddress.Broadcast, 9);
        udpClient.Send(magicPacket, magicPacket.Length);

        Console.WriteLine("Magic Packet Sent");
    }
}