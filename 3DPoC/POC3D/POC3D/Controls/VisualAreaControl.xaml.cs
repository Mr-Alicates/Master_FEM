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
            Viewport.MouseWheel += CameraControl_MouseWheel;
            Viewport.MouseMove += CameraControl_MouseMove;
            Viewport.KeyDown += CameraControl_KeyboardKeyDown;


            MainViewModel.Camera.OnCameraViewModelChanged += (a, b) => UpdateCamera();

            MainViewModel.Problem.SelectedNodeChanged += (a, b) => UpdateProblem();
            MainViewModel.Problem.Nodes.CollectionChanged += (a, b) => UpdateProblem();
            MainViewModel.Problem.Elements.CollectionChanged += (a, b) => UpdateProblem();

            UpdateCamera();
            UpdateProblem();
        }

        private void CameraControl_MouseMove(object sender, MouseEventArgs e)
        {
            MainViewModel.CameraControlViewModel.ReactToMouseMovement(e.MiddleButton, e.RightButton, e.GetPosition(this));
        }

        private void CameraControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            MainViewModel.CameraControlViewModel.ReactToMouseWheelMovement(e.Delta);
        }

        private void CameraControl_KeyboardKeyDown(object sender, KeyEventArgs e)
        {
            MainViewModel.CameraControlViewModel.ReactToKeyBoardKeyDown(Keyboard.IsKeyDown(Key.LeftShift), e.Key);
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

            foreach (var node in MainViewModel.Problem.Nodes)
            {
                Model3DGroup.Children.Add(node.Geometry);
            }

            foreach (var element in MainViewModel.Problem.Elements)
            {
                Model3DGroup.Children.Add(element.Geometry);
            }
        }

        public void UpdateCamera()
        {
            Camera.Position = MainViewModel.Camera.Position;
            Camera.UpDirection = MainViewModel.Camera.UpDirection;
            Camera.LookDirection = MainViewModel.Camera.LookDirection;
        }
    }
}
