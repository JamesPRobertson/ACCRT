// Check if all imports are needed.
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

using Driver;

namespace Server {
   class TelemetryServer {
      readonly TelemetryParser telemetry_source;

      public TelemetryServer() {
         telemetry_source = new TelemetryParser();
      }

      public void ExecuteUDPServer(string server_ip_address, int port) {
         IPAddress broadcast = IPAddress.Parse(server_ip_address);
         Socket broadcaster  = new Socket(broadcast.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
         IPEndPoint local_endpoint = new IPEndPoint(broadcast, port);
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
            string string_data = String.Join("\n", this.telemetry_source.GetTelemetryData());
            byte[] sendbuf = Encoding.ASCII.GetBytes(string_data);

            broadcaster.SendTo(sendbuf, remote_caller);
            
            // 20 Hz
            Thread.Sleep(50);
         }
      }
   }
}
