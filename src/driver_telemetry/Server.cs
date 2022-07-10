// Check if all imports are needed.
using System.Net;
using System.Net.Sockets;
using System.Text;

using Driver;

namespace Server {
    class TelemetryServer {
        const int BUFFER_SIZE = 1024;
        const int MS_DELAY = 50; // 20 Hz
        const int CONNECTED_CLIENT_TIMEOUT_MS = 5000;

        const int DEFAULT_PORT = 9000;

        readonly int port = DEFAULT_PORT;
        readonly IPAddress broadcast_address = IPAddress.Any;
        readonly Socket broadcaster;
        readonly IPEndPoint local_endpoint;

        private SocketAsyncEventArgs connection_listner_args;

        readonly TelemetryParser telemetry_source;

        readonly Dictionary<EndPoint, long> connected_clients = new();

        public TelemetryServer(string[] args) {
            telemetry_source = new TelemetryParser();

            if (args.Length >= 2) {
                broadcast_address = IPAddress.Parse(args[0]);
                port = Int32.Parse(args[1]);
            }

            broadcaster = new Socket(broadcast_address.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            local_endpoint = new IPEndPoint(broadcast_address, port);

            connection_listner_args = BuildConnectionListener();
        }

        ~TelemetryServer() {
            broadcaster.Dispose();
            connection_listner_args.Dispose();
        }

        public void ExecuteUDPServer() {
            Console.WriteLine($"Beginning UDP broadcast on: {local_endpoint}\nPress 'Q' to exit.");

            broadcaster.Bind(local_endpoint);

            broadcaster.ReceiveFromAsync(connection_listner_args);

            do {
                while (!Console.KeyAvailable) {
                    if (connected_clients.Count == 0) {
                        Thread.Sleep(MS_DELAY);
                    } else {
                        byte[] sendbuf = Encoding.ASCII.GetBytes(telemetry_source.GetJSONTelemetryData());
                        long start_time_millis = CurrentTimeMillis();

                        foreach ((EndPoint client, long lastHeartbeatMillis) in connected_clients) {
                            if (lastHeartbeatMillis < (start_time_millis - CONNECTED_CLIENT_TIMEOUT_MS)) {
                                Console.WriteLine($"Heartbeat timeout for {client} after {start_time_millis - CONNECTED_CLIENT_TIMEOUT_MS}ms");
                                connected_clients.Remove(client);
                                continue;
                            }

                            broadcaster.SendTo(sendbuf, client);
                        }

                        Thread.Sleep((int)Math.Max(start_time_millis + MS_DELAY - CurrentTimeMillis(), 0));
                    }
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Q);

            Console.WriteLine("Q key pressed, exiting.");
        }

        private SocketAsyncEventArgs BuildConnectionListener() {
            SocketAsyncEventArgs new_connection_listner = new();
            byte[]connection_listener_buffer = new byte[BUFFER_SIZE];

            new_connection_listner.Completed += new(ConnectionHandler);
            new_connection_listner.SetBuffer(connection_listener_buffer, 0, BUFFER_SIZE);
            new_connection_listner.RemoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

            return new_connection_listner;
        }

        private void ConnectionHandler(object? sender, SocketAsyncEventArgs e) {
            long connection_request_time_ms = CurrentTimeMillis();

            if (e.Buffer == null || e.RemoteEndPoint == null) {
                throw new NullReferenceException();
            }
            Console.WriteLine($"Received '{Encoding.ASCII.GetString(e.Buffer)}' from {e.RemoteEndPoint} at {connection_request_time_ms}ms");

            if (e.SocketError != SocketError.Success) {
                Console.WriteLine($"Error {e.SocketError}");
            } else {
                Console.WriteLine($"Adding {e.RemoteEndPoint}");
                connected_clients[e.RemoteEndPoint] = connection_request_time_ms;
            }
            e.Dispose();
            connection_listner_args = BuildConnectionListener();
            broadcaster.ReceiveFromAsync(connection_listner_args);
        }

        private static long CurrentTimeMillis() {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }
    }
}
