import json
import socket

IP_ADDR = "99.129.97.238"
PORT    = 9000

def udp_testing():
    # This address must be changed based on your system

    server_address = (IP_ADDR, PORT)
    local_address = ('', PORT)
    bufferSize     = 4096

    client_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM,  socket.IPPROTO_UDP)
    client_socket.bind(local_address)
    client_socket.sendto(b"I request data now", server_address)

    message_str = ""

    while True:
        message_str = client_socket.recvfrom(bufferSize)[0]
        message_json = json.loads(message_str)

        print(f"----------------------------------------------\n")
        print(f"Current Packet: {message_json['packetId']}")
        print(f"Temperatures:")
        print(f"   Air:      {message_json['airTemp']} c")
        print(f"   Track:    {message_json['roadTemp']} c")
        print(f"\n")
        
        print(f"speed:       {message_json['speedKmh']} km/h")
        print(f"throttle:    {message_json['gas']}")
        print(f"brake:       {message_json['brake']}")
        print(f"RPM:         {message_json['rpms']}")
        print(f"gear:        {message_json['gear']}")
        print(f"fuel:        {message_json['fuel']} L")
        print(f"-------------------------------\n")
        print(f"\n")
        #print("RECV: " + str(message_str, encoding="ASCII"), "\n\n")
        

if __name__ == "__main__":
    udp_testing()