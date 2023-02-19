using Grpc.Core;
using RectangularsMoving.Shared.Protos;

namespace RectangularsMoving.Server.Services {
    public class ConfigService : Config.ConfigBase {
        public ConfigService() {
            
        }
        // Получаем экземпляр сервиса, который пробегается по хранилищу прямоугольников и меняет их координаты
        public override Task SetConfig(ConfigRequest request, IServerStreamWriter<Rect> responseStream, ServerCallContext context) {
            // Иду в фабрику прямоугольников, передаю туда параметры
            // Подписываюсь на событие создания нового прямоугольника
            // Как только производится новый прямоугольник, скидываю его в свое хранилище
            // Отправляю его на клиент (пишу в responseStream)
            // Если сервис изменения координат не запущен, то запускаю его, передавая ссылку на хранилище и подписываюсь на изменения координат
            // Получаю новые координаты -> отправляю их клиенту
        }
    }
}