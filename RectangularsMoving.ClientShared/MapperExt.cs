using RectangularsMoving.ClientShared.ViewModels;
using RectangularsMoving.Shared.Protos;

namespace RectangularsMoving.ClientShared {
    public static class MapperExt {
        public static RectViewModel Map(this Rect source, string color) {
            return new RectViewModel(source.Id, source.X, source.Y, source.Height, source.Width, color);
        }
    }
}