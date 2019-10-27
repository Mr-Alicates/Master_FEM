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
        public static readonly DependencyProperty MainViewModelProperty = DependencyProperty.Register(
            nameof(MainViewModel), typeof(MainViewModel), typeof(MainWindow));

        public MainViewModel MainViewModel
        {
            get { return (MainViewModel)this.GetValue(MainViewModelProperty); }
            set { this.SetValue(MainViewModelProperty, value); }
        }

        public MainWindow()
        {
            InitializeComponent();
            MainViewModel = new MainViewModel();
            DataContext = MainViewModel;

            Init();
            
            //I had to wire this events here because the events are not reaching the custom user control
            MouseWheel += CameraControl_MouseWheel;
            MouseMove += CameraControl_MouseMove;
            KeyDown += CameraControl_KeyboardKeyDown;
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
        }

        private void CameraControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!Viewport.IsMouseOver)
            {
                return;
            }

            MainViewModel.CameraControlViewModel.ReactToMouseMovement(e.MiddleButton, e.RightButton, e.GetPosition(Viewport));
        }

        private void CameraControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!Viewport.IsMouseOver)
            {
                return;
            }

            MainViewModel.CameraControlViewModel.ReactToMouseWheelMovement(e.Delta);
        }

        private void CameraControl_KeyboardKeyDown(object sender, KeyEventArgs e)
        {
            if (!Viewport.IsMouseOver)
            {
                return;
            }

            MainViewModel.CameraControlViewModel.ReactToKeyBoardKeyDown(Keyboard.IsKeyDown(Key.LeftShift), e.Key);
        }
    }
}
