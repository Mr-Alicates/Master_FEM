using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using POC3D.ViewModel.Implementation;

namespace POC3D.Controls.Matrix.Controls
{
    /// <summary>
    ///     Interaction logic for CompactedGlobalStiffnessMatrixControl.xaml
    /// </summary>
    public partial class SolvedDisplacementsControl : UserControl
    {
        public static readonly DependencyProperty ProblemViewModelProperty = DependencyProperty.Register(
            nameof(ProblemViewModel),
            typeof(ProblemViewModel), typeof(SolvedDisplacementsControl),
            new PropertyMetadata(null, ProblemViewModelChangedCallback));

        public SolvedDisplacementsControl()
        {
            InitializeComponent();
        }

        public ProblemViewModel ProblemViewModel
        {
            get => GetValue(ProblemViewModelProperty) as ProblemViewModel;
            set => SetValue(ProblemViewModelProperty, value);
        }

        private void FillDisplacements(ProblemViewModel problemViewModel)
        {
            DisplacementsContainer.Children.Clear();
            DisplacementsContainer.RowDefinitions.Clear();

            var indexes = 1;

            var nodes = problemViewModel.Nodes
                .ToDictionary(node => indexes++, node => node)
                .ToList();

            foreach (var rowIndex in Enumerable.Range(0, nodes.Count * 3))
                DisplacementsContainer.RowDefinitions.Add(new RowDefinition
                {
                    Name = $"R{rowIndex}",
                    Height = GridLength.Auto
                });

            var gridRowIndex = 0;
            foreach (var nodeKeyPair in nodes)
            {
                var elementIndex = nodeKeyPair.Key;
                var node = nodeKeyPair.Value;

                var colorX = node.IsXFixed ? Brushes.Red : Brushes.Green;
                var colorY = node.IsYFixed ? Brushes.Red : Brushes.Green;
                var colorZ = node.IsZFixed ? Brushes.Red : Brushes.Green;

                BuildTextBlock(DisplacementsContainer, $"d{elementIndex}x", gridRowIndex, 0, colorX);
                gridRowIndex++;

                BuildTextBlock(DisplacementsContainer, $"d{elementIndex}y", gridRowIndex, 0, colorY);
                gridRowIndex++;

                BuildTextBlock(DisplacementsContainer, $"d{elementIndex}z", gridRowIndex, 0, colorZ);
                gridRowIndex++;
            }
        }

        private void FillResults(ProblemViewModel problemViewModel)
        {
            ResultsContainer.Children.Clear();
            ResultsContainer.RowDefinitions.Clear();

            foreach (var index in Enumerable.Range(0, problemViewModel.Nodes.Count * 3))
                ResultsContainer.RowDefinitions.Add(new RowDefinition
                {
                    Name = $"R{index}",
                    Height = GridLength.Auto
                });

            var resultsMatrix = problemViewModel.ProblemCalculationViewModel.SolvedDisplacementsVector;

            var rowIndex = 0;
            var gridRowIndex = 0;
            foreach (var node in problemViewModel.Nodes)
            {
                if (node.IsXFixed)
                {
                    BuildTextBlock(ResultsContainer, $"{0}", gridRowIndex, 0, Brushes.Black);
                }
                else
                {
                    BuildTextBlock(ResultsContainer, $"{resultsMatrix[rowIndex, 0]}", gridRowIndex, 0, Brushes.Black);
                    rowIndex++;
                }

                gridRowIndex++;

                if (node.IsYFixed)
                {
                    BuildTextBlock(ResultsContainer, $"{0}", gridRowIndex, 0, Brushes.Black);
                }
                else
                {
                    BuildTextBlock(ResultsContainer, $"{resultsMatrix[rowIndex, 0]}", gridRowIndex, 0, Brushes.Black);
                    rowIndex++;
                }

                gridRowIndex++;

                if (node.IsZFixed)
                {
                    BuildTextBlock(ResultsContainer, $"{0}", gridRowIndex, 0, Brushes.Black);
                }
                else
                {
                    BuildTextBlock(ResultsContainer, $"{resultsMatrix[rowIndex, 0]}", gridRowIndex, 0, Brushes.Black);
                    rowIndex++;
                }

                gridRowIndex++;
            }
        }

        private static void ProblemViewModelChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SolvedDisplacementsControl;

            if (control == null) return;

            var problemViewModel = e.NewValue as ProblemViewModel;

            if (problemViewModel == null) return;

            control.ProblemViewModelChangedCallback(problemViewModel);
        }

        private void ProblemViewModelChangedCallback(ProblemViewModel problemViewModel)
        {
            problemViewModel.PropertyChanged += ProblemViewModel_PropertyChanged;

            FillDisplacements(problemViewModel);
            FillResults(problemViewModel);
        }

        private void ProblemViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            FillDisplacements(ProblemViewModel);
            FillResults(ProblemViewModel);
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