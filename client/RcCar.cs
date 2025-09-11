namespace client;

public static class RcCar {
    public static void Drive(int speed) {
        Console.WriteLine($"Drive with speed: {speed}");
    }
    
    public static void Turn(int angle, int speed) {
        Console.WriteLine($"Turn with angle: {angle} and speed: {speed}");
    }
    
    public static void Stop() {
        Console.WriteLine("Stopped");
    }
}