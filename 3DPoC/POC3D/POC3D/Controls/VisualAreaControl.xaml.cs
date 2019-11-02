using POC3D.ViewModel;
using POC3D.ViewModel.Camera;
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

namespace POC3D.Controls
{
    /// <summary>
    /// Lógica de interacción para VisualAreaControl.xaml
    /// </summary>
    public partial class VisualAreaControl : UserControl
    {
        public static readonly DependencyProperty MainViewModelProperty = DependencyProperty.Register(
          nameof(MainViewModel), typeof(MainViewModel), typeof(VisualAreaControl), new PropertyMetadata(null, Callback));

        public VisualAreaControl()
        {
            InitializeComponent();
        }

        public MainViewModel MainViewModel
        {
            get { return (MainViewModel)this.GetValue(MainViewModelProperty); }
            set { this.SetValue(MainViewModelProperty, value); }
        }

        private static void Callback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is VisualAreaControl control)
            {
                control.InitViewModel();
            }
        }

        public void InitViewModel()
        {
            Viewport.PreviewMouseMove += Viewport_PreviewMouseMove;
            Viewport.PreviewMouseWheel += Viewport_PreviewMouseWheel;
            Viewport.PreviewKeyDown += Viewport_PreviewKeyDown;
            Viewport.PreviewMouseDown += Viewport_PreviewMouseDown;

            MainViewModel.CameraViewModel.OnCameraViewModelChanged += (a, b) => UpdateCamera();

            MainViewModel.ProblemViewModel.Nodes.CollectionChanged += ProblemNodesChanged;
            MainViewModel.ProblemViewModel.Elements.CollectionChanged += ProblemElementsChanged;

            UpdateCamera();
            UpdateProblem();
        }

        private void Viewport_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Focus();

            var ptMouse = e.GetPosition(Viewport);
            var result = VisualTreeHelper.HitTest(Viewport, ptMouse);

            if (result is RayMeshGeometry3DHitTestResult meshResult)
            {
                var geometry = meshResult.MeshHit;

                foreach (var node in MainViewModel.ProblemViewModel.Nodes)
                {
                    var nodeGeometry = node.Geometry.Geometry as MeshGeometry3D;

                    if (geometry.Equals(nodeGeometry))
                    {
                        MainViewModel.ProblemViewModel.SelectedNode = node;
                        break;
                    }
                }

                foreach (var element in MainViewModel.ProblemViewModel.Elements)
                {
                    var elementGeometry = element.Geometry.Geometry as MeshGeometry3D;

                    if (geometry.Equals(elementGeometry))
                    {
                        MainViewModel.ProblemViewModel.SelectedElement = element;
                        break;
                    }
                }
            }
        }

        private void Viewport_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            MainViewModel.InterfaceControlViewModel.ReactToMouseMovement(e.MiddleButton, e.RightButton, e.GetPosition(Viewport));
        }

        private void Viewport_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            MainViewModel.InterfaceControlViewModel.ReactToKeyBoardKeyDown(Keyboard.IsKeyDown(Key.LeftShift), e.Key);
        }

        private void Viewport_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            MainViewModel.InterfaceControlViewModel.ReactToMouseWheelMovement(e.Delta);
        }

        public void UpdateProblem()
        {
            //Remove all the elements
            Model3DGroup.Children.Clear();

            //Add general lighting to the scene
            AmbientLight ambientLight = new AmbientLight(Colors.Black);
            DirectionalLight directionalLight = new DirectionalLight(Colors.White, new Vector3D(0, 0, -1));

            Model3DGroup.Children.Add(ambientLight);
            Model3DGroup.Children.Add(directionalLight);

            foreach (var node in MainViewModel.ProblemViewModel.Nodes)
            {
                Model3DGroup.Children.Add(node.Geometry);
            }

            foreach (var element in MainViewModel.ProblemViewModel.Elements)
            {
                Model3DGroup.Children.Add(element.Geometry);
            }
        }

        public void UpdateCamera()
        {
            Camera.Position = MainViewModel.CameraViewModel.Position;
            Camera.UpDirection = MainViewModel.CameraViewModel.UpDirection;
            Camera.LookDirection = MainViewModel.CameraViewModel.LookDirection;
        }

        private void ProblemNodesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (NodeViewModel node in e.NewItems)
                {
                    Model3DGroup.Children.Add(node.Geometry);
                }
            }

            if (e.OldItems != null)
            {
                foreach (NodeViewModel node in e.OldItems)
                {
                    Model3DGroup.Children.Remove(node.Geometry);
                }
            }
        }

        private void ProblemElementsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (ElementViewModel element in e.NewItems)
                {
                    Model3DGroup.Children.Add(element.Geometry);
                }
            }

            if (e.OldItems != null)
            {
                foreach (ElementViewModel element in e.OldItems)
                {
                    Model3DGroup.Children.Remove(element.Geometry);
                }
            }
        }
    }
}
