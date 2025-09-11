using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("TCP Server is starting...");

var server = new TcpListener(IPAddress.Any, 5000);
server.Start();
Console.WriteLine("Server started.");

var client = server.AcceptTcpClient();
Console.WriteLine("Client connected.");

var stream = client.GetStream();

// 여기에 스레드를 사용하여 클라이언트에게 메시지를 보냄.
new Thread(() => {
    while (true) {
        var msg = Console.ReadLine()!;
        Send(msg);
    }
    // ReSharper disable once FunctionNeverReturns
}).Start();

while (true) {
    // 클라이언트로 부터 메시지를 받음
    var buffer = new byte[1024];
    var byteCount = stream.Read(buffer, 0, buffer.Length);
    var msg = Encoding.UTF8.GetString(buffer, 0, byteCount);
    ProcessMsg(msg);
}

void Send(string msg) {
    var data = Encoding.UTF8.GetBytes(msg);
    stream.Write(data, 0, data.Length);
}

void ProcessMsg(string msg) {
    var command = msg.Split(";")[0];
    switch (command) {
        case "print": {
            var text = msg.Split(";")[1];
            Console.WriteLine(text);
            break;
        }
        case "exit": {
            Console.WriteLine("Exiting...");
            Environment.Exit(0);
            break;
        }
        default:
            Console.WriteLine($"Unknown command: {command}");
            break;
    }
}