using System.Windows;
using System.Windows.Controls;

namespace POC3D.Controls.Matrix.Controls.Helpers
{
    /// <summary>
    ///     Interaction logic for Vector2Control.xaml
    /// </summary>
    public partial class Vector2Control : UserControl
    {
        public static readonly DependencyProperty V1Property = DependencyProperty.Register(
            nameof(V1), typeof(string), typeof(Vector2Control),
            new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty V2Property = DependencyProperty.Register(
            nameof(V2), typeof(string), typeof(Vector2Control),
            new PropertyMetadata(string.Empty, PropertyChangedCallback));


        public Vector2Control()
        {
            InitializeComponent();
        }

        public string V1
        {
            get => Position1.Text;
            set => SetValue(V1Property, value);
        }

        public string V2
        {
            get => Position2.Text;
            set => SetValue(V2Property, value);
        }

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Vector2Control;

            if (control == null)
            {
                return;
            }

            var value = e.NewValue as string;

            switch (e.Property.Name)
            {
                case nameof(V1): control.Position1.Text = value; break;
                case nameof(V2): control.Position2.Text = value; break;
            }
        }
    }
}