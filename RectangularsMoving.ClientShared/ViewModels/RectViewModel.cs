using CommunityToolkit.Mvvm.ComponentModel;

namespace RectangularsMoving.ClientShared.ViewModels {
    public partial class RectViewModel : ObservableObject {
        public int Id{ get; }
        [ObservableProperty] int _x;
        [ObservableProperty] int _y;
        [ObservableProperty] int _height;
        [ObservableProperty] int _width;
        [ObservableProperty] string _color;


        public RectViewModel(int id, int x, int y, int height, int width, string color) {
            Id = id;
            X = x; 
            Y = y; 
            Height = height; 
            Width = width;
            Color = string.IsNullOrEmpty(color) ? "#c0392b" : color;
        }

        public void SetCoordinates(int x, int y){
            X = x;
            Y = y;
        }
    }
}
