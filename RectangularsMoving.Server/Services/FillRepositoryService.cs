using RectangularsMoving.Server.Enums;
using RectangularsMoving.Server.Models;
using RectangularsMoving.Shared.Protos;

namespace RectangularsMoving.Server.Services {
    public class FillRepositoryService {
        public event Action<RectWithDirection> NewRectAdded;
        
        public async Task GenerateRects(int count,int height, int width) {
            var minH = height / 50;
            var minW = width / 50;
            var maxH = height / 30;
            var maxW = width / 30;
            var rand = new Random();
            for (int i = 0; i < count; i++) {
                var h = rand.Next(minH, maxH);
                var w = rand.Next(minW, maxW);
                var x = rand.Next(width);
                var direct = rand.Next(0, 3);
                var rect = new RectWithDirection() {
                    Id = i + 1,
                    Width = w,
                    Height = h,
                    X = rand.Next(width),
                    Y = rand.Next(height),
                    Direction = (MoveDirection)direct
                };
                NewRectAdded?.Invoke(rect);
            }
        }
    }
}