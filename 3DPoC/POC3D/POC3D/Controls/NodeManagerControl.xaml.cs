using POC3D.ViewModel;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POC3D.Controls
{
    /// <summary>
    /// Interaction logic for NodeManagerControl.xaml
    /// </summary>
    public partial class NodeManagerControl : UserControl
    {

        public static readonly DependencyProperty ProblemViewModelProperty = DependencyProperty.Register(
            nameof(ProblemViewModel), typeof(ProblemViewModel), typeof(NodeManagerControl), new PropertyMetadata(null, Callback));

        public ProblemViewModel ProblemViewModel
        {
            get { return (ProblemViewModel)this.GetValue(ProblemViewModelProperty); }
            set
            {
                this.SetValue(ProblemViewModelProperty, value);
                DataContext = value;
            }
        }

        private static void Callback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NodeManagerControl control)
            {
                control.InitViewModel(e.NewValue as ProblemViewModel);
            }
        }

        private void InitViewModel(ProblemViewModel value)
        {
            DataContext = null;
        }

        public NodeManagerControl()
        {
            InitializeComponent();
        }
    }
}
