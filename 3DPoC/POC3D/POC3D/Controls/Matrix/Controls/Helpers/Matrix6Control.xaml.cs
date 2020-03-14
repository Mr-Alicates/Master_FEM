using System.Windows;
using System.Windows.Controls;
using POC3D.ViewModel.Calculations;

namespace POC3D.Controls.Matrix.Controls.Helpers
{
    /// <summary>
    ///     Interaction logic for Matrix6Control.xaml
    /// </summary>
    public partial class Matrix6Control : UserControl
    {
        public static readonly DependencyProperty M11Property = DependencyProperty.Register(nameof(M11), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M12Property = DependencyProperty.Register(nameof(M12), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M13Property = DependencyProperty.Register(nameof(M13), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M14Property = DependencyProperty.Register(nameof(M14), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M15Property = DependencyProperty.Register(nameof(M15), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M16Property = DependencyProperty.Register(nameof(M16), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M21Property = DependencyProperty.Register(nameof(M21), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M22Property = DependencyProperty.Register(nameof(M22), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M23Property = DependencyProperty.Register(nameof(M23), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M24Property = DependencyProperty.Register(nameof(M24), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M25Property = DependencyProperty.Register(nameof(M25), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M26Property = DependencyProperty.Register(nameof(M26), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M31Property = DependencyProperty.Register(nameof(M31), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M32Property = DependencyProperty.Register(nameof(M32), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M33Property = DependencyProperty.Register(nameof(M33), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M34Property = DependencyProperty.Register(nameof(M34), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M35Property = DependencyProperty.Register(nameof(M35), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M36Property = DependencyProperty.Register(nameof(M36), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M41Property = DependencyProperty.Register(nameof(M41), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M42Property = DependencyProperty.Register(nameof(M42), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M43Property = DependencyProperty.Register(nameof(M43), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M44Property = DependencyProperty.Register(nameof(M44), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M45Property = DependencyProperty.Register(nameof(M45), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M46Property = DependencyProperty.Register(nameof(M46), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M51Property = DependencyProperty.Register(nameof(M51), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M52Property = DependencyProperty.Register(nameof(M52), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M53Property = DependencyProperty.Register(nameof(M53), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M54Property = DependencyProperty.Register(nameof(M54), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M55Property = DependencyProperty.Register(nameof(M55), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M56Property = DependencyProperty.Register(nameof(M56), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M61Property = DependencyProperty.Register(nameof(M61), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M62Property = DependencyProperty.Register(nameof(M62), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M63Property = DependencyProperty.Register(nameof(M63), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M64Property = DependencyProperty.Register(nameof(M64), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M65Property = DependencyProperty.Register(nameof(M65), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M66Property = DependencyProperty.Register(nameof(M66), typeof(string),
            typeof(Matrix6Control), new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty MatrixProperty = DependencyProperty.Register(nameof(Matrix),
            typeof(NumericMatrix), typeof(Matrix6Control),
            new PropertyMetadata(null, MatrixChangedCallback));

        public Matrix6Control()
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

        public string M33
        {
            get => Position33.Text;
            set => SetValue(M33Property, value);
        }

        public string M34
        {
            get => Position34.Text;
            set => SetValue(M34Property, value);
        }

        public string M35
        {
            get => Position35.Text;
            set => SetValue(M35Property, value);
        }

        public string M36
        {
            get => Position36.Text;
            set => SetValue(M36Property, value);
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

        public string M43
        {
            get => Position43.Text;
            set => SetValue(M43Property, value);
        }

        public string M44
        {
            get => Position44.Text;
            set => SetValue(M44Property, value);
        }

        public string M45
        {
            get => Position45.Text;
            set => SetValue(M45Property, value);
        }

        public string M46
        {
            get => Position46.Text;
            set => SetValue(M46Property, value);
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

        public string M53
        {
            get => Position53.Text;
            set => SetValue(M53Property, value);
        }

        public string M54
        {
            get => Position54.Text;
            set => SetValue(M54Property, value);
        }

        public string M55
        {
            get => Position55.Text;
            set => SetValue(M55Property, value);
        }

        public string M56
        {
            get => Position56.Text;
            set => SetValue(M56Property, value);
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

        public string M63
        {
            get => Position63.Text;
            set => SetValue(M63Property, value);
        }

        public string M64
        {
            get => Position64.Text;
            set => SetValue(M64Property, value);
        }

        public string M65
        {
            get => Position65.Text;
            set => SetValue(M65Property, value);
        }

        public string M66
        {
            get => Position66.Text;
            set => SetValue(M66Property, value);
        }

        private static void MatrixChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Matrix6Control;

            if (control == null) return;

            var value = e.NewValue as NumericMatrix;

            if (value == null)
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

            control.M31 = value[2, 0].ToString("E2");
            control.M32 = value[2, 1].ToString("E2");
            control.M33 = value[2, 2].ToString("E2");
            control.M34 = value[2, 3].ToString("E2");
            control.M35 = value[2, 4].ToString("E2");
            control.M36 = value[2, 5].ToString("E2");

            control.M41 = value[3, 0].ToString("E2");
            control.M42 = value[3, 1].ToString("E2");
            control.M43 = value[3, 2].ToString("E2");
            control.M44 = value[3, 3].ToString("E2");
            control.M45 = value[3, 4].ToString("E2");
            control.M46 = value[3, 5].ToString("E2");

            control.M51 = value[4, 0].ToString("E2");
            control.M52 = value[4, 1].ToString("E2");
            control.M53 = value[4, 2].ToString("E2");
            control.M54 = value[4, 3].ToString("E2");
            control.M55 = value[4, 4].ToString("E2");
            control.M56 = value[4, 5].ToString("E2");

            control.M61 = value[5, 0].ToString("E2");
            control.M62 = value[5, 1].ToString("E2");
            control.M63 = value[5, 2].ToString("E2");
            control.M64 = value[5, 3].ToString("E2");
            control.M65 = value[5, 4].ToString("E2");
            control.M66 = value[5, 5].ToString("E2");
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

            M31 = "0";
            M32 = "0";
            M33 = "0";
            M34 = "0";
            M35 = "0";
            M36 = "0";

            M41 = "0";
            M42 = "0";
            M43 = "0";
            M44 = "0";
            M45 = "0";
            M46 = "0";

            M51 = "0";
            M52 = "0";
            M53 = "0";
            M54 = "0";
            M55 = "0";
            M56 = "0";

            M61 = "0";
            M62 = "0";
            M63 = "0";
            M64 = "0";
            M65 = "0";
            M66 = "0";
        }

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Matrix6Control;

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

                case nameof(M31):
                    control.Position31.Text = value;
                    break;
                case nameof(M32):
                    control.Position32.Text = value;
                    break;
                case nameof(M33):
                    control.Position33.Text = value;
                    break;
                case nameof(M34):
                    control.Position34.Text = value;
                    break;
                case nameof(M35):
                    control.Position35.Text = value;
                    break;
                case nameof(M36):
                    control.Position36.Text = value;
                    break;

                case nameof(M41):
                    control.Position41.Text = value;
                    break;
                case nameof(M42):
                    control.Position42.Text = value;
                    break;
                case nameof(M43):
                    control.Position43.Text = value;
                    break;
                case nameof(M44):
                    control.Position44.Text = value;
                    break;
                case nameof(M45):
                    control.Position45.Text = value;
                    break;
                case nameof(M46):
                    control.Position46.Text = value;
                    break;

                case nameof(M51):
                    control.Position51.Text = value;
                    break;
                case nameof(M52):
                    control.Position52.Text = value;
                    break;
                case nameof(M53):
                    control.Position53.Text = value;
                    break;
                case nameof(M54):
                    control.Position54.Text = value;
                    break;
                case nameof(M55):
                    control.Position55.Text = value;
                    break;
                case nameof(M56):
                    control.Position56.Text = value;
                    break;

                case nameof(M61):
                    control.Position61.Text = value;
                    break;
                case nameof(M62):
                    control.Position62.Text = value;
                    break;
                case nameof(M63):
                    control.Position63.Text = value;
                    break;
                case nameof(M64):
                    control.Position64.Text = value;
                    break;
                case nameof(M65):
                    control.Position65.Text = value;
                    break;
                case nameof(M66):
                    control.Position66.Text = value;
                    break;
            }
        }
    }
}