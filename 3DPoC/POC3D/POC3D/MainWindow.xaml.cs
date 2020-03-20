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

            InitSimple();
        }

        public MainViewModel MainViewModel
        {
            get => (MainViewModel) GetValue(MainViewModelProperty);
            set => SetValue(MainViewModelProperty, value);
        }

        public void InitSimple()
        {
            MainViewModel.CameraViewModel.Position = new Point3D(-100, 0, 0);

            var node1 = MainViewModel.ProblemViewModel.AddNode(new Point3D(-10, -10, -10));
            var node5 = MainViewModel.ProblemViewModel.AddNode(new Point3D(0, 0, 10));
            var node2 = MainViewModel.ProblemViewModel.AddNode(new Point3D(10, -10, -10));
            var node3 = MainViewModel.ProblemViewModel.AddNode(new Point3D(10, 10, -10));

            node1.IsFixed = true;
            node5.IsFixed = false;
            node2.IsFixed = true;
            node3.IsFixed = true;

            MainViewModel.ProblemViewModel.SelectedNode = null;

            var element1 = MainViewModel.ProblemViewModel.AddBarElement(node1, node5);
            element1.Material.YoungsModulus = 1E9;
            element1.CrossSectionArea = 1E-3;

            var element2 = MainViewModel.ProblemViewModel.AddBarElement(node2, node5);
            element2.CrossSectionArea = 1E-3;

            var element3 = MainViewModel.ProblemViewModel.AddBarElement(node3, node5);
            element3.CrossSectionArea = 1E-3;

            var force = MainViewModel.ProblemViewModel.AddForce(node5);
            force.ApplicationVectorZ = -1000;

            MainViewModel.ProblemViewModel.SelectedElement = null;
            MainViewModel.ProblemViewModel.SelectedNode = null;
            MainViewModel.ProblemViewModel.SelectedForce = null;

            MainViewModel.InterfaceControlViewModel.HideAllCommand.Execute(null);
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