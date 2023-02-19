using RectangularsMoving.Shared.Enums;
using RectangularsMoving.Shared.Interfaces.Repository.Models;

namespace InMemoryRepository {
    public record RectModel: IRect {
        public string Id { get; init; } = String.Empty;
        public int Height { get; set; }
        public int Width { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public MovingDirection Direction { get; set; }
    }
}