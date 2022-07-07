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
      const string IP_ADDRESS = "192.168.1.113";

      public TelemetryServer() {
         telemetry_source = new TelemetryParser();
      }

      public void ExecuteUDPServer() {
         // This address must be changed based on your system
         IPAddress broadcast = IPAddress.Parse(IP_ADDRESS);
         Socket broadcaster  = new Socket(broadcast.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
         IPEndPoint local_endpoint = new IPEndPoint(broadcast, PORT);
         Console.WriteLine($"Beginning UDP broadcast on: {local_endpoint}");

         broadcaster.Bind(local_endpoint);

         // Attempt to see who's calling
         byte[] message = new byte[4096];
         
         IPEndPoint caller = new IPEndPoint(IPAddress.Any, 0);
         EndPoint remote_caller = (EndPoint)caller;

         // Note: this is blocking
         broadcaster.ReceiveFrom(message, ref remote_caller);
         Console.WriteLine($"Received {Encoding.ASCII.GetString(message)} from {remote_caller}");
         broadcaster.SendTo(Encoding.ASCII.GetBytes("Thanks!\n"), remote_caller);

         while(true) {
            string string_data = "";
            foreach (string line in this.telemetry_source.GetDataToSend()) {
               string_data += line;
            }
            byte[] sendbuf = Encoding.ASCII.GetBytes(string_data);

            broadcaster.SendTo(sendbuf, remote_caller);

            
            Console.Write("s");
            // 10 Hz
            Thread.Sleep(100);
         }
      }
   }
}
