using Grpc.Core;
using RectangularsMoving.Server.Models;
using RectangularsMoving.Shared.Protos;

namespace RectangularsMoving.Server.Services {
    public class RectMovingService : RectMoving.RectMovingBase {
        private readonly RectGeneratorService _generatorService;
        private readonly MovingService _movingService;
        private readonly ILogger<RectMovingService> _logge;
        private List<RectWithDirection> _collection;
        private readonly object _collectionLock = new object();
        public RectMovingService(RectGeneratorService generatorService, MovingService movingService) {
            _generatorService = generatorService;
            _movingService = movingService;
        }

        public override async Task SetConfig(ConfigRequest request, IServerStreamWriter<Rect> responseStream, ServerCallContext context) {
            _collection = new List<RectWithDirection>(request.Board.RectsCount);
            _generatorService.NewRectGenerated += r => Task.Run(async () => {
                lock (_collectionLock) {
                    _collection.Add(r);
                }
                await responseStream.WriteAsync(r.Map());
            });

            SemaphoreSlim semaphore = new SemaphoreSlim(request.TasksCount);
            
            Task.Run(() => _generatorService.GenerateRects(semaphore, request.Board.RectsCount, request.Board.Height, request.Board.Width));
            
            while (true) {
                int count;
                lock (_collectionLock) {
                    count = _collection.Count;
                }
                for (int i = 0; i < count; i++) {
                    var item = _collection[i];
                    Task.Run(async () => {
                        semaphore.Wait();
                        lock (_collectionLock) {
                            _movingService.MoveRect(ref item, 20, request.Board.Height, request.Board.Width);
                        }

                        await responseStream.WriteAsync(item.Map());
                    });
                }
            }
        }
    }
}