using POC3D.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace POC3D.Controls.Matrix.Controls
{
    /// <summary>
    ///     Interaction logic for CompactedGlobalStiffnessMatrixControl.xaml
    /// </summary>
    public partial class SolvedDisplacementsControl : UserControl
    {
        public static readonly DependencyProperty ProblemViewModelProperty = DependencyProperty.Register(nameof(ProblemViewModel),
            typeof(ProblemViewModel), typeof(SolvedDisplacementsControl),
            new PropertyMetadata(null, ProblemViewModelChangedCallback));

        public SolvedDisplacementsControl()
        {
            InitializeComponent();
        }

        public ProblemViewModel ProblemViewModel { get; set; }

        private void FillDisplacements(ProblemViewModel problemViewModel)
        {
            DisplacementsContainer.RowDefinitions.Clear();

            int indexes = 1;
            
            var freeNodes = problemViewModel.Nodes
                .ToDictionary(node => indexes++, node=> node)
                .Where(x => !x.Value.IsFixed)
                .ToList();

            foreach (var rowIndex in Enumerable.Range(0, freeNodes.Count * 3))
                DisplacementsContainer.RowDefinitions.Add(new RowDefinition
                {
                    Name = $"R{rowIndex}",
                    Height = GridLength.Auto
                });

            int gridRowIndex = 0;
            foreach (var nodeKeyPair in freeNodes)
            {
                var elementIndex = nodeKeyPair.Key;
                var node = nodeKeyPair.Value;

                BuildTextBlock(DisplacementsContainer, $"d{elementIndex}x", gridRowIndex, 0, Brushes.Green);
                BuildTextBlock(DisplacementsContainer, $"d{elementIndex}y", gridRowIndex + 1, 0, Brushes.Green);
                BuildTextBlock(DisplacementsContainer, $"d{elementIndex}z", gridRowIndex + 2, 0, Brushes.Green);

                gridRowIndex = gridRowIndex + 3;
            }
        }
        
        private void FillResults(ProblemViewModel problemViewModel)
        {
            ResultsContainer.RowDefinitions.Clear();

            var resultsMatrix = problemViewModel.SolvedDisplacementsVector;
            
            foreach (var rowIndex in Enumerable.Range(0, resultsMatrix.Rows))
                ResultsContainer.RowDefinitions.Add(new RowDefinition
                {
                    Name = $"R{rowIndex}",
                    Height = GridLength.Auto
                });
            
            foreach (var rowIndex in Enumerable.Range(0, resultsMatrix.Rows))
            {
                BuildTextBlock(ResultsContainer, $"{resultsMatrix[rowIndex, 0]}", rowIndex, 0, Brushes.Black);
            }
        }

        private static void ProblemViewModelChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SolvedDisplacementsControl;

            if (control == null) return;

            var problemViewModel = e.NewValue as ProblemViewModel;

            if (problemViewModel == null) return;

            control.FillDisplacements(problemViewModel);
            control.FillResults(problemViewModel);
        }

        private static void BuildTextBlock(Grid container, string text, int rowIndex, int columnIndex, Brush color)
        {
            var textBlock = new TextBlock
            {
                Text = text,
                Margin = new Thickness(10),
                Foreground = color
            };

            textBlock.SetValue(Grid.RowProperty, rowIndex);
            textBlock.SetValue(Grid.ColumnProperty, columnIndex);

            container.Children.Add(textBlock);
        }
    }
}