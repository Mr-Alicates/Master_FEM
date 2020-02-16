﻿using POC3D.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace POC3D.Controls.Matrix.Controls
{
    /// <summary>
    ///     Interaction logic for GlobalStiffnessMatrixControl.xaml
    /// </summary>
    public partial class GlobalStiffnessMatrixControl : UserControl
    {
        public static readonly DependencyProperty ProblemViewModelProperty = DependencyProperty.Register(nameof(ProblemViewModel),
            typeof(ProblemViewModel), typeof(GlobalStiffnessMatrixControl),
            new PropertyMetadata(null, ProblemViewModelChangedCallback));

        public GlobalStiffnessMatrixControl()
        {
            InitializeComponent();
        }

        public ProblemViewModel ProblemViewModel { get; set; }

        private void FillGlobalStiffnessMatrix(ProblemViewModel problemViewModel)
        {
            GlobalStiffnessMatrixContainer.ColumnDefinitions.Clear();
            GlobalStiffnessMatrixContainer.RowDefinitions.Clear();
            GlobalStiffnessMatrixContainer.Children.Clear();

            var globalStiffnessMatrix = problemViewModel.GlobalStiffnessMatrix;

            //Create rows and columns in the grid
            foreach (var columnIndex in Enumerable.Range(0, globalStiffnessMatrix.Columns + 1))
                GlobalStiffnessMatrixContainer.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Name = $"C{columnIndex}",
                    Width = GridLength.Auto
                });

            foreach (var rowIndex in Enumerable.Range(0, globalStiffnessMatrix.Rows + 1))
                GlobalStiffnessMatrixContainer.RowDefinitions.Add(new RowDefinition
                {
                    Name = $"R{rowIndex}",
                    Height = GridLength.Auto
                });

            //Create header row and column
            int elementIndex = 1;
            foreach (var node in problemViewModel.Nodes)
            {
                var color = node.IsFixed ? Brushes.Red : Brushes.Green;

                BuildTextBlock(GlobalStiffnessMatrixContainer, $"d{elementIndex}x", 0, elementIndex * 3 - 2, color);
                BuildTextBlock(GlobalStiffnessMatrixContainer, $"d{elementIndex}y", 0, elementIndex * 3 - 1, color);
                BuildTextBlock(GlobalStiffnessMatrixContainer, $"d{elementIndex}z", 0, elementIndex * 3, color);

                BuildTextBlock(GlobalStiffnessMatrixContainer, $"d{elementIndex}x", elementIndex * 3 - 2, 0, color);
                BuildTextBlock(GlobalStiffnessMatrixContainer, $"d{elementIndex}y", elementIndex * 3 - 1, 0, color);
                BuildTextBlock(GlobalStiffnessMatrixContainer, $"d{elementIndex}z", elementIndex * 3, 0, color);

                elementIndex++;
            }

            foreach (var rowIndex in Enumerable.Range(0, globalStiffnessMatrix.Rows))
                foreach (var columnIndex in Enumerable.Range(0, globalStiffnessMatrix.Columns))
                {
                    BuildTextBlock(GlobalStiffnessMatrixContainer, $"{globalStiffnessMatrix[rowIndex, columnIndex]}", rowIndex + 1, columnIndex + 1, Brushes.Black);
                }
        }

        private void FillDisplacements(ProblemViewModel problemViewModel)
        {
            DisplacementsContainer.ColumnDefinitions.Clear();
            DisplacementsContainer.RowDefinitions.Clear();


            foreach (var rowIndex in Enumerable.Range(0, problemViewModel.NumberOfNodes * 3 + 1))
                DisplacementsContainer.RowDefinitions.Add(new RowDefinition
                {
                    Name = $"R{rowIndex}",
                    Height = GridLength.Auto
                });

            // This is filler to align the items from the global stiffness matrix to the displacement vector
            BuildTextBlock(DisplacementsContainer, "", 0, 0, Brushes.Black);

            int elementIndex = 1;
            foreach (var node in problemViewModel.Nodes)
            {
                if (node.IsFixed)
                {
                    BuildTextBlock(DisplacementsContainer, $"0", elementIndex * 3 - 2, 0, Brushes.Red);
                    BuildTextBlock(DisplacementsContainer, $"0", elementIndex * 3 - 1, 0, Brushes.Red);
                    BuildTextBlock(DisplacementsContainer, $"0", elementIndex * 3, 0, Brushes.Red);
                }
                else
                {
                    BuildTextBlock(DisplacementsContainer, $"d{elementIndex}x", elementIndex * 3 - 2, 0, Brushes.Green);
                    BuildTextBlock(DisplacementsContainer, $"d{elementIndex}y", elementIndex * 3 - 1, 0, Brushes.Green);
                    BuildTextBlock(DisplacementsContainer, $"d{elementIndex}z", elementIndex * 3, 0, Brushes.Green);
                }

                elementIndex++;
            }
        }

        private void FillForces(ProblemViewModel problemViewModel)
        {
            ForcesContainer.ColumnDefinitions.Clear();
            ForcesContainer.RowDefinitions.Clear();


            foreach (var rowIndex in Enumerable.Range(0, problemViewModel.NumberOfNodes * 3 + 1))
                ForcesContainer.RowDefinitions.Add(new RowDefinition
                {
                    Name = $"R{rowIndex}",
                    Height = GridLength.Auto
                });

            // This is filler to align the items from the global stiffness matrix to the displacement vector
            BuildTextBlock(ForcesContainer, "", 0, 0, Brushes.Black);

            int elementIndex = 1;
            foreach (var node in problemViewModel.Nodes)
            {
                if (node.IsFixed)
                {
                    BuildTextBlock(ForcesContainer, $"Reaction F{elementIndex}x", elementIndex * 3 - 2, 0, Brushes.Red);
                    BuildTextBlock(ForcesContainer, $"Reaction F{elementIndex}y", elementIndex * 3 - 1, 0, Brushes.Red);
                    BuildTextBlock(ForcesContainer, $"Reaction F{elementIndex}z", elementIndex * 3, 0, Brushes.Red);
                }
                else
                {
                    var appliedForce = problemViewModel.Forces.FirstOrDefault(force => force.Node == node);
                    
                    BuildTextBlock(ForcesContainer, appliedForce?.ApplicationVector.X.ToString() ?? "0", elementIndex * 3 - 2, 0, Brushes.Green);
                    BuildTextBlock(ForcesContainer, appliedForce?.ApplicationVector.Y.ToString() ?? "0", elementIndex * 3 - 1, 0, Brushes.Green);
                    BuildTextBlock(ForcesContainer, appliedForce?.ApplicationVector.Z.ToString() ?? "0", elementIndex * 3, 0, Brushes.Green);
                }

                elementIndex++;
            }
        }


        private static void ProblemViewModelChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as GlobalStiffnessMatrixControl;

            if (control == null) return;

            var problemViewModel = e.NewValue as ProblemViewModel;

            if (problemViewModel == null) return;

            control.FillGlobalStiffnessMatrix(problemViewModel);
            control.FillDisplacements(problemViewModel);
            control.FillForces(problemViewModel);
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