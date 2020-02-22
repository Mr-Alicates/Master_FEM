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
    public partial class CompactedGlobalStiffnessMatrixControl : UserControl
    {
        public static readonly DependencyProperty ProblemViewModelProperty = DependencyProperty.Register(nameof(ProblemViewModel),
            typeof(ProblemViewModel), typeof(CompactedGlobalStiffnessMatrixControl),
            new PropertyMetadata(null, ProblemViewModelChangedCallback));

        public CompactedGlobalStiffnessMatrixControl()
        {
            InitializeComponent();
        }

        public ProblemViewModel ProblemViewModel
        {
            get => GetValue(ProblemViewModelProperty) as ProblemViewModel;
            set => SetValue(ProblemViewModelProperty, value);
        }

        private void FillGlobalStiffnessMatrix(ProblemViewModel problemViewModel)
        {
            GlobalStiffnessMatrixContainer.ColumnDefinitions.Clear();
            GlobalStiffnessMatrixContainer.RowDefinitions.Clear();
            GlobalStiffnessMatrixContainer.Children.Clear();

            var matrix = problemViewModel.CompactedMatrix;

            //Create rows and columns in the grid
            foreach (var columnIndex in Enumerable.Range(0, matrix.Columns))
                GlobalStiffnessMatrixContainer.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Name = $"C{columnIndex}",
                    Width = GridLength.Auto
                });

            foreach (var rowIndex in Enumerable.Range(0, matrix.Rows))
                GlobalStiffnessMatrixContainer.RowDefinitions.Add(new RowDefinition
                {
                    Name = $"R{rowIndex}",
                    Height = GridLength.Auto
                });


            foreach (var rowIndex in Enumerable.Range(0, matrix.Rows))
            {
                foreach (var columnIndex in Enumerable.Range(0, matrix.Columns))
                {
                    BuildTextBlock(GlobalStiffnessMatrixContainer, $"{matrix[rowIndex, columnIndex]}", rowIndex, columnIndex, Brushes.Black);
                }
            }
        }

        private void FillDisplacements(ProblemViewModel problemViewModel)
        {
            DisplacementsContainer.Children.Clear();
            DisplacementsContainer.ColumnDefinitions.Clear();
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

        private void FillForces(ProblemViewModel problemViewModel)
        {
            ForcesContainer.Children.Clear();
            ForcesContainer.ColumnDefinitions.Clear();
            ForcesContainer.RowDefinitions.Clear();

            var forces = problemViewModel.CompactedForcesVector;

            foreach (var rowIndex in Enumerable.Range(0, forces.Rows))
                ForcesContainer.RowDefinitions.Add(new RowDefinition
                {
                    Name = $"R{rowIndex}",
                    Height = GridLength.Auto
                });
                                   
            foreach (var rowIndex in Enumerable.Range(0, forces.Rows))
            {
                BuildTextBlock(ForcesContainer, $"{forces[rowIndex, 0]}", rowIndex, 0, Brushes.Black);
            }
        }

        private static void ProblemViewModelChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CompactedGlobalStiffnessMatrixControl;

            if (control == null) return;

            var problemViewModel = e.NewValue as ProblemViewModel;

            if (problemViewModel == null) return;

            control.ProblemViewModelChangedCallback(problemViewModel);
        }

        private void ProblemViewModelChangedCallback(ProblemViewModel problemViewModel)
        {
            problemViewModel.PropertyChanged += ProblemViewModel_PropertyChanged;

            FillGlobalStiffnessMatrix(ProblemViewModel);
            FillDisplacements(ProblemViewModel);
            FillForces(ProblemViewModel);
        }

        private void ProblemViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            FillGlobalStiffnessMatrix(ProblemViewModel);
            FillDisplacements(ProblemViewModel);
            FillForces(ProblemViewModel);
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