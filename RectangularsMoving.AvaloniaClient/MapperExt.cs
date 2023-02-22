using RectangularsMoving.AvaloniaClient.ViewModels;
using RectangularsMoving.Shared.Protos;

namespace RectangularsMoving.AvaloniaClient {
    public static class MapperExt {
        public static RectViewModel Map(this Rect source, string color) {
            return new RectViewModel(source.Id, source.X, source.Y, source.Height, source.Width, color);
        }
    }
}