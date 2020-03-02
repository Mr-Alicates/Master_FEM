﻿using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using POC3D.ViewModel;

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

            var freeNodes = problemViewModel.Nodes
                .ToDictionary(node => indexes++, node => node)
                .Where(x => !x.Value.IsFixed)
                .ToList();

            foreach (var rowIndex in Enumerable.Range(0, freeNodes.Count * 3))
                DisplacementsContainer.RowDefinitions.Add(new RowDefinition
                {
                    Name = $"R{rowIndex}",
                    Height = GridLength.Auto
                });

            var gridRowIndex = 0;
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
            ResultsContainer.Children.Clear();
            ResultsContainer.RowDefinitions.Clear();

            var resultsMatrix = problemViewModel.SolvedDisplacementsVector;

            foreach (var rowIndex in Enumerable.Range(0, resultsMatrix.Rows))
                ResultsContainer.RowDefinitions.Add(new RowDefinition
                {
                    Name = $"R{rowIndex}",
                    Height = GridLength.Auto
                });

            foreach (var rowIndex in Enumerable.Range(0, resultsMatrix.Rows))
                BuildTextBlock(ResultsContainer, $"{resultsMatrix[rowIndex, 0]}", rowIndex, 0, Brushes.Black);
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

            textBlock.SetValue(Grid.RowProperty, rowIndex);
            textBlock.SetValue(Grid.ColumnProperty, columnIndex);

            container.Children.Add(textBlock);
        }
    }
}