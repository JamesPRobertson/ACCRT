// Check if all imports are needed.
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

using Driver;

namespace Server {
   class TelemetryServer {
      TelemetryParser telemetry_source;

      public TelemetryServer() {
         telemetry_source = new TelemetryParser();
      }

      public void ExecuteUDPServer() {
         // This address must be changed based on your system
         IPAddress broadcast = IPAddress.Parse("192.168.1.113");
         Socket broadcaster = new Socket(broadcast.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

         while(true) {
            string massive_string = "";
            foreach (string line in this.telemetry_source.GetDataToSend()) {
               massive_string += line;
            }
            byte[] sendbuf = Encoding.ASCII.GetBytes(massive_string);
            IPEndPoint ep = new IPEndPoint(broadcast, 11000);

            broadcaster.SendTo(sendbuf, ep);
            
            // 10 Hz
            Thread.Sleep(100);
         }
      }
   }
}
