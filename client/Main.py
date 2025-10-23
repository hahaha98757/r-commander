import socket
import threading
import os

HOST = "127.0.0.1"
PORT = 5000

def main():
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as it:
        it.connect((HOST, PORT))
        print(f"Connected to server at {HOST}:{PORT}")

        # 수신 스레드 시작
        recv_thread = threading.Thread(target=receive_messages, args=(it,), daemon=True)
        recv_thread.start()

        # 메인 스레드는 송신 담당
        while True:
            message = input()  # 콘솔 입력
            if message.lower() == "exit":
                print("Shutting down...")
                it.close()
                exit(0)
            it.sendall(message.encode('utf-8'))

def receive_messages(sock: socket.socket):
    while True:
        # noinspection PyBroadException
        try:
            data = sock.recv(1024)
            if not data:
                print("Server closed the connection.")
                break
            print("\n[server] " + data.decode('utf-8'))
        except Exception:
            print("Connection was closed by the server.")
            os._exit(0)


if __name__ == "__main__":
    main()