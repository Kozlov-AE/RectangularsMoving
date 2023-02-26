using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using RectangularsMoving.Protos;
using RectangularsMoving.Server.Models;

namespace RectangularsMoving.Server.Services {
    public class Rpc_RectMovingService : RectMoving.RectMovingBase {
        private readonly RectGeneratorService _generatorService;
        private readonly MovingService _movingService;
        private readonly ClientsWorkHolder _workHolder;
        private readonly ILogger<Rpc_RectMovingService> _logger;
        private List<RectWithDirection> _collection;
        private readonly object _collectionLock = new object();
        private readonly object _sendMessageLock = new object();
        public Rpc_RectMovingService(RectGeneratorService generatorService, 
            MovingService movingService, 
            ILogger<Rpc_RectMovingService> logger, 
            ClientsWorkHolder workHolder) {
            _generatorService = generatorService;
            _movingService = movingService;
            _logger = logger;
            _workHolder = workHolder;
        }

        public override async Task StartAsync(ConfigRequest request, IServerStreamWriter<Rect> responseStream, ServerCallContext context) {
            var semaphore = new SemaphoreSlim(request.TasksCount);
            var clientId = context.RequestHeaders.GetValue("ClientId");
            if (string.IsNullOrEmpty(clientId)) {
                _logger.LogError("Client id is null or empty, return from the start method");
                return;
            }
            try {
                _logger.LogInformation("Received ConfigRequest");
                _collection = new List<RectWithDirection>(request.Board.RectsCount);

                
                _workHolder.ClientStart(clientId);
                
                _generatorService.NewRectGenerated += r => Task.Run(async () => {
                    _collection.Add(r);
                    _logger.LogInformation($"New rect ({r.Id}) added to collection");
                    await responseStream.WriteAsync(r.Map());
                    _logger.LogInformation($"Rect ({r.Id}) sent to client");
                });


                await Task.Run(() => _generatorService.GenerateRects(semaphore, request.Board.RectsCount,
                    request.Board.Height, request.Board.Width));

                List<Task> tasks = new List<Task>(request.Board.RectsCount);
                while (_workHolder.CheckIfClientIsWorking(clientId)) {
                    var opId = Guid.NewGuid();
                    _logger.LogInformation($"Start moving in collection ({opId})");
                    var count = _collection.Count;
                    for (int i = 0; i < count; i++) {
                        var item = _collection[i];
                        tasks.Add(Task.Run(async () => {
                            await semaphore.WaitAsync();
                            try {
                                lock (_collectionLock) {
                                    _movingService.MoveRect(ref item, (byte)request.MaxMovingDistance, request.Board.Height, request.Board.Width);
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
                _workHolder.ClientStop(clientId);
            }
        }

        public override Task<Empty> Stop(Empty request, ServerCallContext context) {
            _workHolder.ClientStop(context.RequestHeaders.GetValue("ClientId"));
            return Task.FromResult(new Empty());
        }
    }
}
