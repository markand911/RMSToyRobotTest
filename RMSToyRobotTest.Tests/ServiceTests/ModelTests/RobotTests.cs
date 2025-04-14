using RMSToyRobotTest.Service.Models;
using Shouldly;

namespace RMSToyRobotTest.Tests.ServiceTests.ModelTests
{
    [TestClass]
    public class RobotTests
    {
        Robot robot;

        [TestInitialize]
        public void Setup()
        {
            robot = new Robot(5);
        }

        [TestMethod]
        public void Place_GivenValidPosition_ShouldSetPositionAndFacing()
        {
            var position = new Position(2, 2);
            // Act
            robot.Place(position.X, position.Y, Direction.North);

            // Assert
            robot.Position.ShouldNotBeNull();
            robot.Position.X.ShouldBe(position.X);
            robot.Position.Y.ShouldBe(position.Y);
            robot.Facing.ShouldBe(Direction.North);
            robot.IsPlaced.ShouldBeTrue();
        }

        [TestMethod]
        [DataRow(0, 4, DisplayName = "Place_GivenTopLeftBoundaryPosition_ShouldSetPosition")]
        [DataRow(4, 0, DisplayName = "Place_GivenBottomRightBoundaryPosition_ShouldSetPosition")]
        [DataRow(4, 4, DisplayName = "Place_GivenTopRightBoundaryPosition_ShouldSetPosition")]
        [DataRow(0, 0, DisplayName = "Place_GivenBottomLeftBoundaryPosition_ShouldSetPosition")]
        public void Place_GivenBoundaryPositions_ShouldSetPosition(int X, int Y)
        {
            // Act
            robot.Place(X, Y, Direction.North);

            // Assert
            robot.Position.ShouldNotBeNull();
            robot.Position.X.ShouldBe(X);
            robot.Position.Y.ShouldBe(Y);
            robot.Facing.ShouldBe(Direction.North);
            robot.IsPlaced.ShouldBeTrue();
        }

        [TestMethod]
        [DataRow(5, 0, DisplayName = "Place_GivenInvalidXCoordinate_ShouldNotSetPosition")]
        [DataRow(1, 6, DisplayName = "Place_GivenInvalidYCoordinate_ShouldNotSetPosition")]
        [DataRow(7, 6, DisplayName = "Place_GivenInvalidCoordinates_ShouldNotSetPosition")]
        public void Place_ShouldNotSetPosition_WhenInvalidPosition(int X, int Y)
        {
            // Act
            robot.Place(X, Y, Direction.North);
            // Assert
            robot.Position.ShouldBeNull();
            robot.IsPlaced.ShouldBeFalse();
        }

        [TestMethod]
        public void Move_GivenValidMove_ShouldUpdatePosition()
        {
            // Arrange
            var position = new Position(2, 2);
            robot.Place(position.X, position.Y, Direction.East);

            // Act
            robot.Move();

            // Assert
            robot.Position.X.ShouldBe(position.X + 1); // moving East
            robot.Position.Y.ShouldBe(position.Y);
            robot.Facing.ShouldBe(Direction.East);
        }

        [TestMethod]
        [DataRow(
            0,
            0,
            Direction.South,
            DisplayName = "Move_GivenMovingDownFromTheMostBottomPlace_ShouldNotMove"
        )]
        [DataRow(
            3,
            0,
            Direction.South,
            DisplayName = "Move_GivenMovingDownFromTheMiddleBottomPlace_ShouldNotMove"
        )]
        [DataRow(
            0,
            0,
            Direction.West,
            DisplayName = "Move_GivenMovingLeftFromTheMostLeftPlace_ShouldNotMove"
        )]
        [DataRow(
            0,
            3,
            Direction.West,
            DisplayName = "Move_GivenMovingLeftFromTheMiddleLeftPlace_ShouldNotMove"
        )]
        [DataRow(
            0,
            4,
            Direction.North,
            DisplayName = "Move_GivenMovingTopFromTheMostTopPlace_ShouldNotMove"
        )]
        [DataRow(
            3,
            4,
            Direction.North,
            DisplayName = "Move_GivenMovingTopFromTheMiddleTopPlace_ShouldNotMove"
        )]
        [DataRow(
            4,
            4,
            Direction.East,
            DisplayName = "Move_GivenMovingRightFromTheMostRightPlace_ShouldNotMove"
        )]
        [DataRow(
            4,
            3,
            Direction.East,
            DisplayName = "Move_GivenMovingRightFromTheMiddleRightPlace_ShouldNotMove"
        )]
        public void Move_GivenInvalidBoundaryPosition_WhenMove_ShouldNotUpdatePosition(
            int X,
            int Y,
            Direction facing
        )
        {
            // Arrange
            robot.Place(X, Y, facing);

            // Act
            robot.Move();

            // Assert - No change in position
            robot.Position.X.ShouldBe(X);
            robot.Position.Y.ShouldBe(Y);
        }

