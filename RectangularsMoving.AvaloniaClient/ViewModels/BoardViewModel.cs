using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectangularsMoving.AvaloniaClient.ViewModels {
    public partial class BoardViewModel : ObservableObject {
        [ObservableProperty] private int _width;
        [ObservableProperty] private int _height;
        [ObservableProperty] private int _rectsCount;
        [ObservableProperty] private ObservableCollection<RectViewModel> _rects;

        public BoardViewModel(int width, int height, int rectsCount, ObservableCollection<RectViewModel> rects) {
            Width = width;
            Height = height;
            RectsCount = rectsCount;
            Rects = rects;
        }

        public void GenerateRects(int count){
            
        }

        public void SetRectCoords(RectViewModel rect, int x, int y){
            rect.SetCoordinates(x, y);
        }
    }
}
