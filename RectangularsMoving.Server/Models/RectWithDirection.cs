using RectangularsMoving.Server.Enums;
using RectangularsMoving.Shared.Protos;

namespace RectangularsMoving.Server.Models {
    public class RectWithDirection {
        public int Id { get; init; } = -1;
        public int Height { get; set; }
        public int Width { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public MoveDirection Direction { get; set; }

    }
}