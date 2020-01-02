using System.Windows;
using System.Windows.Controls;

namespace POC3D.Controls.Matrix.Controls.Helpers
{
    /// <summary>
    ///     Interaction logic for Matrix2Control.xaml
    /// </summary>
    public partial class Matrix2Control : UserControl
    {
        public static readonly DependencyProperty M11Property = DependencyProperty.Register(
            nameof(M11), typeof(string), typeof(Matrix2Control),
            new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M12Property = DependencyProperty.Register(
            nameof(M12), typeof(string), typeof(Matrix2Control),
            new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M21Property = DependencyProperty.Register(
            nameof(M21), typeof(string), typeof(Matrix2Control),
            new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public static readonly DependencyProperty M22Property = DependencyProperty.Register(
            nameof(M22), typeof(string), typeof(Matrix2Control),
            new PropertyMetadata(string.Empty, PropertyChangedCallback));

        public Matrix2Control()
        {
            InitializeComponent();
        }

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

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Matrix2Control;

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
            }
        }
    }
}