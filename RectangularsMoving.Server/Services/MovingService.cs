using RectangularsMoving.Server.Enums;
using RectangularsMoving.Server.Models;

namespace RectangularsMoving.Server.Services {
    public class MovingService {
        private ILogger<MovingService> _logger;

        public MovingService(ILogger<MovingService> logger) {
            _logger = logger;
        }

        public RectWithDirection MoveRect(ref RectWithDirection rect, byte maxValue, int height, int width) {
            var result = rect;
            Random random = new Random();
            var move = random.Next(maxValue);
            int x = 0;
            int y = 0;
            switch (rect.Direction) {
                case MoveDirection.TopLeft:
                    x = rect.X - move;
                    y = rect.Y - move;
                    if (x < 0) {
                        y -= x;
                        x = 0;
                        result.Direction = MoveDirection.TopRight;
                    }

                    if (y < 0) {
                        x -= y;
                        y = 0;
                        result.Direction = MoveDirection.BottomLeft;
                    }

                    if (x == 0 && y == 0) {
                        result.Direction = MoveDirection.BottomRight;
                    }
                    break;
                case MoveDirection.BottomLeft:
                    x = rect.X - move;
                    y = rect.Y + move;
                    if (x < 0) {
                        y -= x;
                        x = 0;
                        result.Direction = MoveDirection.BottomRight;
                    }

                    if ((y + rect.Height) > height) {
                        var deltaY = y - height - rect.Height;
                        y = height - rect.Height;
                        x -= deltaY;
                        result.Direction = MoveDirection.TopLeft;
                    }

                    if (x == 0 && y == height - rect.Height) {
                        result.Direction = MoveDirection.TopRight;
                    }
                    break;
                case MoveDirection.TopRight:
                    x = rect.X + move;
                    y = rect.Y - move;
                    if (y < 0) {
                        x -= y;
                        y = 0;
                        result.Direction = MoveDirection.BottomRight;
                    }

                    if ((x + rect.Width) > width) {
                        var deltaX = x - width - rect.Width;
                        x = width - rect.Width;
                        y -= deltaX;
                        result.Direction = MoveDirection.TopLeft;
                    }

                    if (x == width - rect.Width && y == 0) {
                        result.Direction = MoveDirection.BottomLeft;
                    }
                    break;
                case MoveDirection.BottomRight:
                    x = rect.X + move;
                    y = rect.Y + move;
                    if ((x + rect.Width) > width) {
                        var deltaX = x - width - rect.Width;
                        x = width - rect.Width;
                        y -= deltaX;
                        result.Direction = MoveDirection.BottomLeft;
                    }

                    if ((y + rect.Height) > height) {
                        var deltaY = y - height - rect.Height;
                        y = height - rect.Height;
                        x -= deltaY;
                        result.Direction = MoveDirection.TopRight;
                    }

                    if (x == width - rect.Width && y == height - rect.Height) {
                        result.Direction = MoveDirection.TopLeft;
                    }
                    break;
            }
            result.X = x;
            result.Y = y;
            return result;
        }
    }
}