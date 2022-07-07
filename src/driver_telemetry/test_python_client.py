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

    msgFromServer = ""

    while True:
        print("PREPARING TO HEAR BACK :3")
        msgFromServer = client_socket.recvfrom(bufferSize)[0]
        #client_socket.sendto(b"give me packets", server_address)
        print("RECV: " + str(msgFromServer, encoding="ASCII"), "\n\n")

if __name__ == "__main__":
    udp_testing()