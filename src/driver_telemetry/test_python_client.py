import json
import socket

REMOTE_IP_ADDR = "localhost"
REMOTE_PORT    = 9000

LOCAL_PORT = 9001

def udp_testing():
    # This address must be changed based on your system

    server_address = (REMOTE_IP_ADDR, REMOTE_PORT)
    local_address = ('', LOCAL_PORT)
    bufferSize     = 4096

    client_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM,  socket.IPPROTO_UDP)
    client_socket.setsockopt(socket.SOL_SOCKET, socket.SO_RCVTIMEO, 1000)

    client_socket.bind(local_address)
    client_socket.sendto(b"I request data now", server_address)

    message_str = ""

    initial_message_str = client_socket.recvfrom(bufferSize)[0]
    print("Inital message: " + str(initial_message_str, encoding="ASCII"), "\n\n")

    while True:
        message_str = client_socket.recvfrom(bufferSize)[0]

        try:
            incoming_data = json.loads(message_str)

            physics_json  = incoming_data['physics_data']
            graphics_json = incoming_data['graphics_data']
            static_json   = incoming_data['static_data']

            #message_json = json.loads(str(message_str, encoding="ASCII"))
            print(f"----------------------------------------------\n")
            print(f"Current Packet: {physics_json['packetId']}")
            print(f"Temperatures:")
            print(f"   Air:      {physics_json['airTemp']} c")
            print(f"   Track:    {physics_json['roadTemp']} c")
            print(f"\n")
            
            print(f"speed:       {physics_json['speedKmh']} km/h")
            print(f"throttle:    {physics_json['gas']}")
            print(f"brake:       {physics_json['brake']}")
            print(f"RPM:         {physics_json['rpms']}")
            print(f"gear:        {physics_json['gear']}")
            print(f"fuel:        {physics_json['fuel']} L")
            print(f"-------------------------------")
            print(f"Lap Time:    {graphics_json['currentTime']}")
            print(f"Last Lap:    {graphics_json['lastTime']}")
            print(f"Best Lap:    {graphics_json['bestTime']}")
            print(f"")
            print(f"Current Track:  {static_json['track']}")
            print(f"\n")
        except Exception as e:
            message_txt = str(message_str, encoding="ASCII")
            print(f"Exception in message: {message_txt}")
            print(e)
            #print("RECV: " + str(message_str, encoding="ASCII"), "\n\n")
        

if __name__ == "__main__":
    udp_testing()