        [TestMethod]
        public void Move_GivenNoPlace_WhenMove_ShouldNotMove()
        {
            // Act
            robot.Move();
            robot.Position.ShouldBeNull();
            robot.IsPlaced.ShouldBeFalse();
        }

        [TestMethod]
        public void RotateLeft_GivenValidPosition_WhenRotateLeft_ShouldUpdateFacingCorrectly()
        {
            // Arrange
            robot.Place(2, 2, Direction.North);

            // Act
            robot.RotateLeft();

            // Assert
            robot.Facing.ShouldBe(Direction.West);
        }

        [TestMethod]
        public void RotateLeft_GivenValidPosition_WhenRotateLeftMultipleTimes_ShouldUpdateFacingCorrectly()
        {
            // Arrange
            robot.Place(2, 2, Direction.North);

            // Act & Assert
            robot.RotateLeft();
            robot.Facing.ShouldBe(Direction.West);

            robot.RotateLeft();
            robot.Facing.ShouldBe(Direction.South);

            robot.RotateLeft();
            robot.Facing.ShouldBe(Direction.East);

            robot.RotateLeft();
            robot.Facing.ShouldBe(Direction.North); // Back to north. Spin on same position
        }

        [TestMethod]
        public void RotateLeft_GivenRobotNotPlaced_WhenRotateLeft_ShouldNotUpdateFacing()
        {
            //Act
            robot.RotateLeft();
            robot.Facing.ShouldBe(Direction.North); // Default facing;
        }

        [TestMethod]
        public void RotateRight_GivenValidPosition_WhenRotateRight_ShouldUpdateFacingCorrectly()
        {
            // Arrange
            robot.Place(2, 2, Direction.North);

            // Act
            robot.RotateRight();

            // Assert
            robot.Facing.ShouldBe(Direction.East);
        }

        [TestMethod]
        public void RotateRight_GivenValidPosition_WhenRotateRightMultipleTimes_ShouldUpdateFacingCorrectly()
        {
            // Arrange
            robot.Place(2, 2, Direction.North);

            // Act & Assert
            robot.RotateRight();
            robot.Facing.ShouldBe(Direction.East);

            robot.RotateRight();
            robot.Facing.ShouldBe(Direction.South);

            robot.RotateRight();
            robot.Facing.ShouldBe(Direction.West);

            robot.RotateRight();
            robot.Facing.ShouldBe(Direction.North); // Back to north. Spin on same position
        }

        [TestMethod]
        public void RotateRight_GivenRobotNotPlaced_WhenRotateRight_ShouldNotUpdateFacing()
        {
            //Act
            robot.RotateRight();
            robot.Facing.ShouldBe(Direction.North); // Default facing;
        }

        //[TestMethod]
        //public void Report_GivenValidPosition_ShouldReturnCorrectPositionAndFacing()
        //{
        //    int x = 1,
        //        y = 2;

        //    // Arrange
        //    robot.Place(x, y, Direction.East);

        //    // Act
        //    var report = robot.Report();

        //    // Assert
        //    report.ShouldBe($"{x}, {y}, {Direction.East}");
        //}

        //[TestMethod]
        //public void Report_GivenRobotNotPlaced_ShouldReturnNull()
        //{
        //    // Act
        //    var report = robot.Report();

        //    // Assert
        //    report.ShouldBeNull();
        //}
    }
}
