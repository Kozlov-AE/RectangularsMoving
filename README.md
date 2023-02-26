# RectangularsMoving
Реализован бэкенд на gRPC + 2 клиента на Avalonia (10.18 и 11.preview-5).
Размеры прямоугольников генерируются случайным образом по формуле: 
- Минимальная высота = высота зоны / 50;
- Минимальная ширина = ширина зоны / 50;
- Максимальная высота = высота зоны / 20;
- Максимальная ширина = ширина зоны / 20;
## Описание проектов в решении
1. Папка `Protos` - Папка для хранения Protobuf файлов
2. `RectangularsMoving.Server` - Проект сервера net7
3. `RectangularsMoving.ClientShared` - Общий проект для клиентских приложений (вьюмодели, маппинг, общие DI зависимости)
4. `RectangularsMoving.AvaloniaClient` - Клиент на фрэймворке Avalonia-11.preview-5 (Основной клиент)
5. `RectangularsMoving.A10Client` - Клиент на фрэймворке Avalonia-10.18 (Дополнительный клиент для стравнения скорости работы UI)

## Порядок запуска из MS Visual studio 2022
**Внимание! Запуск проектов осуществляется в режиме без отладки**
1. Запустить проект сервера в режиме **https**
2. Запустить проект клиента (можно оба по очереди, будут работать вместе)
  Внимание клиент Avalonia 11 примет цветовую тему вашей ОС (Dark/light).
  
## Описание интерфейса клиентов
![UI](https://user-images.githubusercontent.com/53231526/221420833-e3c42625-3b6a-4cb1-bb97-259d4a380ccb.png)
1. Количество прямоугольников
2. Высота зоны вывода
3. Ширина зоны вывода
4. Количество одновременно выполняемых задач на строне сервера
5. Максимальная дистанция перемещения за одну итерацию
6. Оптимизация вывода на строне клиента - Происходит накопление координат прямоугольников и применяется по таймеру через заданный интервал
7. Оптимизация скорости изменения координат прямоугольников на стороне сервера - Имитируется тяжелая работа при расчете новых координат (задается значение Task.Delay в мс)
8. Начать процесс генерации и перемещения прямоугольников
9. Остановить запущенный процесс

## Видео демонстрация
https://user-images.githubusercontent.com/53231526/221421280-3f4301b9-427b-4f96-989e-19a598a4b671.mp4

