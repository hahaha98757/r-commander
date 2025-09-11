using System.Net.Sockets;
using System.Text;
using client;

const string ip = "127.0.0.1";

Console.WriteLine("TCP Client is starting...");

var client = new TcpClient(ip, 5000);
Console.WriteLine("Connected to server.");

var stream = client.GetStream();
while (true) {
    var buffer = new byte[1024];
    var byteCount = stream.Read(buffer, 0, buffer.Length);
    var msg = Encoding.UTF8.GetString(buffer, 0, byteCount);
    ProcessMsg(msg);
}

void ProcessMsg(string msg) {
    var command = msg.Split(";")[0];
    switch (command) {
        case "signal": {
            var signal = msg.Split(";")[1].Split(":")[0];
            var args = msg.Split(";")[1].Split(":")[1..];
            ProcessSignal(signal, args);
            break;
        }
        case "print": {
            var text = msg.Split(";")[1];
            Console.WriteLine(text);
            break;
        }
        case "exit": {
            Console.WriteLine("Exiting...");
            Send("exit;");
            Environment.Exit(0);
            break;
        }
        default:
            Send("print;Unknown command received");
            break;
        
    }
}

void ProcessSignal(string signal, string[] args) {
    switch (signal) {
        case "drive": {
            var speed = int.Parse(args[0]);
            RcCar.Drive(speed);
            Send($"print;Driving with speed {speed}");
            break;
        }
        case "turn": {
            var angle = int.Parse(args[0]);
            var speed = int.Parse(args[1]);
            RcCar.Turn(angle, speed);
            Send($"print;Turning with angle {angle} and speed {speed}");
            break;
        }
        case "stop": {
            RcCar.Stop();
            Send("print;Stopped");
            break;
        }
        default:
            Send("print;Unknown signal received");
            break;
    }
}

void Send(string msg) {
    var data = Encoding.UTF8.GetBytes(msg);
    stream.Write(data, 0, data.Length);
}