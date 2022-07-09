// Check if all imports are needed.
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

using Driver;

namespace Server {
   class TelemetryServer {
      const int BUFFER_SIZE = 1024;

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
         
         // SendTo requries a reference to an EndPoint specifically so we do this.
         // Using IPAddress.Any should allow us to receive a message from any IP
         EndPoint remote_caller = new IPEndPoint(IPAddress.Any, 0);


         // Attempt to see who's calling
         // Note: an engineer must connect before data will start sending
         byte[] message = new byte[BUFFER_SIZE];
         broadcaster.ReceiveFrom(message, ref remote_caller);
         Console.WriteLine($"Received '{Encoding.ASCII.GetString(message)}' from {remote_caller}");
         broadcaster.SendTo(Encoding.ASCII.GetBytes("Thanks!\n"), remote_caller);

         while(true) {
            string string_data = String.Join("\n", this.telemetry_source.GetTelemetryData());
            byte[] sendbuf = Encoding.ASCII.GetBytes(string_data);

            broadcaster.SendTo(sendbuf, remote_caller);
            
            // 20 Hz
            Thread.Sleep(50);
         }
         broadcaster.Dispose();
      }
   }
}
