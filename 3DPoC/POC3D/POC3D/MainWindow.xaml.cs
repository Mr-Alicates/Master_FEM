using POC3D.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POC3D
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel MainViewModel;

        public MainWindow()
        {
            InitializeComponent();
            MainViewModel = new MainViewModel();

            MainViewModel.Camera.OnCameraViewModelChanged += OnCameraViewModelChanged;
            MainViewModel.Problem.Nodes.CollectionChanged += RedrawProblem;
            MainViewModel.Problem.Elements.CollectionChanged += RedrawProblem;

            MainViewModel.Camera.Position = new Point3D(-100, 0, 0);

            var node1 = MainViewModel.Problem.AddNode(new Point3D(-10, -10, -10)).SetAsFixed();
            var node2 = MainViewModel.Problem.AddNode(new Point3D(10, -10, -10)).SetAsFixed();
            var node3 = MainViewModel.Problem.AddNode(new Point3D(10, 10, -10)).SetAsFixed();
            var node4 = MainViewModel.Problem.AddNode(new Point3D(-10, 10, -10)).SetAsFixed();

            var node5 = MainViewModel.Problem.AddNode(new Point3D(-10, -10, 10)).SetAsFixed();
            var node6 = MainViewModel.Problem.AddNode(new Point3D(10, -10, 10)).SetAsFixed();
            var node7 = MainViewModel.Problem.AddNode(new Point3D(10, 10, 10)).SetAsFixed();
            var node8 = MainViewModel.Problem.AddNode(new Point3D(-10, 10, 10)).SetAsFixed();


            MainViewModel.Problem.AddBarElement(node1, node2);
            MainViewModel.Problem.AddBarElement(node2, node3);
            MainViewModel.Problem.AddBarElement(node3, node4);
            MainViewModel.Problem.AddBarElement(node4, node1);

            MainViewModel.Problem.AddBarElement(node1, node5);
            MainViewModel.Problem.AddBarElement(node2, node6);
            MainViewModel.Problem.AddBarElement(node3, node7);
            MainViewModel.Problem.AddBarElement(node4, node8);

            MainViewModel.Problem.AddBarElement(node5, node6);
            MainViewModel.Problem.AddBarElement(node6, node7);
            MainViewModel.Problem.AddBarElement(node7, node8);
            MainViewModel.Problem.AddBarElement(node8, node5);
        }

        private void RedrawProblem(object sender, NotifyCollectionChangedEventArgs e)
        {
            //Remove all the elements
            Model3DGroup.Children.Clear();

            //Add general lighting to the scene
            AmbientLight ambientLight = new AmbientLight(Colors.Black);
            DirectionalLight directionalLight = new DirectionalLight(Colors.White, new Vector3D(0, 0, -1));

            Model3DGroup.Children.Add(ambientLight);
            Model3DGroup.Children.Add(directionalLight);

            foreach (var node in MainViewModel.Problem.Nodes)
            {
                Model3DGroup.Children.Add(node.Geometry);
            }

            foreach (var element in MainViewModel.Problem.Elements)
            {
                Model3DGroup.Children.Add(element.Geometry);
            }
        }



        private void OnCameraViewModelChanged(object sender, EventArgs e)
        {
            Camera.Position = MainViewModel.Camera.Position;
            Camera.LookDirection = MainViewModel.Camera.LookDirection;
            Camera.UpDirection = MainViewModel.Camera.UpDirection;
        }

        private void Viewport3D_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Q:
                    MainViewModel.Camera.RotateCounterClockwise();
                    break;
                case Key.E:
                    MainViewModel.Camera.RotateClockwise();
                    break;

                case Key.A:
                    MainViewModel.Camera.MoveLeft();
                    break;
                case Key.D:
                    MainViewModel.Camera.MoveRight();
                    break;
                case Key.W:
                    MainViewModel.Camera.MoveForward();
                    break;
                case Key.S:
                    MainViewModel.Camera.MoveBackwards();
                    break;

                case Key.Space:
                    MainViewModel.Camera.MoveUp();
                    break;
                case Key.C:
                    MainViewModel.Camera.MoveDown();
                    break;
            }
        }
    }
}
