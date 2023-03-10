using RectangularsMoving.Protos;
using RectangularsMoving.Server.Models;

namespace RectangularsMoving.Server.Services {
    public static class MapExts {
        public static Rect Map(this RectWithDirection source) {
            return new Rect() {
                Id = source.Id,
                X = source.X,
                Y = source.Y,
                Width = source.Width,
                Height = source.Height,
                IsReflectioning = source.IsReflectioning
            };
        }
    }
}