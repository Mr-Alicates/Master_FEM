using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using POC3D.ViewModel;

namespace POC3D.Controls.Matrix.Controls
{
    /// <summary>
    ///     Interaction logic for GlobalStiffnessMatrixControl.xaml
    /// </summary>
    public partial class CorrespondenceMatrixControl : UserControl
    {
        public static readonly DependencyProperty ProblemViewModelProperty = DependencyProperty.Register(
            nameof(ProblemViewModel),
            typeof(ProblemViewModel), typeof(CorrespondenceMatrixControl),
            new PropertyMetadata(null, ProblemViewModelChangedCallback));

        public CorrespondenceMatrixControl()
        {
            InitializeComponent();
        }

        public ProblemViewModel ProblemViewModel
        {
            get => GetValue(ProblemViewModelProperty) as ProblemViewModel;
            set => SetValue(ProblemViewModelProperty, value);
        }

        private void FillCorrespondenceMatrix(ProblemViewModel problemViewModel)
        {
            CorrespondenceMatrixContainer.ColumnDefinitions.Clear();
            CorrespondenceMatrixContainer.RowDefinitions.Clear();
            CorrespondenceMatrixContainer.Children.Clear();

            var correspondenceMatrix = problemViewModel.CorrespondenceMatrix;

            //Create rows and columns in the grid
            foreach (var columnIndex in Enumerable.Range(0, correspondenceMatrix.Columns + 1))
                CorrespondenceMatrixContainer.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Name = $"C{columnIndex}",
                    Width = GridLength.Auto
                });

            foreach (var rowIndex in Enumerable.Range(0, correspondenceMatrix.Rows + 1))
                CorrespondenceMatrixContainer.RowDefinitions.Add(new RowDefinition
                {
                    Name = $"R{rowIndex}",
                    Height = GridLength.Auto
                });

            //Create header row and column
            
            var index = 1;
            foreach (var node in problemViewModel.Nodes)
            {
                var color = node.IsFixed ? Brushes.Red : Brushes.Green;

                BuildTextBlock(CorrespondenceMatrixContainer, $"Node {node.Id}", 0, index, color);
                BuildTextBlock(CorrespondenceMatrixContainer, $"Node {node.Id}", index, 0, color);

                index++;
            }

            foreach (var rowIndex in Enumerable.Range(0, correspondenceMatrix.Rows))
            {
                foreach (var columnIndex in Enumerable.Range(0, correspondenceMatrix.Columns))
                {
                    var entry = string.Join("\r\n", correspondenceMatrix.Matrix[(rowIndex, columnIndex)].Select(x => $"Element {x.Description}"));

                    BuildTextBlock(CorrespondenceMatrixContainer, entry, rowIndex + 1, columnIndex + 1, Brushes.Black);
                }
            }
        }

        private static void ProblemViewModelChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CorrespondenceMatrixControl;

            if (control == null) return;

            var problemViewModel = e.NewValue as ProblemViewModel;

            if (problemViewModel == null) return;

            control.ProblemViewModelChangedCallback(problemViewModel);
        }

        private void ProblemViewModelChangedCallback(ProblemViewModel problemViewModel)
        {
            problemViewModel.PropertyChanged += ProblemViewModel_PropertyChanged;

            FillCorrespondenceMatrix(ProblemViewModel);
        }

        private void ProblemViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            FillCorrespondenceMatrix(ProblemViewModel);
        }

        private static void BuildTextBlock(Grid container, string text, int rowIndex, int columnIndex, Brush color)
        {
            var textBlock = new TextBlock
            {
                Text = text,
                Margin = new Thickness(10),
                Foreground = color
            };
            
            Border border = new Border();
            border.SetValue(Border.BorderThicknessProperty, new Thickness(2));
            border.SetValue(Border.BorderBrushProperty, Brushes.Black);
            border.SetValue(Grid.RowProperty, rowIndex);
            border.SetValue(Grid.ColumnProperty, columnIndex);
            border.Child = textBlock;

            container.Children.Add(border);
        }
    }
}