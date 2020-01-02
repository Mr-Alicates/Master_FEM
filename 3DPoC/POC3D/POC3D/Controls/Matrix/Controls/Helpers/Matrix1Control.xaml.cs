using System.Windows;
using System.Windows.Controls;

namespace POC3D.Controls.Matrix.Controls.Helpers
{
    /// <summary>
    ///     Interaction logic for Matrix1Control.xaml
    /// </summary>
    public partial class Matrix1Control : UserControl
    {
        public static readonly DependencyProperty M11Property = DependencyProperty.Register(
            nameof(M11), typeof(string), typeof(Matrix1Control),
            new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public Matrix1Control()
        {
            InitializeComponent();
        }

        public string M11
        {
            get => Position11.Text;
            set => SetValue(M11Property, value);
        }

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Matrix1Control;

            if (control == null)
            {
                return;
            }

            var value = e.NewValue as string;
            control.Position11.Text = value;
        }
    }
}