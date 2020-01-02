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

        public MainWindow()
        {
            InitializeComponent();
            MainViewModel = new MainViewModel();
            DataContext = MainViewModel;

            Init();
        }

        public MainViewModel MainViewModel
        {
            get => (MainViewModel) GetValue(MainViewModelProperty);
            set => SetValue(MainViewModelProperty, value);
        }

        public void Init()
        {
            MainViewModel.CameraViewModel.Position = new Point3D(-100, 0, 0);

            var node1 = MainViewModel.ProblemViewModel.AddNode(new Point3D(-10, -10, -10)).SetAsFixed();
            var node2 = MainViewModel.ProblemViewModel.AddNode(new Point3D(10, -10, -10)).SetAsFixed();
            var node3 = MainViewModel.ProblemViewModel.AddNode(new Point3D(10, 10, -10)).SetAsFixed();
            var node4 = MainViewModel.ProblemViewModel.AddNode(new Point3D(-10, 10, -10)).SetAsFixed();

            var node5 = MainViewModel.ProblemViewModel.AddNode(new Point3D(-10, -10, 10)).SetAsFree();
            var node6 = MainViewModel.ProblemViewModel.AddNode(new Point3D(10, -10, 10)).SetAsFree();
            var node7 = MainViewModel.ProblemViewModel.AddNode(new Point3D(10, 10, 10)).SetAsFree();
            var node8 = MainViewModel.ProblemViewModel.AddNode(new Point3D(-10, 10, 10)).SetAsFree();

            MainViewModel.ProblemViewModel.SelectedNode = null;

            MainViewModel.ProblemViewModel.AddBarElement(node1, node2);
            MainViewModel.ProblemViewModel.AddBarElement(node2, node3);
            MainViewModel.ProblemViewModel.AddBarElement(node3, node4);
            MainViewModel.ProblemViewModel.AddBarElement(node4, node1);

            MainViewModel.ProblemViewModel.AddBarElement(node1, node5);
            MainViewModel.ProblemViewModel.AddBarElement(node2, node6);
            MainViewModel.ProblemViewModel.AddBarElement(node3, node7);
            MainViewModel.ProblemViewModel.AddBarElement(node4, node8);

            MainViewModel.ProblemViewModel.AddBarElement(node5, node6);
            MainViewModel.ProblemViewModel.AddBarElement(node6, node7);
            MainViewModel.ProblemViewModel.AddBarElement(node7, node8);
            MainViewModel.ProblemViewModel.AddBarElement(node8, node5);

            var force = MainViewModel.ProblemViewModel.AddForce(node8);
            force.ApplicationVectorX = -10;
            force.ApplicationVectorY = -10;
            force.ApplicationVectorZ = -10;

            MainViewModel.ProblemViewModel.SelectedElement = null;
            MainViewModel.ProblemViewModel.SelectedNode = null;
            MainViewModel.ProblemViewModel.SelectedForce = null;

            MainViewModel.InterfaceControlViewModel.HideAllCommand.Execute(null);
        }

        private void ShowMatrixInfoWindow(object sender, RoutedEventArgs e)
        {
            var matrixInfoWindow = new MatrixInfoWindow
            {
                DataContext = DataContext
            };

            matrixInfoWindow.Show();
        }
    }
}