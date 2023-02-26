using Grpc.Core;
using RectangularsMoving.Protos;
using RectangularsMoving.Server.Models;

namespace RectangularsMoving.Server.Services {
    public class RectMovingService : RectMoving.RectMovingBase {
        private readonly RectGeneratorService _generatorService;
        private readonly MovingService _movingService;
        private readonly ILogger<RectMovingService> _logger;
        private List<RectWithDirection> _collection;
        private readonly object _collectionLock = new object();
        private readonly object _sendMessageLock = new object();
        public RectMovingService(RectGeneratorService generatorService, MovingService movingService, ILogger<RectMovingService> logger) {
            _generatorService = generatorService;
            _movingService = movingService;
            _logger = logger;
        }

        public override async Task SetConfig(ConfigRequest request, IServerStreamWriter<Rect> responseStream, ServerCallContext context) {
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


                await Task.Run(() => _generatorService.GenerateRects(semaphore, request.Board.RectsCount,
                    request.Board.Height, request.Board.Width));

                List<Task> tasks = new List<Task>(request.Board.RectsCount);
                while (true) {
                    var opId = Guid.NewGuid();
                    _logger.LogInformation($"Start moving in collection ({opId})");
                    var count = _collection.Count;
                    for (int i = 0; i < count; i++) {
                        var item = _collection[i];
                        tasks.Add(Task.Run(async () => {
                            await semaphore.WaitAsync();
                            try {
                                lock (_collectionLock) {
                                    _movingService.MoveRect(ref item, 30, request.Board.Height, request.Board.Width);
                                }

                                lock (_sendMessageLock) {
                                    responseStream.WriteAsync(item.Map());
                                }

                                await Task.Delay(request.TaskDelay);
                            }
                            catch (Exception e) {
                                _logger.LogError(e.Message);
                            }
                            finally {
                                semaphore.Release();
                            }

                        }));
                    }

                    Task.WaitAll(tasks.ToArray());
                    tasks.Clear();
                    _logger.LogInformation($"Finished all tasks in moving in collection ({opId})");
                }

            }
            catch (Exception ex) {
                _logger.LogError(ex.Message, ex);
                semaphore.Release();
            }
        }
        
    }
}
