namespace RMSToyRobotTest.Service.Models
{
    public class Robot
    {
        // Assuming table is square
        public readonly int _tableAreaSize;

        public Position Position { get; private set; }
        public Direction Facing { get; private set; }
        public bool IsPlaced => Position != null;

        public Robot(int tableAreaSize = 5)
        {
            _tableAreaSize = tableAreaSize;
        }

        public void Place(int x, int y, Direction facing)
        {
            if (IsValidPosition(x, y))
            {
                Position = new Position(x, y);
                Facing = facing;
            }
        }

        public void Move()
        {
            if (!IsPlaced)
                return;

            var newPosition = CalculateNewPosition();
            if (IsValidPosition(newPosition.X, newPosition.Y))
            {
                Position = newPosition;
            }
        }

        public void RotateLeft()
        {
            if (!IsPlaced)
                return;
            Facing = (Direction)(((int)Facing + 3) % 4);
        }

        public void RotateRight()
        {
            if (!IsPlaced)
                return;
            Facing = (Direction)(((int)Facing + 1) % 4);
        }

        private Position CalculateNewPosition()
        {
            var (newX, newY) = Facing switch
            {
                Direction.North => (Position.X, Position.Y + 1),
                Direction.East => (Position.X + 1, Position.Y),
                Direction.South => (Position.X, Position.Y - 1),
                Direction.West => (Position.X - 1, Position.Y),
                _ => (Position.X, Position.Y),
            };

            return new Position(newX, newY);
        }

        private bool IsValidPosition(int x, int y)
        {
            return x >= 0 && x < _tableAreaSize && y >= 0 && y < _tableAreaSize;
        }
    }
}
