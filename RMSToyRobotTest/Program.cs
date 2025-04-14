using RMSToyRobotTest.Service.Handlers;
using RMSToyRobotTest.Service.Models;

namespace RMSToyRobotTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var robot = new Robot(5);
            var commandHandler = new RobotCommandHandler(robot);
            var app = new ToyRobotApp(commandHandler);

            app.Run(Console.ReadLine, Console.WriteLine);
        }
    }
}
