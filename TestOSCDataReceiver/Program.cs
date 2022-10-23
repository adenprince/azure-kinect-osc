namespace TestOSCDataReceiver {
    using Rug.Osc;
    using System;
    using System.Net;

    class Program {
        static void Main(string address = "127.0.0.1", int port = 12345) {
            if (!TryOscReceiverConnect(address, port, out OscReceiver receiver)) {
                receiver?.Dispose();
                WaitForKeyInput();
                return;
            }

            try {
                while (true) {
                    OscPacket receivedPacket = receiver.Receive();

                    Console.WriteLine(receivedPacket);

                    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape) {
                        break;
                    }
                }
            }
            finally {
                receiver?.Dispose();
            }
        }

        public static bool TryOscReceiverConnect(string address, int port, out OscReceiver receiver) {
            receiver = null;

            try {
                IPAddress ipAddress = IPAddress.Parse(address);
                receiver = new OscReceiver(ipAddress, port);
                receiver.Connect();
                return true;
            }
            catch (Exception e) {
                Console.WriteLine($"Failed to connect to OSC receiver with exception message: {e.Message}");
                return false;
            }
        }

        public static void WaitForKeyInput() {
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    }
}
