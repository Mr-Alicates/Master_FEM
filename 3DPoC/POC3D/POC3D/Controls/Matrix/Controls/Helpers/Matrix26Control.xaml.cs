using System.Windows;
using System.Windows.Controls;

namespace POC3D.Controls.Matrix.Controls.Helpers
{
    /// <summary>
    ///     Interaction logic for Matrix26Control.xaml
    /// </summary>
    public partial class Matrix26Control : UserControl
    {
        public static readonly DependencyProperty M11Property = DependencyProperty.Register(nameof(M11), typeof(string),
            typeof(Matrix26Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M12Property = DependencyProperty.Register(nameof(M12), typeof(string),
            typeof(Matrix26Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M13Property = DependencyProperty.Register(nameof(M13), typeof(string),
            typeof(Matrix26Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M14Property = DependencyProperty.Register(nameof(M14), typeof(string),
            typeof(Matrix26Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M15Property = DependencyProperty.Register(nameof(M15), typeof(string),
            typeof(Matrix26Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M16Property = DependencyProperty.Register(nameof(M16), typeof(string),
            typeof(Matrix26Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M21Property = DependencyProperty.Register(nameof(M21), typeof(string),
            typeof(Matrix26Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M22Property = DependencyProperty.Register(nameof(M22), typeof(string),
            typeof(Matrix26Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M23Property = DependencyProperty.Register(nameof(M23), typeof(string),
            typeof(Matrix26Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M24Property = DependencyProperty.Register(nameof(M24), typeof(string),
            typeof(Matrix26Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M25Property = DependencyProperty.Register(nameof(M25), typeof(string),
            typeof(Matrix26Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M26Property = DependencyProperty.Register(nameof(M26), typeof(string),
            typeof(Matrix26Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty MatrixProperty = DependencyProperty.Register(nameof(Matrix),
            typeof(Model.Calculations.NumericMatrix), typeof(Matrix26Control),
            new PropertyMetadata(null, MatrixChangedCallback));

        public Matrix26Control()
        {
            InitializeComponent();
            InitializeAllToZero();
        }

        public Model.Calculations.NumericMatrix Matrix { get; set; }

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

        public string M13
        {
            get => Position13.Text;
            set => SetValue(M13Property, value);
        }

        public string M14
        {
            get => Position14.Text;
            set => SetValue(M14Property, value);
        }

        public string M15
        {
            get => Position15.Text;
            set => SetValue(M15Property, value);
        }

        public string M16
        {
            get => Position16.Text;
            set => SetValue(M16Property, value);
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

        public string M23
        {
            get => Position23.Text;
            set => SetValue(M23Property, value);
        }

        public string M24
        {
            get => Position24.Text;
            set => SetValue(M24Property, value);
        }

        public string M25
        {
            get => Position25.Text;
            set => SetValue(M25Property, value);
        }

        public string M26
        {
            get => Position26.Text;
            set => SetValue(M26Property, value);
        }

        private static void MatrixChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Matrix26Control;

            if (control == null) return;

            var value = e.NewValue as Model.Calculations.NumericMatrix;

            if(value == null)
            {
                control.InitializeAllToZero();
                return;
            }

            control.M11 = value[0, 0].ToString("E2");
            control.M12 = value[0, 1].ToString("E2");
            control.M13 = value[0, 2].ToString("E2");
            control.M14 = value[0, 3].ToString("E2");
            control.M15 = value[0, 4].ToString("E2");
            control.M16 = value[0, 5].ToString("E2");

            control.M21 = value[1, 0].ToString("E2");
            control.M22 = value[1, 1].ToString("E2");
            control.M23 = value[1, 2].ToString("E2");
            control.M24 = value[1, 3].ToString("E2");
            control.M25 = value[1, 4].ToString("E2");
            control.M26 = value[1, 5].ToString("E2");
        }

        private void InitializeAllToZero()
        {
            M11 = "0";
            M12 = "0";
            M13 = "0";
            M14 = "0";
            M15 = "0";
            M16 = "0";

            M21 = "0";
            M22 = "0";
            M23 = "0";
            M24 = "0";
            M25 = "0";
            M26 = "0";
        }

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Matrix26Control;

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
                case nameof(M13):
                    control.Position13.Text = value;
                    break;
                case nameof(M14):
                    control.Position14.Text = value;
                    break;
                case nameof(M15):
                    control.Position15.Text = value;
                    break;
                case nameof(M16):
                    control.Position16.Text = value;
                    break;

                case nameof(M21):
                    control.Position21.Text = value;
                    break;
                case nameof(M22):
                    control.Position22.Text = value;
                    break;
                case nameof(M23):
                    control.Position23.Text = value;
                    break;
                case nameof(M24):
                    control.Position24.Text = value;
                    break;
                case nameof(M25):
                    control.Position25.Text = value;
                    break;
                case nameof(M26):
                    control.Position26.Text = value;
                    break;
            }
        }
    }
}