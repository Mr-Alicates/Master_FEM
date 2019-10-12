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
            MainViewModel.Body.Elements.CollectionChanged += OnBodyViewModelChanged;

            MainViewModel.Camera.Position = new Point3D(-100, 0, 0);
            MainViewModel.Body.Elements.Add(new ElementViewModel(new Point3D(0, 10, 0), 5));
            MainViewModel.Body.Elements.Add(new ElementViewModel(new Point3D(0, 20, 0), 5));
            MainViewModel.Body.Elements.Add(new ElementViewModel(new Point3D(0, 30, 0), 5));
        }

        private void OnBodyViewModelChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //Remove all the elements
            Model3DGroup.Children.Clear();

            //Add general lighting to the scene
            AmbientLight ambientLight = new AmbientLight(Colors.Black);
            DirectionalLight directionalLight = new DirectionalLight(Colors.White, new Vector3D(0, 0, -1));

            Model3DGroup.Children.Add(ambientLight);
            Model3DGroup.Children.Add(directionalLight);

            foreach (var element in MainViewModel.Body.Elements)
            {
                var cube = BuildCube3D(element);
                Model3DGroup.Children.Add(cube);
            }

        }

        private GeometryModel3D BuildCube3D(ElementViewModel elementViewModel)
        {
            GeometryModel3D result = new GeometryModel3D();
            result.Material = new DiffuseMaterial(Brushes.Red);

            result.Geometry = new MeshGeometry3D()
            {
                Positions = new Point3DCollection()
                {
                    elementViewModel.Center + new Vector3D(-1, -1, -1) * elementViewModel.HalfSize,
                    elementViewModel.Center + new Vector3D(1, -1, -1) * elementViewModel.HalfSize,
                    elementViewModel.Center + new Vector3D(1, 1, -1) * elementViewModel.HalfSize,
                    elementViewModel.Center + new Vector3D(-1, 1, -1) * elementViewModel.HalfSize,

                    elementViewModel.Center + new Vector3D(-1, -1, 1) * elementViewModel.HalfSize,
                    elementViewModel.Center + new Vector3D(1, -1, 1) * elementViewModel.HalfSize,
                    elementViewModel.Center + new Vector3D(1, 1, 1) * elementViewModel.HalfSize,
                    elementViewModel.Center + new Vector3D(-1, 1, 1) * elementViewModel.HalfSize,
                },
                TriangleIndices = new Int32Collection()
                {
                    //Bottom
                    0,3,1,
                    3,2,1,

                    //Top
                    7,4,5,
                    7,5,6,

                    //Left
                    0,5,4,
                    0,1,5,

                    //Right
                    7,6,3,
                    3,6,2,

                    //Back
                    1,2,5,
                    5,2,6,

                    //Front
                    7,3,4,
                    4,3,0,
                }
            };


            return result;
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
