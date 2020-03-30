using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using POC3D.ViewModel;
using POC3D.ViewModel.Geometry;
using POC3D.ViewModel.Implementation;

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
            Viewport.PreviewMouseDown += ViewPortClicked;

            //Events to wire the CameraViewmodel to this control
            var cameraVM = MainViewModel.CameraViewModel;
            Viewport.PreviewMouseMove += (_, e) => cameraVM.ReactToMouseMovement(e.GetPosition(Viewport));
            Viewport.PreviewMouseWheel += (_, e) => cameraVM.ReactToMouseWheelMovement(e.Delta);
            Viewport.PreviewMouseDown += (_, e) => cameraVM.ReactToMouseDown(e.ChangedButton);
            Viewport.PreviewMouseUp += (_, e) => cameraVM.ReactToMouseUp(e.ChangedButton);
            Viewport.PreviewKeyDown += (_, e) => cameraVM.ReactToKeyboardKeyDown(e.Key);
            Viewport.PreviewKeyUp += (_, e) => cameraVM.ReactToKeyboardKeyUp(e.Key);

            MainViewModel.CameraViewModel.OnCameraViewModelChanged += (a, b) => UpdateCamera();

            MainViewModel.ProblemViewModel.Nodes.CollectionChanged += ProblemNodesChanged;
            MainViewModel.ProblemViewModel.Elements.CollectionChanged += ProblemElementsChanged;
            MainViewModel.ProblemViewModel.Forces.CollectionChanged += ProblemForcesChanged;

            MainViewModel.ProblemViewModel.PropertyChanged += ShowProblemChangedCallback;

            UpdateCamera();
            DisplayProblem();
            Focus();
        }

        private void ShowProblemChangedCallback(object sender, PropertyChangedEventArgs e)
        {
            if (MainViewModel.ProblemViewModel.ProblemCalculationViewModel.ShowProblem)
                DisplayProblem();
            else
                DisplayResults();
        }

        private void ViewPortClicked(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton != MouseButton.Left)
            {
                return;
            }

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

            foreach (var node in MainViewModel.ProblemViewModel.Nodes) Model3DGroup.Children.Add(node.ResultGeometry);

            foreach (var element in MainViewModel.ProblemViewModel.Elements)
                Model3DGroup.Children.Add(element.ResultGeometry);

            foreach (var force in MainViewModel.ProblemViewModel.Forces)
                Model3DGroup.Children.Add(force.ResultGeometry);
        }

        public void UpdateCamera()
        {
            Camera.Position = MainViewModel.CameraViewModel.Position;
            Camera.UpDirection = MainViewModel.CameraViewModel.UpDirection;
            Camera.LookDirection = MainViewModel.CameraViewModel.UnaryForward;
        }

        private void ProblemNodesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!MainViewModel.ProblemViewModel.ProblemCalculationViewModel.ShowProblem) return;

            if (e.NewItems != null)
                foreach (NodeViewModel node in e.NewItems)
                    Model3DGroup.Children.Add(node.Geometry);

            if (e.OldItems != null)
                foreach (NodeViewModel node in e.OldItems)
                    Model3DGroup.Children.Remove(node.Geometry);
        }

        private void ProblemElementsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!MainViewModel.ProblemViewModel.ProblemCalculationViewModel.ShowProblem) return;

            if (e.NewItems != null)
                foreach (ElementViewModel element in e.NewItems)
                    Model3DGroup.Children.Add(element.Geometry);

            if (e.OldItems != null)
                foreach (ElementViewModel element in e.OldItems)
                    Model3DGroup.Children.Remove(element.Geometry);
        }

        private void ProblemForcesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!MainViewModel.ProblemViewModel.ProblemCalculationViewModel.ShowProblem) return;

            if (e.NewItems != null)
                foreach (ForceViewModel force in e.NewItems)
                    Model3DGroup.Children.Add(force.Geometry);

            if (e.OldItems != null)
                foreach (ForceViewModel force in e.OldItems)
                    Model3DGroup.Children.Remove(force.Geometry);
        }
    }
}