using RMSToyRobotTest.Service.Handlers;
using RMSToyRobotTest.Service.Models;
using Shouldly;

namespace RMSToyRobotTest.Tests.ConsoleAppTests
{
    [TestClass]
    public class ToyRobotAppTests
    {
        private Robot _robot;
        private RobotCommandHandler _commandHandler;
        private ToyRobotApp _app;
        private List<string> _output;

        [TestInitialize]
        public void Setup()
        {
            _robot = new Robot(5);
            _commandHandler = new RobotCommandHandler(_robot);
            _app = new ToyRobotApp(_commandHandler);
            _output = new List<string>();
        }

        [TestMethod]
        public void GivenValidCommands_WhenRun_ShouldDisplayHelpText()
        {
            // Arrange
            var inputs = new Queue<string>(["EXIT"]);
            string? ReadInput() => inputs.Count > 0 ? inputs.Dequeue() : null;
            void WriteOutput(string output) => _output.Add(output);

            // Act
            _app.Run(ReadInput, WriteOutput);

            // Assert
            var expectedHelpText = new string[]
            {
                "Toy Robot Movement",
                "Commands:",
                $"- {Command.Place} X,Y,DIRECTION (e.g., PLACE 0,0,NORTH)",
                $"- {Command.Move}",
                $"- {Command.Left}",
                $"- {Command.Right}",
                $"- {Command.Report}",
                $"- {Command.Exit}",
            };

            foreach (var text in expectedHelpText)
            {
                _output.ShouldContain(text);
            }
        }

        [TestMethod]
        public void GivenValidCommands_WhenRun_ShouldSucceed()
        {
            var inputs = new Queue<string>(["PLACE 0,0,NORTH", "REPORT", "EXIT"]);
            string? ReadInput() => inputs.Count > 0 ? inputs.Dequeue() : null;
            void WriteOutput(string output) => _output.Add(output);

            // Act
            _app.Run(ReadInput, WriteOutput);

            _output.ShouldContain("0, 0, North");
        }

        [TestMethod]
        public void GivenInvalidCommand_WhenRun_ShouldHandleInvalidCommand()
        {
            // Arrange
            var inputs = new Queue<string>(["INVALID", "EXIT"]);
            string? ReadInput() => inputs.Count > 0 ? inputs.Dequeue() : null;
            void WriteOutput(string output) => _output.Add(output);

            // Act
            _app.Run(ReadInput, WriteOutput);

            // Assert
            _output.ShouldContain("Invalid command value: INVALID");
        }

        [TestMethod]
        public void GivenExitCommand_WhenRun_ShouldExit()
        {
            // Arrange
            var inputs = new Queue<string>(["EXIT"]);
            string? ReadInput() => inputs.Count > 0 ? inputs.Dequeue() : null;
            void WriteOutput(string output) => _output.Add(output);

            // Act
            _app.Run(ReadInput, WriteOutput);

            // Assert
            _output.ShouldContain("Toy Robot Movement");
            _output.ShouldNotContain("Invalid command value");
        }
    }
}
