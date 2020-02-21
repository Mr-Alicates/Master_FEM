using System.Linq;
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

            InitSimple();
        }

        public MainViewModel MainViewModel
        {
            get => (MainViewModel) GetValue(MainViewModelProperty);
            set => SetValue(MainViewModelProperty, value);
        }

        public void InitSolvedProblem()
        {
            //http://ocw.ump.edu.my/pluginfile.php/9827/mod_resource/content/2/10_Space_Truss_Example.pdf

            MainViewModel.CameraViewModel.Position = new Point3D(-100, 0, 0);

            var node1 = MainViewModel.ProblemViewModel.AddNode(new Point3D(0.32, 1.5, 0.1848)).SetAsFree();
            var node2 = MainViewModel.ProblemViewModel.AddNode(new Point3D(0, 0, 0)).SetAsFixed();
            var node3 = MainViewModel.ProblemViewModel.AddNode(new Point3D(0.64, 0, 0)).SetAsFixed();
            var node4 = MainViewModel.ProblemViewModel.AddNode(new Point3D(0.32, 0, 0.5543)).SetAsFixed();


            MainViewModel.ProblemViewModel.SelectedNode = null;

            var element1 = MainViewModel.ProblemViewModel.AddBarElement(node2, node1);
            element1.CrossSectionArea = 1.2566E-3;
            element1.Material = new MaterialViewModel(new Model.ModelMaterial("test", 1E7, 0));

            var element2 = MainViewModel.ProblemViewModel.AddBarElement(node1, node3);
            element2.CrossSectionArea = element1.CrossSectionArea;
            element2.Material = element1.Material;

            var element3 = MainViewModel.ProblemViewModel.AddBarElement(node1, node4);
            element3.CrossSectionArea = element1.CrossSectionArea;
            element3.Material = element1.Material;

            var force = MainViewModel.ProblemViewModel.AddForce(node1);
            force.ApplicationVectorX = 0;
            force.ApplicationVectorY = -200;
            force.ApplicationVectorZ = 0;

            MainViewModel.ProblemViewModel.SelectedElement = null;
            MainViewModel.ProblemViewModel.SelectedNode = null;
            MainViewModel.ProblemViewModel.SelectedForce = null;

            MainViewModel.InterfaceControlViewModel.HideAllCommand.Execute(null);
        }

        public void InitSimple()
        {
            MainViewModel.CameraViewModel.Position = new Point3D(-100, 0, 0);

            var node1 = MainViewModel.ProblemViewModel.AddNode(new Point3D(-10, -10, -10)).SetAsFixed();
            var node5 = MainViewModel.ProblemViewModel.AddNode(new Point3D(0, 0, 10)).SetAsFree();
            var node2 = MainViewModel.ProblemViewModel.AddNode(new Point3D(10, -10, -10)).SetAsFixed();
            var node3 = MainViewModel.ProblemViewModel.AddNode(new Point3D(10, 10, -10)).SetAsFixed();


            MainViewModel.ProblemViewModel.SelectedNode = null;

            var element1 = MainViewModel.ProblemViewModel.AddBarElement(node1, node5);
            element1.Material= MainViewModel.ProblemViewModel.Materials.Last();
            element1.CrossSectionArea = 1E-3;

            var element2 = MainViewModel.ProblemViewModel.AddBarElement(node2, node5);
            element2.Material = MainViewModel.ProblemViewModel.Materials.Last();
            element2.CrossSectionArea = 1E-3;

            var element3 = MainViewModel.ProblemViewModel.AddBarElement(node3, node5);
            element3.Material = MainViewModel.ProblemViewModel.Materials.Last();
            element3.CrossSectionArea = 1E-3;

            var force = MainViewModel.ProblemViewModel.AddForce(node5);
            force.ApplicationVectorX = -10;
            force.ApplicationVectorY = -10;
            force.ApplicationVectorZ = -10;

            MainViewModel.ProblemViewModel.SelectedElement = null;
            MainViewModel.ProblemViewModel.SelectedNode = null;
            MainViewModel.ProblemViewModel.SelectedForce = null;

            MainViewModel.InterfaceControlViewModel.HideAllCommand.Execute(null);
        }

        public void InitCube()
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

            MainViewModel.ProblemViewModel.AddBarElement(node1, node6);
            MainViewModel.ProblemViewModel.AddBarElement(node2, node7);
            MainViewModel.ProblemViewModel.AddBarElement(node3, node8);
            MainViewModel.ProblemViewModel.AddBarElement(node4, node5);


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

        private void ShowGlobalMatrixInfoWindow(object sender, RoutedEventArgs e)
        {
            var globalMatrixInfoWindow = new GlobalMatrixInfoWindow
            {
                DataContext = DataContext
            };

            globalMatrixInfoWindow.Show();
        }
    }
}