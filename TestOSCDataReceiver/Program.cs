namespace TestOSCDataReceiver {
    using Rug.Osc;
    using System;
    using System.Net;

    class Program {
        static void Main(string[] args) {
            IPAddress address = IPAddress.Parse("127.0.0.1");
            int port = 12345;

            using (OscReceiver receiver = new OscReceiver(address, port)) {
                receiver.Connect();

                while (true) {
                    OscPacket receivedPacket = receiver.Receive();

                    Console.WriteLine(receivedPacket);

                    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape) {
                        break;
                    }
                }
            }
        }
    }
}
