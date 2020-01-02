using System.Windows;
using System.Windows.Controls;

namespace POC3D.Controls.Matrix.Controls.Helpers
{
    /// <summary>
    ///     Interaction logic for ScalarControl.xaml
    /// </summary>
    public partial class ScalarControl : UserControl
    {
        public static readonly DependencyProperty ScalarProperty = DependencyProperty.Register(
            nameof(Scalar), typeof(string), typeof(ScalarControl),
            new PropertyMetadata(string.Empty, M11PropertyChangedCallback));


        public ScalarControl()
        {
            InitializeComponent();
        }

        public string Scalar
        {
            get => Position1.Text;
            set => SetValue(ScalarProperty, value);
        }

        private static void M11PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ScalarControl;

            control.Position1.Text = e.NewValue as string;
        }
    }
}