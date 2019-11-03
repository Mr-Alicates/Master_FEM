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
        }
    }
}
