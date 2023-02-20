using RectangularsMoving.Shared.Enums;

namespace RectangularsMoving.Shared.Interfaces.Repository.Models {
    public interface IRect {
        string Id { get; init; }
        int Height { get; set; }
        int Width { get; set; }
        int X { get; set; }
        int Y { get; set; }
        MoveDirection Direction { get; set; }
    }
}