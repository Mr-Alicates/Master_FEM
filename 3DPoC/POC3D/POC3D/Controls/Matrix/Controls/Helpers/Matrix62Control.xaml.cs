using System.Windows;
using System.Windows.Controls;
using POC3D.Model.Calculations;

namespace POC3D.Controls.Matrix.Controls.Helpers
{
    /// <summary>
    ///     Interaction logic for Matrix62Control.xaml
    /// </summary>
    public partial class Matrix62Control : UserControl
    {
        public static readonly DependencyProperty M11Property = DependencyProperty.Register(nameof(M11), typeof(string),
            typeof(Matrix62Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M12Property = DependencyProperty.Register(nameof(M12), typeof(string),
            typeof(Matrix62Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M21Property = DependencyProperty.Register(nameof(M21), typeof(string),
            typeof(Matrix62Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M22Property = DependencyProperty.Register(nameof(M22), typeof(string),
            typeof(Matrix62Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M31Property = DependencyProperty.Register(nameof(M31), typeof(string),
            typeof(Matrix62Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M32Property = DependencyProperty.Register(nameof(M32), typeof(string),
            typeof(Matrix62Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M41Property = DependencyProperty.Register(nameof(M41), typeof(string),
            typeof(Matrix62Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M42Property = DependencyProperty.Register(nameof(M42), typeof(string),
            typeof(Matrix62Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M51Property = DependencyProperty.Register(nameof(M51), typeof(string),
            typeof(Matrix62Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M52Property = DependencyProperty.Register(nameof(M52), typeof(string),
            typeof(Matrix62Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M61Property = DependencyProperty.Register(nameof(M61), typeof(string),
            typeof(Matrix62Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M62Property = DependencyProperty.Register(nameof(M62), typeof(string),
            typeof(Matrix62Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty MatrixProperty = DependencyProperty.Register(nameof(Matrix),
            typeof(NumericMatrix), typeof(Matrix62Control),
            new PropertyMetadata(null, MatrixChangedCallback));

        public Matrix62Control()
        {
            InitializeComponent();
            InitializeAllToZero();
        }

        public NumericMatrix Matrix { get; set; }

        public string M11
        {
            get => Position11.Text;
            set => SetValue(M11Property, value);
        }

        public string M12
        {
            get => Position12.Text;
            set => SetValue(M12Property, value);
        }

        public string M21
        {
            get => Position21.Text;
            set => SetValue(M21Property, value);
        }

        public string M22
        {
            get => Position22.Text;
            set => SetValue(M22Property, value);
        }

        public string M31
        {
            get => Position31.Text;
            set => SetValue(M31Property, value);
        }

        public string M32
        {
            get => Position32.Text;
            set => SetValue(M32Property, value);
        }

        public string M41
        {
            get => Position41.Text;
            set => SetValue(M41Property, value);
        }

        public string M42
        {
            get => Position42.Text;
            set => SetValue(M42Property, value);
        }

        public string M51
        {
            get => Position51.Text;
            set => SetValue(M51Property, value);
        }

        public string M52
        {
            get => Position52.Text;
            set => SetValue(M52Property, value);
        }

        public string M61
        {
            get => Position61.Text;
            set => SetValue(M61Property, value);
        }

        public string M62
        {
            get => Position62.Text;
            set => SetValue(M62Property, value);
        }

        private static void MatrixChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Matrix62Control;

            if (control == null) return;

            var value = e.NewValue as NumericMatrix;

            if (value == null)
            {
                control.InitializeAllToZero();
                return;
            }

            control.M11 = value[0, 0].ToString("E2");
            control.M12 = value[0, 1].ToString("E2");

            control.M21 = value[1, 0].ToString("E2");
            control.M22 = value[1, 1].ToString("E2");

            control.M31 = value[2, 0].ToString("E2");
            control.M32 = value[2, 1].ToString("E2");

            control.M41 = value[3, 0].ToString("E2");
            control.M42 = value[3, 1].ToString("E2");

            control.M51 = value[4, 0].ToString("E2");
            control.M52 = value[4, 1].ToString("E2");

            control.M61 = value[5, 0].ToString("E2");
            control.M62 = value[5, 1].ToString("E2");
        }

        private void InitializeAllToZero()
        {
            M11 = "0";
            M12 = "0";

            M21 = "0";
            M22 = "0";

            M31 = "0";
            M32 = "0";

            M41 = "0";
            M42 = "0";

            M51 = "0";
            M52 = "0";

            M61 = "0";
            M62 = "0";
        }

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Matrix62Control;

            if (control == null) return;

            var value = e.NewValue as string;

            switch (e.Property.Name)
            {
                case nameof(M11):
                    control.Position11.Text = value;
                    break;
                case nameof(M12):
                    control.Position12.Text = value;
                    break;

                case nameof(M21):
                    control.Position21.Text = value;
                    break;
                case nameof(M22):
                    control.Position22.Text = value;
                    break;

                case nameof(M31):
                    control.Position31.Text = value;
                    break;
                case nameof(M32):
                    control.Position32.Text = value;
                    break;

                case nameof(M41):
                    control.Position41.Text = value;
                    break;
                case nameof(M42):
                    control.Position42.Text = value;
                    break;

                case nameof(M51):
                    control.Position51.Text = value;
                    break;
                case nameof(M52):
                    control.Position52.Text = value;
                    break;

                case nameof(M61):
                    control.Position61.Text = value;
                    break;
                case nameof(M62):
                    control.Position62.Text = value;
                    break;
            }
        }
    }
}