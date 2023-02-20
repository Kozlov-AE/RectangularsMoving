using Grpc.Core;
using RectangularsMoving.Shared.Interfaces.Repository;
using RectangularsMoving.Shared.Protos;

namespace RectangularsMoving.Server.Services {
    public class RectMovingService : RectMoving.RectMovingBase {
        private readonly FillRepositoryService _fillService;
        private readonly MovingService _movingService;
        private readonly IRectRepository _repo;
        
        public RectMovingService(FillRepositoryService fillService, MovingService movingService, IRectRepository repo) {
            _fillService = fillService;
            _movingService = movingService;
            _repo = repo;
        }

        public override async Task SetConfig(ConfigRequest request, IServerStreamWriter<Rect> responseStream, ServerCallContext context) {
            var fillTask = Task.Run(() => _fillService.FillRepository(request.TasksCount, request.Board.Height, request.Board.Width));
            _fillService.NewRectAdded += rect => {
                await responseStream.WriteAsync(rect)
            };
            while (true) {
                var coll = await _repo.GetAllAsync();
            }
            // Иду в фабрику прямоугольников, передаю туда параметры
            // Подписываюсь на событие создания нового прямоугольника
            // Как только производится новый прямоугольник, скидываю его в свое хранилище
            // Отправляю его на клиент (пишу в responseStream)
            // Если сервис изменения координат не запущен, то запускаю его, передавая ссылку на хранилище и подписываюсь на изменения координат
            // Получаю новые координаты -> отправляю их клиенту
            
        }
    }
}