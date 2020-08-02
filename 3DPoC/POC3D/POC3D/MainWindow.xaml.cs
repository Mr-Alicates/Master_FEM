using System.Windows;
using System.Windows.Media.Media3D;
using POC3D.Controls.Matrix;
using POC3D.ViewModel;

namespace POC3D
{
    /// <summary>
    ///     Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty MainViewModelProperty = DependencyProperty.Register(
            nameof(MainViewModel), typeof(MainViewModel), typeof(MainWindow));

        private GlobalMatrixInfoWindow _globalMatrixInfoWindow;

        private MatrixInfoWindow _matrixInfoWindow;

        public MainWindow()
        {
            InitializeComponent();
            MainViewModel = new MainViewModel();
            DataContext = MainViewModel;
        }

        public MainViewModel MainViewModel
        {
            get => (MainViewModel) GetValue(MainViewModelProperty);
            set => SetValue(MainViewModelProperty, value);
        }

        private void ShowMatrixInfoWindow(object sender, RoutedEventArgs e)
        {
            if (_matrixInfoWindow == null ||
                !_matrixInfoWindow.IsVisible)
            {
                _matrixInfoWindow = new MatrixInfoWindow
                {
                    DataContext = DataContext
                };

                _matrixInfoWindow.Show();
            }
        }

        private void ShowGlobalMatrixInfoWindow(object sender, RoutedEventArgs e)
        {
            if (!MainViewModel.ProblemViewModel.ProblemCalculationViewModel.CanBeSolved)
            {
                return;
            }

            if (_globalMatrixInfoWindow == null ||
                !_globalMatrixInfoWindow.IsVisible)
            {
                _globalMatrixInfoWindow = new GlobalMatrixInfoWindow
                {
                    DataContext = DataContext
                };

                _globalMatrixInfoWindow.Show();
            }
        }
    }
}