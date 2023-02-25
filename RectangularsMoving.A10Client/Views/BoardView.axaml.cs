using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace RectangularsMoving.A10Client.Views {
    public partial class BoardView : UserControl {
        public BoardView() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }
    }
}