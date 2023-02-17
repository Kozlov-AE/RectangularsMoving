using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectangularsMoving.AvaloniaClient.ViewModels {
    public partial class RectViewModel : ObservableObject {
        public int Id{ get; set; }
        [ObservableProperty] int _x;
        [ObservableProperty] int _y;
        [ObservableProperty] int _height;
        [ObservableProperty] int _width;
        [ObservableProperty] string _color;


        public RectViewModel(int x, int y, int height, int width,string color) {
            X = x; 
            Y = y; 
            Height = height; 
            Width = width;
            Color = color;
        }

        public void SetCoordinates(int x, int y){
            X = x;
            Y = y;
        }
    }
}
