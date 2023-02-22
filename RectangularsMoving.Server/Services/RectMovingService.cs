using Grpc.Core;
using RectangularsMoving.Server.Models;
using RectangularsMoving.Shared.Protos;

namespace RectangularsMoving.Server.Services {
    public class RectMovingService : RectMoving.RectMovingBase {
        private readonly RectGeneratorService _generatorService;
        private readonly MovingService _movingService;
        private readonly ILogger<RectMovingService> _logger;
        private List<RectWithDirection> _collection;
        private readonly object _collectionLock = new object();
        public RectMovingService(RectGeneratorService generatorService, MovingService movingService, ILogger<RectMovingService> logger) {
            _generatorService = generatorService;
            _movingService = movingService;
            _logger = logger;
        }

        public override Task SetConfig(ConfigRequest request, IServerStreamWriter<Rect> responseStream, ServerCallContext context) {
            var semaphore = new SemaphoreSlim(request.TasksCount);
            try {
                _logger.LogInformation("Received ConfigRequest");
                _collection = new List<RectWithDirection>(request.Board.RectsCount);
                _generatorService.NewRectGenerated += r => Task.Run(async () => {
                    _collection.Add(r);
                    _logger.LogInformation($"New rect ({r.Id}) added to collection");
                    await responseStream.WriteAsync(r.Map());
                    _logger.LogInformation($"Rect ({r.Id}) sent to client");
                });


                Task.Run(() => _generatorService.GenerateRects(semaphore, request.Board.RectsCount,
                    request.Board.Height, request.Board.Width));

                while (true) {
                    int count;
                    lock (_collectionLock) {
                        count = _collection.Count;
                    }

                    for (int i = 0; i < count; i++) {
                        var item = _collection[i];
                        Task.Run(async () => {
                            await semaphore.WaitAsync();
                            lock (_collectionLock) {
                                _movingService.MoveRect(ref item, 20, request.Board.Height, request.Board.Width);
                            }
                            await responseStream.WriteAsync(item.Map());
                        });
                        semaphore.Release();
                    }
                }

            }
            catch (Exception ex) {
                _logger.LogError(ex.Message, ex);
                semaphore.Release();
            }
            return Task.CompletedTask;
        }
    }
}
