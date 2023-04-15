using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using RectangularsMoving.Protos;

namespace RectangularsMoving.ClientShared.ViewModels {
    public partial class BoardViewModel : ObservableObject {
        private readonly IAppContext _appContext;
        
        private readonly object _rectsLocker;
        [ObservableProperty] private int _width;
        [ObservableProperty] private int _height;
        [ObservableProperty] private ObservableCollection<RectViewModel> _rects;


        public BoardViewModel(IAppContext appContext) {
            _appContext = appContext;
            _rectsLocker = new object();
            Rects = new ObservableCollection<RectViewModel>();
        }

        public void SetBoardsProperties(int width, int height) {
            Width = width;
            Height = height;
        }
        public void SetRectCoords(Rect rect) {
            try {
                lock (_rectsLocker) {
                    var currentRect = Rects.FirstOrDefault(r => r.Id == rect.Id);
                    if (currentRect == null) {
                        var color = $"#{Random.Shared.Next(0x808080) & 0x8A8A8A:X6}";
                        var newRect = rect.Map(color);
                        _appContext.RunInUiThreadAsync(() => {
                            Rects.Add(newRect);
                            return Task.CompletedTask;
                        });
                    }
                    else {
                            _appContext.RunInUiThreadAsync(() => {
                                currentRect.SetCoordinates(rect.X, rect.Y);
                                return Task.CompletedTask;
                        });
                    }
                }
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
