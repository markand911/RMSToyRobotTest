using RMSToyRobotTest.Service.Handlers;

namespace RMSToyRobotTest
{
    public class ToyRobotApp
    {
        private readonly RobotCommandHandler _commandHandler;

        public ToyRobotApp(RobotCommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
        }

        public void Run(Func<string?> readInput, Action<string> writeOutput)
        {
            string[] helpText =
            [
                "Toy Robot Movement",
                "Commands:",
                $"- {Command.Place} X,Y,DIRECTION (e.g., PLACE 0,0,NORTH)",
                $"- {Command.Move}",
                $"- {Command.Left}",
                $"- {Command.Right}",
                $"- {Command.Report}",
                $"- {Command.Exit}",
            ];

            foreach (var text in helpText)
            {
                writeOutput(text);
            }

            while (true)
            {
                writeOutput("\nEnter command: ");
                var command = readInput()?.Trim();

                if (
                    string.Equals(
                        command,
                        Command.Exit.ToString(),
                        StringComparison.OrdinalIgnoreCase
                    )
                )
                    break;

                try
                {
                    var result = _commandHandler.ExecuteCommand(command);
                    if (!string.IsNullOrEmpty(result))
                    {
                        writeOutput(result);
                    }
                }
                catch (Exception ex)
                {
                    writeOutput(ex.Message);
                }
            }
        }
    }
}
