    using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace POC3D.Controls.Matrix.Controls
{
    /// <summary>
    ///     Interaction logic for GlobalStiffnessMatrixControl.xaml
    /// </summary>
    public partial class GlobalStiffnessMatrixControl : UserControl
    {
        public static readonly DependencyProperty MatrixProperty = DependencyProperty.Register(nameof(Matrix),
            typeof(Model.Calculations.NumericMatrix), typeof(GlobalStiffnessMatrixControl),
            new PropertyMetadata(null, MatrixChangedCallback));

        public GlobalStiffnessMatrixControl()
        {
            InitializeComponent();
        }

        public Model.Calculations.NumericMatrix Matrix { get; set; }

        private static void MatrixChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as GlobalStiffnessMatrixControl;

            if (control == null) return;

            var value = e.NewValue as Model.Calculations.NumericMatrix;

            if (value == null) return;

            control.Container.ColumnDefinitions.Clear();

            foreach (var columnIndex in Enumerable.Range(0, value.Columns))
                control.Container.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Name = $"C{columnIndex}",
                    Width = GridLength.Auto
                });

            foreach (var rowIndex in Enumerable.Range(0, value.Rows))
                control.Container.RowDefinitions.Add(new RowDefinition
                {
                    Name = $"R{rowIndex}",
                    Height = GridLength.Auto
                });

            foreach (var rowIndex in Enumerable.Range(0, value.Rows))
            foreach (var columnIndex in Enumerable.Range(0, value.Columns))
            {
                var textBlock = new TextBlock
                {
                    Text = $"{value[rowIndex, columnIndex]}"
                };

                textBlock.SetValue(Grid.RowProperty, rowIndex);
                textBlock.SetValue(Grid.ColumnProperty, columnIndex);
                textBlock.SetValue(MarginProperty, new Thickness(10));

                    control.Container.Children.Add(textBlock);
            }
        }
    }
}