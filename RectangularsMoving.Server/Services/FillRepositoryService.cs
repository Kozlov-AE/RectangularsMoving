using RectangularsMoving.Shared.Enums;
using RectangularsMoving.Shared.Interfaces.Repository;
using RectangularsMoving.Shared.Interfaces.Repository.Models;

namespace RectangularsMoving.Server.Services {
    public class FillRepositoryService {
        private readonly IRectRepository _repo;
        
        public event Action<IRect> NewRectAdded;
        
        public void FillRepository(int count,int height, int width) {
            var minH = height / 50;
            var minW = width / 50;
            var maxH = height / 30;
            var maxW = width / 30;
            var rand = new Random();
            for (int i = 0; i < count; i++) {
                var h = rand.Next(minH, maxH);
                var w = rand.Next(minW, maxW);
                var x = rand.Next(width);
                var direct = rand.Next(3);
                var rect = new RectRecord() {
                    Width = w,
                    Height = h,
                    X = rand.Next(width),
                    Y = rand.Next(height),
                    Direction = (MovingDirection)direct
                };
                _repo.AddOrUpdateAsync(rect);
            }
        }
    }
}