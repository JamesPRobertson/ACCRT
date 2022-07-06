// A C# Program for Server
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server {
   class TelemetryServer {
      public static void ExecuteTCPServer() {
         // We use 0.0.0.0 to broadcast on all available devices
         IPAddress ipAddr = IPAddress.Parse("0.0.0.0");
         IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 1001);

         Socket listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

         try {
            listener.Bind(localEndPoint);
            listener.Listen(10);

            while (true) {
               Console.WriteLine("Waiting connection ... ");
               Socket clientSocket = listener.Accept();
               Console.WriteLine($"  Connected to: {clientSocket.RemoteEndPoint}");

               // Data buffer
               byte[] bytes = new Byte[1024];
               string data = null;

               while (true) {
                  int numByte = clientSocket.Receive(bytes);
                  
                  data += Encoding.ASCII.GetString(bytes, 0, numByte);
                              
                  if (data.IndexOf("!") > -1)
                  break;
               }

               Console.WriteLine($"Text received -> {data} ");

               byte[] message = Encoding.ASCII.GetBytes("Test Server");
               clientSocket.Send(message);

               clientSocket.Shutdown(SocketShutdown.Both);
               clientSocket.Close();
            }
         }
         catch (Exception e) {
            Console.WriteLine(e.ToString());
         }
      }

      public static void ExecuteUDPServer() {
         IPAddress broadcast = IPAddress.Parse("0.0.0.0");
         //Socket broadcaster = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
         Socket broadcaster = new Socket(broadcast.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

         byte[] sendbuf = Encoding.ASCII.GetBytes("Hellooooooooo");
         IPEndPoint ep = new IPEndPoint(broadcast, 11000);

         broadcaster.SendTo(sendbuf, ep);

         Console.WriteLine("Message sent to the broadcast address");
      }
   }
}
