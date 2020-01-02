using System.Windows;
using System.Windows.Controls;

namespace POC3D.Controls.Matrix.Controls.Helpers
{
    /// <summary>
    ///     Interaction logic for Vector6Control.xaml
    /// </summary>
    public partial class Vector6Control : UserControl
    {
        public static readonly DependencyProperty V1Property = DependencyProperty.Register(
            nameof(V1), typeof(string), typeof(Vector6Control),
            new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty V2Property = DependencyProperty.Register(
            nameof(V2), typeof(string), typeof(Vector6Control),
            new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty V3Property = DependencyProperty.Register(
            nameof(V3), typeof(string), typeof(Vector6Control),
            new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty V4Property = DependencyProperty.Register(
            nameof(V4), typeof(string), typeof(Vector6Control),
            new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty V5Property = DependencyProperty.Register(
            nameof(V5), typeof(string), typeof(Vector6Control),
            new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty V6Property = DependencyProperty.Register(
            nameof(V6), typeof(string), typeof(Vector6Control),
            new PropertyMetadata(string.Empty, PropertyChangedCallback));


        public Vector6Control()
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

        public string V3
        {
            get => Position3.Text;
            set => SetValue(V3Property, value);
        }

        public string V4
        {
            get => Position4.Text;
            set => SetValue(V4Property, value);
        }

        public string V5
        {
            get => Position5.Text;
            set => SetValue(V5Property, value);
        }

        public string V6
        {
            get => Position6.Text;
            set => SetValue(V6Property, value);
        }

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Vector6Control;

            if (control == null)
            {
                return;
            }

            var value = e.NewValue as string;

            switch (e.Property.Name)
            {
                case nameof(V1): control.Position1.Text = value; break;
                case nameof(V2): control.Position2.Text = value; break;
                case nameof(V3): control.Position3.Text = value; break;
                case nameof(V4): control.Position4.Text = value; break;
                case nameof(V5): control.Position5.Text = value; break;
                case nameof(V6): control.Position6.Text = value; break;
            }
        }
    }
}