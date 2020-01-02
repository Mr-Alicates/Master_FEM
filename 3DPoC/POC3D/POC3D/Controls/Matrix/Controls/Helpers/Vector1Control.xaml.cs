using System.Windows;
using System.Windows.Controls;

namespace POC3D.Controls.Matrix.Controls.Helpers
{
    /// <summary>
    ///     Interaction logic for Vector1Control.xaml
    /// </summary>
    public partial class Vector1Control : UserControl
    {
        public static readonly DependencyProperty V1Property = DependencyProperty.Register(
            nameof(V1), typeof(string), typeof(Vector1Control),
            new PropertyMetadata(string.Empty, PropertyChangedCallback));


        public Vector1Control()
        {
            InitializeComponent();
        }

        public string V1
        {
            get => Position1.Text;
            set => SetValue(V1Property, value);
        }

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Vector1Control;

            if (control == null) return;

            var value = e.NewValue as string;
            control.Position1.Text = value;
        }
    }
}