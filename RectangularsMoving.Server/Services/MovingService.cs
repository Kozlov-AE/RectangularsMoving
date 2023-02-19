using RectangularsMoving.Shared.Enums;
using RectangularsMoving.Shared.Interfaces.Repository.Models;
using RectangularsMoving.Shared.Models;

namespace RectangularsMoving.Server.Services {
    public class MovingService {
        public event Action<IRect> RectProcessed;
        public void ProcessCollection(IEnumerable<RectRecord> collection, int tCount, byte maxValue, int height, int width) {
            Parallel.ForEach(collection, new ParallelOptions(){MaxDegreeOfParallelism = tCount}, item => {
                MoveRect(item, maxValue, height, width);
            });
        }

        public IRect MoveRect(IRect rect, byte maxValue, int height, int width) {
            var result = rect;
            Random random = new Random();
            var move = random.Next(maxValue);
            int x = 0;
            int y = 0;
            switch (rect.Direction) {
                case MovingDirection.TopLeft:
                    x = rect.X - move;
                    y = rect.Y - move;
                    if (x < 0) {
                        y -= x;
                        x = 0;
                        result.Direction = MovingDirection.TopRight;
                    }

                    if (y < 0) {
                        x -= y;
                        y = 0;
                        result.Direction = MovingDirection.BottomLeft;
                    }

                    if (x == 0 && y == 0) {
                        result.Direction = MovingDirection.BottomRight;
                    }
                    break;
                case MovingDirection.BottomLeft:
                    x = rect.X - move;
                    y = rect.Y + move;
                    if (x < 0) {
                        y -= x;
                        x = 0;
                        result.Direction = MovingDirection.BottomRight;
                    }

                    if ((y + rect.Height) > height) {
                        var deltaY = y - height - rect.Height;
                        y = height - rect.Height;
                        x -= deltaY;
                        result.Direction = MovingDirection.TopLeft;
                    }

                    if (x == 0 && y == height - rect.Height) {
                        result.Direction = MovingDirection.TopRight;
                    }
                    break;
                case MovingDirection.TopRight:
                    x = rect.X + move;
                    y = rect.Y - move;
                    if (y < 0) {
                        x -= y;
                        y = 0;
                        result.Direction = MovingDirection.BottomRight;
                    }

                    if ((x + rect.Width) > width) {
                        var deltaX = x - width - rect.Width;
                        x = width - rect.Width;
                        y -= deltaX;
                        result.Direction = MovingDirection.TopLeft;
                    }

                    if (x == width - rect.Width && y == 0) {
                        result.Direction = MovingDirection.BottomLeft;
                    }
                    break;
                case MovingDirection.BottomRight:
                    x = rect.X + move;
                    y = rect.Y - move;
                    if ((x + rect.Width) > width) {
                        var deltaX = x - width - rect.Width;
                        x = width - rect.Width;
                        y -= deltaX;
                        result.Direction = MovingDirection.BottomLeft;
                    }

                    if ((y + rect.Height) > height) {
                        var deltaY = y - height - rect.Height;
                        y = height - rect.Height;
                        x -= deltaY;
                        result.Direction = MovingDirection.TopRight;
                    }

                    if (x == width - rect.Width && y == height - rect.Height) {
                        result.Direction = MovingDirection.TopLeft;
                    }
                    break;
            }
            result.X = x;
            result.Y = y;
            RectProcessed?.Invoke(result);
            return result;
        }
    }
}