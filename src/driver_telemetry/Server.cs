// Check if all imports are needed.
using System.Net;
using System.Net.Sockets;
using System.Text;

using Driver;

namespace Server {
   class TelemetryServer {
      const int BUFFER_SIZE = 1024;
      const int MS_DELAY    = 50; // 20 Hz
      const string JSON_DELIMITER = "|||";

      const int DEFAULT_PORT = 9000;

      readonly TelemetryParser telemetry_source;

      public TelemetryServer() {
         telemetry_source = new TelemetryParser();
      }

      public void ExecuteUDPServer(string[] args) {
         int port = DEFAULT_PORT;
         IPAddress broadcast = IPAddress.Any;
         
         if (args.Length >= 2) {
            broadcast = IPAddress.Parse(args[0]);
            port = Int32.Parse(args[1]);
         }
         
         Socket broadcaster  = new Socket(broadcast.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
         IPEndPoint local_endpoint = new IPEndPoint(broadcast, port);
         Console.WriteLine($"Beginning UDP broadcast on: {local_endpoint}");

         // For local testing
         // broadcaster.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

         broadcaster.Bind(local_endpoint);
         
         // SendTo requries a reference to an EndPoint specifically so we do this.
         // Using IPAddress.Any should allow us to receive a message from any IP
         EndPoint remote_caller = new IPEndPoint(IPAddress.Any, 0);

         // Attempt to see who's calling
         // Note: an engineer must connect before data will start sending
         byte[] message = new byte[BUFFER_SIZE];
         broadcaster.ReceiveFrom(message, ref remote_caller);
         Console.WriteLine($"Received '{Encoding.ASCII.GetString(message)}' from {remote_caller}");
         broadcaster.SendTo(Encoding.ASCII.GetBytes($"init data transmission: {MS_DELAY} ms\n"), remote_caller);

         while(true) {
            byte[] sendbuf = Encoding.ASCII.GetBytes(this.telemetry_source.GetJSONTelemetryData());

            broadcaster.SendTo(sendbuf, remote_caller);
            
            Thread.Sleep(MS_DELAY);
         }

         broadcaster.Dispose();
      }
   }
}
