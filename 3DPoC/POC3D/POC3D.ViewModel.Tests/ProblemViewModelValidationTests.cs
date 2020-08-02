using NUnit.Framework;
using POC3D.ViewModel.Calculations;
using POC3D.ViewModel.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace POC3D.ViewModel.Tests
{
    [TestFixture]
    public class ProblemViewModelValidationTests
    {
        [Test]
        public void AnsysInitialValidationTests()
        {
            //Arrange
            const double MaximumAllowedDelta = 2.5E-9;

            var projectViewModel = SetupScenario();

            var expectedResults = BuildExpectedResultsFromAnsys();

            //Act
            var displacementResults = projectViewModel.ProblemCalculationViewModel.FullSolvedDisplacementsVector;

            //Assert
            foreach(var index in Enumerable.Range(0, displacementResults.Rows))
            {
                var calculated = displacementResults[index, 0];
                var expected = expectedResults[index];

                var displacementDelta = Math.Abs(calculated - expected);

                var isBelowThreshold = displacementDelta < MaximumAllowedDelta;

                Assert.That(isBelowThreshold, Is.True, $"Delta is {displacementDelta}, greater than {MaximumAllowedDelta}");
            }
        }

        [Test]
        public void NastranInitialValidationTests()
        {
            //Arrange
            const double MaximumAllowedDelta = 6E-9;
            
            var projectViewModel = SetupScenario();

            var expectedResults = BuildExpectedResultsFromNastran();

            //Act
            var displacementResults = projectViewModel.ProblemCalculationViewModel.FullSolvedDisplacementsVector;

            var maximum = -1.0;

            //Assert
            foreach (var index in Enumerable.Range(0, displacementResults.Rows))
            {
                var calculated = displacementResults[index, 0];
                var expected = expectedResults[index] * 1E-3; //Nastran results are in MILIMETERS

                var displacementDelta = Math.Abs(calculated - expected);

                var isBelowThreshold = displacementDelta < MaximumAllowedDelta;

                Assert.That(isBelowThreshold, Is.True, $"Delta is {displacementDelta}, greater than {MaximumAllowedDelta}");

                maximum = Math.Max(maximum, displacementDelta);
            }
        }

        private ProblemViewModel SetupScenario()
        {
            var problemViewModel = new ProblemViewModel();

            #region nodeSetup

            var node1 = problemViewModel.AddNode();
            var node2 = problemViewModel.AddNode();
            var node3 = problemViewModel.AddNode();
            var node4 = problemViewModel.AddNode();
            var node5 = problemViewModel.AddNode();
            var node6 = problemViewModel.AddNode();
            var node7 = problemViewModel.AddNode();
            var node8 = problemViewModel.AddNode();

            node1.X = 0;
            node1.Y = 1;
            node1.Z = 0;
            node1.IsXFixed = false;
            node1.IsYFixed = false;
            node1.IsZFixed = true;

            node2.X = 0;
            node2.Y = 0;
            node2.Z = 0;
            node2.IsXFixed = true;
            node2.IsYFixed = true;
            node2.IsZFixed = true;

            node3.X = 1;
            node3.Y = 1;
            node3.Z = 0;
            node3.IsXFixed = false;
            node3.IsYFixed = false;
            node3.IsZFixed = false;

            node4.X = 1;
            node4.Y = 0;
            node4.Z = 0;
            node4.IsXFixed = false;
            node4.IsYFixed = true;
            node4.IsZFixed = true;

            node5.X = 0;
            node5.Y = 0;
            node5.Z = 1;
            node5.IsXFixed = false;
            node5.IsYFixed = false;
            node5.IsZFixed = false;

            node6.X = 1;
            node6.Y = 0;
            node6.Z = 1;
            node6.IsXFixed = false;
            node6.IsYFixed = false;
            node6.IsZFixed = false;

            node7.X = 1;
            node7.Y = 1;
            node7.Z = 1;
            node7.IsXFixed = false;
            node7.IsYFixed = false;
            node7.IsZFixed = false;

            node8.X = 0;
            node8.Y = 1;
            node8.Z = 1;
            node8.IsXFixed = false;
            node8.IsYFixed = false;
            node8.IsZFixed = false;

            #endregion

            #region forceSetup

            var force = problemViewModel.AddForce(node7);
            force.ApplicationVectorX = 1;

            #endregion

            #region elementSetup

            var element1 = problemViewModel.AddBarElement(node1, node2);
            var element2 = problemViewModel.AddBarElement(node3, node1);
            var element3 = problemViewModel.AddBarElement(node4, node3);
            var element4 = problemViewModel.AddBarElement(node2, node4);
            var element5 = problemViewModel.AddBarElement(node5, node6);
            var element6 = problemViewModel.AddBarElement(node6, node7);
            var element7 = problemViewModel.AddBarElement(node7, node8);
            var element8 = problemViewModel.AddBarElement(node8, node5);
            var element9 = problemViewModel.AddBarElement(node5, node2);
            var element10 = problemViewModel.AddBarElement(node4, node6);
            var element11 = problemViewModel.AddBarElement(node3, node7);
            var element12 = problemViewModel.AddBarElement(node8, node1);
            var element13 = problemViewModel.AddBarElement(node5, node7);
            var element14 = problemViewModel.AddBarElement(node7, node4);
            var element15 = problemViewModel.AddBarElement(node4, node1);
            var element16 = problemViewModel.AddBarElement(node1, node5);
            var element17 = problemViewModel.AddBarElement(node7, node1);
            var element18 = problemViewModel.AddBarElement(node4, node5);

            element1.CrossSectionArea = 3.14E-06;
            element2.CrossSectionArea = 3.14E-06;
            element3.CrossSectionArea = 3.14E-06;
            element4.CrossSectionArea = 3.14E-06;
            element5.CrossSectionArea = 3.14E-06;
            element6.CrossSectionArea = 3.14E-06;
            element7.CrossSectionArea = 3.14E-06;
            element8.CrossSectionArea = 3.14E-06;
            element9.CrossSectionArea = 3.14E-06;
            element10.CrossSectionArea = 3.14E-06;
            element11.CrossSectionArea = 3.14E-06;
            element12.CrossSectionArea = 3.14E-06;
            element13.CrossSectionArea = 3.14E-06;
            element14.CrossSectionArea = 3.14E-06;
            element15.CrossSectionArea = 3.14E-06;
            element16.CrossSectionArea = 3.14E-06;
            element17.CrossSectionArea = 3.14E-06;
            element18.CrossSectionArea = 3.14E-06;

            #endregion

            #region materialSetup

            var material = problemViewModel.Materials.Single();
            material.YoungsModulus = 2.1E11;

            #endregion

            return problemViewModel;
        }

        private double[] BuildExpectedResultsFromAnsys()
        {
            //Ansys results are in METERS
            return new[]
            {
                0.51788E-005,
                0.15169E-005,
                0.0000,
                0.0000,
                0.0000,
                0.0000,
                0.51788E-005,
                -0.39268E-010,
                -0.36620E-005,
                0.15169E-005,
                0.0000,
                0.0000,
                0.51788E-005,
                0.51788E-005,
                0.15169E-005,
                0.51788E-005,
                0.15168E-005,
                -0.39268E-010,
                0.10986E-004,
                0.15168E-005,
                -0.36620E-005,
                0.10986E-004,
                0.51788E-005, 
                -0.22379E-011
            };
        }

        private double[] BuildExpectedResultsFromNastran()
        {
            //Ansys results are in MILIMETERS
            return new[]
            {
                5.175E-3,
                1.516E-3,
                -7.623E-9,
                000.000E+0,
                000.000E+0,
                000.000E+0,
                5.175E-3,
                -7.623E-9,
                -3.659E-3,
                1.516E-3,
                000.000E+0,
                000.000E+0,
                5.175E-3,
                5.175E-3,
                1.516E-3,
                5.175E-3,
                1.516E-3,
                000.000E+0,
                10.978E-3,
                1.516E-3,
                -3.659E-3,
                10.978E-3,
                5.175E-3,
                -649.666E-12
            };
        }
    }
}
