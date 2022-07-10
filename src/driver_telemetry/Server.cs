// Check if all imports are needed.
using System.Net;
using System.Net.Sockets;
using System.Text;

using Driver;

namespace Server
{
    class TelemetryServer
    {
        const int BUFFER_SIZE = 1024;
        const int MS_DELAY = 50; // 20 Hz
        const int CONNECTED_CLIENT_TIMEOUT_MS = 5000;

        const int DEFAULT_PORT = 9000;

        readonly int port = DEFAULT_PORT;
        readonly IPAddress broadcast_address = IPAddress.Any;
        readonly Socket broadcaster;
        readonly IPEndPoint local_endpoint;

        private SocketAsyncEventArgs connectionListnerArgs;
        private byte[] connectionListenerBuffer;

        readonly TelemetryParser telemetry_source;

        readonly Dictionary<EndPoint, long> connected_clients = new();

        public TelemetryServer(string[] args)
        {
            telemetry_source = new TelemetryParser();

            if (args.Length >= 2)
            {
                this.broadcast_address = IPAddress.Parse(args[0]);
                this.port = Int32.Parse(args[1]);
            }

            this.broadcaster = new Socket(this.broadcast_address.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            this.local_endpoint = new IPEndPoint(this.broadcast_address, this.port);

            connectionListenerBuffer = new byte[BUFFER_SIZE];
            this.connectionListnerArgs = BuildConnectionListener();
        }

        ~TelemetryServer()
        {
            broadcaster.Dispose();
            connectionListnerArgs.Dispose();
        }

        public void ExecuteUDPServer()
        {
            Console.WriteLine($"Beginning UDP broadcast on: {local_endpoint}");

            this.broadcaster.Bind(this.local_endpoint);

            this.broadcaster.ReceiveFromAsync(connectionListnerArgs);

            while (true)
            {
                if (connected_clients.Count == 0)
                {
                    Thread.Sleep(MS_DELAY);
                }
                else
                {
                    byte[] sendbuf = Encoding.ASCII.GetBytes(this.telemetry_source.GetJSONTelemetryData());
                    long start_time_millis = CurrentTimeMillis();

                    foreach ((EndPoint client, long lastHeartbeatMillis) in connected_clients)
                    {
                        if (lastHeartbeatMillis < (start_time_millis - CONNECTED_CLIENT_TIMEOUT_MS))
                        {
                            Console.WriteLine($"Heartbeat timeout for {client} after {start_time_millis - CONNECTED_CLIENT_TIMEOUT_MS}ms");
                            connected_clients.Remove(client);
                            continue;
                        }
                        Console.WriteLine($"Sending to {client}");
                        this.broadcaster.SendTo(sendbuf, client);
                    }

                    Console.WriteLine($"Sleeping for {Math.Max(start_time_millis + MS_DELAY - CurrentTimeMillis(), 0)}ms");
                    Thread.Sleep((int)Math.Max(start_time_millis + MS_DELAY - CurrentTimeMillis(), 0));
                }
            }
        }

        private SocketAsyncEventArgs BuildConnectionListener()
        {
            SocketAsyncEventArgs newConnectionListner = new();
            connectionListenerBuffer = new byte[BUFFER_SIZE];

            newConnectionListner.Completed += new(ConnectionHandler);
            newConnectionListner.SetBuffer(connectionListenerBuffer, 0, BUFFER_SIZE);
            newConnectionListner.RemoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

            return newConnectionListner;
        }

        void ConnectionHandler(object? sender, SocketAsyncEventArgs e)
        {
            long connection_request_time_ms = CurrentTimeMillis();

            if (e.Buffer == null || e.RemoteEndPoint == null)
            {
                throw new NullReferenceException();
            }
            Console.WriteLine($"Received '{Encoding.ASCII.GetString(e.Buffer)}' from {e.RemoteEndPoint} at {connection_request_time_ms}ms");

            if (e.SocketError != SocketError.Success)
            {
                // A host is no longer connected. Will be removed from connected_clients after heartbeat timeout.
                Console.WriteLine($"Error {e.SocketError}");
            }
            else
            {
                Console.WriteLine($"Adding {e.RemoteEndPoint}");
                connected_clients[e.RemoteEndPoint] = connection_request_time_ms;
            }
            e.Dispose();
            this.connectionListnerArgs = BuildConnectionListener();
            this.broadcaster.ReceiveFromAsync(connectionListnerArgs);
        }

        private static long CurrentTimeMillis()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }
    }
}
