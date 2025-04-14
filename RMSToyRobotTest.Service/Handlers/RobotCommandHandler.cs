using Ardalis.GuardClauses;
using RMSToyRobotTest.Service.Models;

namespace RMSToyRobotTest.Service.Handlers
{
    // Dont need runtime polymorphism, so no interface implemented
    public class RobotCommandHandler
    {
        private readonly Robot _robot;

        public RobotCommandHandler(Robot robot)
        {
            _robot = robot;
        }

        public string ExecuteCommand(string commandString)
        {
            Guard.Against.NullOrEmpty(
                commandString,
                message: "Parameter 'command cannot be null or empty'"
            );

            var command = ParseCommand(commandString);

            switch (command)
            {
                case Command.Place:
                    var (x, y, direction) = ParsePosition(commandString);
                    _robot.Place(x, y, direction);
                    break;
                case Command.Move:
                    _robot.Move();
                    break;
                case Command.Left:
                    _robot.RotateLeft();
                    break;
                case Command.Right:
                    _robot.RotateRight();
                    break;
                case Command.Report:
                    return Report();
            }
            return string.Empty;
        }

        private string Report() =>
            _robot.IsPlaced
                ? $"{_robot.Position.X}, {_robot.Position.Y}, {_robot.Facing}"
                : string.Empty;

        private Command ParseCommand(string commandString)
        {
            var parts = commandString.Split(' ');
            var commandType = parts[0].ToUpper();
            if (
                !Enum.TryParse<Command>(commandType, true, out var command)
                || !Enum.IsDefined(typeof(Command), command)
            )
                throw new ArgumentException($"Invalid command value: {commandString}");
            return command;
        }

        private (int X, int Y, Direction direction) ParsePosition(string commandString)
        {
            var commandArgs = commandString.Split(' ');

            if (commandArgs.Length != 2)
                throw new ArgumentException(
                    "Invalid PLACE command parameters. Valid format: PLACE X,Y,DIRECTION"
                );

            string[] positionArgs = commandArgs[1].Split(',');

            if (positionArgs.Length != 3)
                throw new ArgumentException(
                    "Invalid PLACE command parameters. Valid format: PLACE X,Y,DIRECTION"
                );

            if (
                !int.TryParse(positionArgs[0], out int x)
                || !int.TryParse(positionArgs[1], out int y)
            )
                throw new ArgumentException(
                    "Invalid location. X and Y must be a number. Example: PLACE 3,2,NORTH"
                );

            if (
                !Enum.TryParse(positionArgs[positionArgs.Length - 1], true, out Direction direction)
            )
                throw new ArgumentException(
                    "Invalid direction. Valid directions: NORTH|EAST|SOUTH|WEST"
                );

            return (x, y, direction);
        }
    }
}
