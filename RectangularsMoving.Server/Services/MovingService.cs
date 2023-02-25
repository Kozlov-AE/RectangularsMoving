using RectangularsMoving.Server.Enums;
using RectangularsMoving.Server.Models;

namespace RectangularsMoving.Server.Services {
    public class MovingService {
        private ILogger<MovingService> _logger;

        public MovingService(ILogger<MovingService> logger) {
            _logger = logger;
        }

        public void MoveRect(ref RectWithDirection rect, byte maxValue, int height, int width) {
            rect.IsReflectioning = false;
            Random random = new Random();
            var move = random.Next(maxValue);
            int x = 0;
            int y = 0;
            bool xRefl = false;
            bool yRefl = false;
            switch (rect.Direction) {
                case MoveDirection.TopLeft:
                    x = rect.X - move;
                    y = rect.Y - move;
                    if (x < 0) {
                        y -= x;
                        x = 0;
                        rect.Direction = MoveDirection.TopRight;
                        xRefl = true;
                    }

                    if (y < 0) {
                        x -= y;
                        y = 0;
                        rect.Direction = MoveDirection.BottomLeft;
                        yRefl = true;
                    }

                    if (xRefl && yRefl) {
                        rect.Direction = MoveDirection.BottomRight;
                    }
                    break;
                case MoveDirection.BottomLeft:
                    x = rect.X - move;
                    y = rect.Y + move;
                    if (x < 0) {
                        y -= x;
                        x = 0;
                        rect.Direction = MoveDirection.BottomRight;
                        xRefl = true;
                    }

                    if ((y + rect.Height) > height) {
                        var deltaY = height - y - rect.Height;
                        y = height - rect.Height;
                        x -= deltaY;
                        rect.Direction = MoveDirection.TopLeft;
                        yRefl = true;
                    }

                    if (xRefl && yRefl) {
                        rect.Direction = MoveDirection.TopRight;
                    }
                    break;
                case MoveDirection.TopRight:
                    x = rect.X + move;
                    y = rect.Y - move;
                    if (y < 0) {
                        x -= y;
                        y = 0;
                        rect.Direction = MoveDirection.BottomRight;
                    }

                    if ((x + rect.Width) > width) {
                        var deltaX = width - x - rect.Width;
                        x = width - rect.Width;
                        y -= deltaX;
                        rect.Direction = MoveDirection.TopLeft;
                    }

                    if (xRefl && yRefl) {
                        rect.Direction = MoveDirection.BottomLeft;
                    }
                    break;
                case MoveDirection.BottomRight:
                    x = rect.X + move;
                    y = rect.Y + move;
                    if ((x + rect.Width) > width) {
                        var deltaX = width - x - rect.Width;
                        x = width - rect.Width;
                        y -= deltaX;
                        rect.Direction = MoveDirection.BottomLeft;
                    }

                    if ((y + rect.Height) > height) {
                        var deltaY = height - y - rect.Height;
                        y = height - rect.Height;
                        x -= deltaY;
                        rect.Direction = MoveDirection.TopRight;
                    }

                    if (xRefl && yRefl) {
                        rect.Direction = MoveDirection.TopLeft;
                    }
                    break;
            }

            if (xRefl || yRefl) rect.IsReflectioning = true;
            rect.X = x;
            rect.Y = y;
        }
    }
}