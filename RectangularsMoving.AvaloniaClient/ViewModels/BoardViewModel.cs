using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using RectangularsMoving.Shared.Protos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RectangularsMoving.AvaloniaClient.ViewModels {
    public partial class BoardViewModel : ObservableObject {
        private readonly object _rectsLocker;
        [ObservableProperty] private int _width;
        [ObservableProperty] private int _height;
        [ObservableProperty] private int _rectsCount;
        [ObservableProperty] private ObservableCollection<RectViewModel> _rects;
        
        public BoardViewModel(int width, int height, int rectsCount) {
            _rectsLocker = new object();
            Width = width;
            Height = height;
            RectsCount = rectsCount;
            Rects = new ObservableCollection<RectViewModel>();
        }
        public void SetRectCoords(Rect rect) {
            try {
                lock (_rectsLocker) {
                    var currentRect = Rects.FirstOrDefault(r => r.Id == rect.Id);
                    if (currentRect == null) {
                        var color = $"#{Random.Shared.Next(0x808080) & 0x8A8A8A:X6}";
                        var newRect = rect.Map(color);
                        Rects.Add(newRect);
                    }
                    else {
                        Dispatcher.UIThread.Post(() => currentRect.SetCoordinates(rect.X, rect.Y));
                    }
                }
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
