using RMSToyRobotTest.Service.Handlers;
using RMSToyRobotTest.Service.Models;
using Shouldly;

namespace RMSToyRobotTest.Tests.ServiceTests.HandlerTests
{
    [TestClass]
    public class RobotCommandHandlerTests
    {
        private Robot _robot;
        private RobotCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _robot = new Robot(5); // Assuming a 5x5 grid
            _handler = new RobotCommandHandler(_robot);
        }

        [TestMethod]
        public void GivenValidPlaceCommand_ShouldPlaceRobot()
        {
            // Act
            _handler.ExecuteCommand($"{Command.Place} 2,3,EAST");

            // Assert
            _robot.Position.ShouldNotBeNull();
            _robot.Position.X.ShouldBe(2);
            _robot.Position.Y.ShouldBe(3);
            _robot.Facing.ShouldBe(Direction.East);
            _robot.IsPlaced.ShouldBeTrue();
        }

        [TestMethod]
        [DataRow(
            "Place",
            "Invalid PLACE command parameters. Valid format: PLACE X,Y,DIRECTION",
            DisplayName = "GivenNoPositionParameter_WhenExecuteCommand_ShouldThrowError"
        )]
        [DataRow(
            "Place 2,",
            "Invalid PLACE command parameters. Valid format: PLACE X,Y,DIRECTION",
            DisplayName = "GivenIncompletePositionParameter_WhenExecuteCommand_ShouldThrowError"
        )]
        [DataRow(
            "Place two,3,East",
            "Invalid location. X and Y must be a number. Example: PLACE 3,2,NORTH",
            DisplayName = "GivenIncorrectXParameter_WhenExecute_ShouldThrowError"
        )]
        [DataRow(
            "Place 2,three,East",
            "Invalid location. X and Y must be a number. Example: PLACE 3,2,NORTH",
            DisplayName = "GivenIncorrectYParameter_WhenExecute_ShouldThrowError"
        )]
        [DataRow(
            "Place 2,3,Eastern",
            "Invalid direction. Valid directions: NORTH|EAST|SOUTH|WEST",
            DisplayName = "GivenIncorrectFacingParameter_WhenExecute_ShouldThrowError"
        )]
        [DataRow(
            "Place 2,3,",
            "Invalid direction. Valid directions: NORTH|EAST|SOUTH|WEST",
            DisplayName = "GivenMissingFacingParameter_WhenExecuteCommand_ShouldThrowError"
        )]
        public void GivenInvalidPlaceCommand_ShouldThrowArgumentException(
            string command,
            string errorMessage
        )
        {
            // Act & Assert
            Should
                .Throw<ArgumentException>(() => _handler.ExecuteCommand(command))
                .Message.ShouldBe(errorMessage);
        }

        [TestMethod]
        public void GivenValidPosition_WhenExecuteMoveCommand_ShouldMoveRobot()
        {
            // Arrange
            _handler.ExecuteCommand($"{Command.Place} 1,1,EAST");

            // Act
            _handler.ExecuteCommand(Command.Move.ToString());

            // Assert
            VerifyPosition(2, 1, Direction.East);
        }

        [TestMethod]
        public void GivenNoPosition_WhenExecuteMoveCommand_ShouldNotMoveRobot()
        {
            // Act
            _handler.ExecuteCommand(Command.Move.ToString());

            // Assert
            _robot.Position.ShouldBeNull();
            _robot.IsPlaced.ShouldBeFalse();
        }

        [TestMethod]
        public void GivenValidPosition_WhenExecuteLeftCommand_ShouldRotateRobotLeft()
        {
            // Arrange
            _handler.ExecuteCommand($"{Command.Place} 1,1,NORTH");

            // Act
            _handler.ExecuteCommand(Command.Left.ToString());

            // Assert
            VerifyPosition(1, 1, Direction.West);
        }

        [TestMethod]
        public void GivenNoPosition_WhenExecuteLeftCommand_ShouldDoNothing()
        {
            // Act
            _handler.ExecuteCommand(Command.Left.ToString());

            // Assert
            _robot.Facing.ShouldBe(Direction.North);
        }

        [TestMethod]
        public void GivenValidPosition_WhenExecuteRightCommand_ShouldRotateRobotRight()
        {
            // Arrange
            _handler.ExecuteCommand($"{Command.Place} 1,1,NORTH");

            // Act
            _handler.ExecuteCommand(Command.Right.ToString());

            // Assert
            VerifyPosition(1, 1, Direction.East);
        }

        [TestMethod]
        public void GivenNoPosition_WhenExecuteRightCommand_ShouldDoNothing()
        {
            // Act
            _handler.ExecuteCommand(Command.Right.ToString());

            // Assert
            _robot.Facing.ShouldBe(Direction.North);
        }

        [TestMethod]
        public void GivenValidPosition_WhenExecuteReport_ShouldOutputPositionAndFacing()
        {
            // Arrange
            _handler.ExecuteCommand($"{Command.Place} 1,2,EAST");

            // Act

            var report = _handler.ExecuteCommand(Command.Report.ToString());

            // Assert
            report.Trim().ShouldBe("1, 2, East");
        }

        [TestMethod]
        public void ExecuteInvalidCommand_WhenExecute_ShouldThrowArgumentException()
        {
            // Act & Assert
            Should
                .Throw<ArgumentException>(() => _handler.ExecuteCommand("INVALID"))
                .Message.ShouldBe("Invalid command value: INVALID");
        }

        [TestMethod]
        public void GivenNullCommand_WhenExecute_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Should
                .Throw<ArgumentNullException>(() => _handler.ExecuteCommand(null))
                .Message.ShouldContain("Parameter 'command cannot be null or empty'");
        }

        [TestMethod]
        public void GivenEmptyCommand_WhenExecute_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Should
                .Throw<ArgumentException>(() => _handler.ExecuteCommand(""))
                .Message.ShouldContain("Parameter 'command cannot be null or empty'");
        }

        private void VerifyPosition(int x, int y, Direction facing)
        {
            _robot.Position.X.ShouldBe(x);
            _robot.Position.Y.ShouldBe(y);
            _robot.Facing.ShouldBe(facing);

            var currentPosition = _handler.ExecuteCommand(Command.Report.ToString());
            currentPosition.ShouldBe($"{x}, {y}, {facing}");
        }
    }
}
