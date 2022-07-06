import socket

IP_ADDR = "192.168.1.113"
PORT    = 9000

def udp_testing():
    # This address must be changed based on your system
    server_address = (IP_ADDR, PORT)
    bufferSize     = 4096

    client_socket = socket.socket(family=socket.AF_INET, type=socket.SOCK_DGRAM)
    client_socket.bind(server_address)

    msgFromServer = ""

    while True:
        msgFromServer = client_socket.recvfrom(bufferSize)[0]
        print(str(msgFromServer, encoding="ASCII"), "\n\n")

if __name__ == "__main__":
    udp_testing()