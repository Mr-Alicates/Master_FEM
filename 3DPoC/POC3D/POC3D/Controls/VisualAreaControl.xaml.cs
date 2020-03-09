using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using POC3D.Helpers;
using POC3D.ViewModel;

namespace POC3D.Controls
{
    /// <summary>
    ///     Lógica de interacción para VisualAreaControl.xaml
    /// </summary>
    public partial class VisualAreaControl : UserControl
    {
        public static readonly DependencyProperty MainViewModelProperty = DependencyProperty.Register(
            nameof(MainViewModel), typeof(MainViewModel), typeof(VisualAreaControl),
            new PropertyMetadata(null, Callback));

        public VisualAreaControl()
        {
            InitializeComponent();
        }

        public MainViewModel MainViewModel
        {
            get => (MainViewModel) GetValue(MainViewModelProperty);
            set => SetValue(MainViewModelProperty, value);
        }

        private static void Callback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is VisualAreaControl control) control.InitViewModel();
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
            MainViewModel.ProblemViewModel.Forces.CollectionChanged += ProblemForcesChanged;

            MainViewModel.ProblemViewModel.PropertyChanged += ShowProblemChangedCallback;

            UpdateCamera();
            DisplayProblem();
        }

        private void ShowProblemChangedCallback(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(ProblemViewModel.ShowProblem) &&
                e.PropertyName != nameof(ProblemViewModel.DisplacementsMultiplier))
                return;

            if (MainViewModel.ProblemViewModel.ShowProblem)
                DisplayProblem();
            else
                DisplayResults();
        }

        private void Viewport_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Focus();

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

                foreach (var force in MainViewModel.ProblemViewModel.Forces)
                {
                    var forceGeometry = force.Geometry.Geometry as MeshGeometry3D;

                    if (geometry.Equals(forceGeometry))
                    {
                        MainViewModel.ProblemViewModel.SelectedForce = force;
                        break;
                    }
                }
            }
        }

        private void Viewport_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            MainViewModel.InterfaceControlViewModel.ReactToMouseMovement(e.MiddleButton, e.RightButton,
                e.GetPosition(Viewport));
        }

        private void Viewport_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            MainViewModel.InterfaceControlViewModel.ReactToKeyBoardKeyDown(Keyboard.IsKeyDown(Key.LeftShift), e.Key);
        }

        private void Viewport_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            MainViewModel.InterfaceControlViewModel.ReactToMouseWheelMovement(e.Delta);
        }

        private void InitialSetup()
        {
            //Remove all the elements
            Model3DGroup.Children.Clear();

            //Add general lighting to the scene
            var ambientLight = new AmbientLight(Colors.Black);
            var directionalLight = new DirectionalLight(Colors.White, new Vector3D(0, 0, -1));

            var origin = GraphicsHelper.BuildOrigin();
            Model3DGroup.Children.Add(origin);

            Model3DGroup.Children.Add(ambientLight);
            Model3DGroup.Children.Add(directionalLight);
        }

        public void DisplayProblem()
        {
            InitialSetup();

            foreach (var node in MainViewModel.ProblemViewModel.Nodes) Model3DGroup.Children.Add(node.Geometry);

            foreach (var element in MainViewModel.ProblemViewModel.Elements)
                Model3DGroup.Children.Add(element.Geometry);

            foreach (var force in MainViewModel.ProblemViewModel.Forces) Model3DGroup.Children.Add(force.Geometry);
        }

        public void DisplayResults()
        {
            InitialSetup();

            foreach (var node in MainViewModel.ProblemViewModel.ResultNodes) Model3DGroup.Children.Add(node.Geometry);

            foreach (var element in MainViewModel.ProblemViewModel.ResultElements)
                Model3DGroup.Children.Add(element.Geometry);

            foreach (var force in MainViewModel.ProblemViewModel.ResultForces)
                Model3DGroup.Children.Add(force.Geometry);
        }

        public void UpdateCamera()
        {
            Camera.Position = MainViewModel.CameraViewModel.Position;
            Camera.UpDirection = MainViewModel.CameraViewModel.UpDirection;
            Camera.LookDirection = MainViewModel.CameraViewModel.UnaryForward;
        }

        private void ProblemNodesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!MainViewModel.ProblemViewModel.ShowProblem) return;

            if (e.NewItems != null)
                foreach (NodeViewModel node in e.NewItems)
                    Model3DGroup.Children.Add(node.Geometry);

            if (e.OldItems != null)
                foreach (NodeViewModel node in e.OldItems)
                    Model3DGroup.Children.Remove(node.Geometry);
        }

        private void ProblemElementsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!MainViewModel.ProblemViewModel.ShowProblem) return;

            if (e.NewItems != null)
                foreach (ElementViewModel element in e.NewItems)
                    Model3DGroup.Children.Add(element.Geometry);

            if (e.OldItems != null)
                foreach (ElementViewModel element in e.OldItems)
                    Model3DGroup.Children.Remove(element.Geometry);
        }

        private void ProblemForcesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!MainViewModel.ProblemViewModel.ShowProblem) return;

            if (e.NewItems != null)
                foreach (ForceViewModel force in e.NewItems)
                    Model3DGroup.Children.Add(force.Geometry);

            if (e.OldItems != null)
                foreach (ForceViewModel force in e.OldItems)
                    Model3DGroup.Children.Remove(force.Geometry);
        }
    }
}