using RectangularsMoving.Server.Enums;
using RectangularsMoving.Server.Models;
using RectangularsMoving.Shared.Protos;

namespace RectangularsMoving.Server.Services {
    public class RectGeneratorService {
        private readonly ILogger<RectGeneratorService> _logger;

        public RectGeneratorService(ILogger<RectGeneratorService> logger) {
            _logger = logger;
        }

        public event Action<RectWithDirection> NewRectGenerated;
        
        public async Task GenerateRects(SemaphoreSlim semaphore, int count,int height, int width) {
            var minH = height / 50;
            var minW = width / 50;
            var maxH = height / 30;
            var maxW = width / 30;
            List<Task> tasks = new List<Task>(count);
            for (int i = 0; i < count; i++) {
                var i1 = i;
                tasks.Add(Task.Run(() => {
                    _logger.LogInformation($"Start creating rect with ID = {i1 + 1}, wait for semaphore.");
                    semaphore.Wait();
                    var rect = new RectWithDirection() {
                        Id = i1 + 1,
                        Width = Random.Shared.Next(minW, maxW),
                        Height = Random.Shared.Next(minH, maxH),
                        X = Random.Shared.Next(width),
                        Y = Random.Shared.Next(height),
                        Direction = (MoveDirection)Random.Shared.Next(3)
                    };
                    _logger.LogInformation(
                        $"Rect ({rect.Id}) generated: H= {rect.Height}, W= {rect.Width}, X= {rect.X}, Y= {rect.Y}");
                    NewRectGenerated?.Invoke(rect);
                }));
                semaphore.Release();
            }
        }
    }
}