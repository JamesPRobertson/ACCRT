// Check if all imports are needed.
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

using Driver;

namespace Server {
   class TelemetryServer {
      TelemetryParser telemetry_source;

      const int PORT = 9000;
      const string IP_ADDRESS = "99.129.97.238";

      public TelemetryServer() {
         telemetry_source = new TelemetryParser();
      }

      public void ExecuteUDPServer() {
         // This address must be changed based on your system
         IPAddress broadcast = IPAddress.Parse(IP_ADDRESS);
         Socket broadcaster  = new Socket(broadcast.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
         IPEndPoint endpoint = new IPEndPoint(broadcast, PORT);
         Console.WriteLine($"Beginning UDP broadcast on: {endpoint}");

         while(true) {
            string string_data = "";
            foreach (string line in this.telemetry_source.GetDataToSend()) {
               string_data += line;
            }
            byte[] sendbuf = Encoding.ASCII.GetBytes(string_data);

            broadcaster.SendTo(sendbuf, endpoint);
            
            // 10 Hz
            Thread.Sleep(100);
         }
      }
   }
}
