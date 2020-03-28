using NUnit.Framework;
using POC3D.ViewModel.Implementation;
using POC3D.ViewModel.Tests.Helper;

namespace POC3D.ViewModel.Tests
{
    [TestFixture]
    public class ProblemViewModelTests
    {
        [Test]
        public void Ctor_ExpectPropertiesCorrectlyInitialized()
        {
            //Arrange
            //Act
            var problemViewModel = new ProblemViewModel();

            //Assert
            Assert.That(problemViewModel.SelectedNode, Is.Null);
            Assert.That(problemViewModel.SelectedElement, Is.Null);
            Assert.That(problemViewModel.SelectedForce, Is.Null);
            Assert.That(problemViewModel.Nodes, Is.Not.Null);
            Assert.That(problemViewModel.Elements, Is.Not.Null);
            Assert.That(problemViewModel.Forces, Is.Not.Null);
            Assert.That(problemViewModel.Materials, Is.Not.Null);
            Assert.That(problemViewModel.ProblemCalculationViewModel, Is.Not.Null);
        }

        [Test]
        public void SelectedNode_SetNode_ExpectNodeIsSelectedIsTrue()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var nodeViewModel = problemViewModel.AddNode();
            problemViewModel.SelectedNode = null;

            //Act
            problemViewModel.SelectedNode = nodeViewModel;

            //Assert
            Assert.That(nodeViewModel.IsSelected, Is.True);
        }

        [Test]
        public void SelectedNode_SetNullNode_ExpetNodeIsSelectedIsFalse()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var nodeViewModel = problemViewModel.AddNode();

            //Act
            problemViewModel.SelectedNode = null;

            //Assert
            Assert.That(nodeViewModel.IsSelected, Is.False);
        }

        [Test]
        public void SelectedNode_SetNode_ExpectOtherPropertiesAreNull()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var nodeViewModel = problemViewModel.AddNode();
            problemViewModel.SelectedNode = null;

            //Act
            problemViewModel.SelectedNode = nodeViewModel;

            //Assert
            Assert.That(problemViewModel.SelectedElement, Is.Null);
            Assert.That(problemViewModel.SelectedForce, Is.Null);
            Assert.That(problemViewModel.SelectedMaterial, Is.Null);
        }

        [Test]
        public void SelectedNode_SetNode_ExpectPropertyChangedEventRaised()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var nodeViewModel = problemViewModel.AddNode();
            problemViewModel.SelectedNode = null;

            var checker = new PropertyChangedEventChecker(problemViewModel);

            //Act
            problemViewModel.SelectedNode = nodeViewModel;

            //Assert
            Assert.That(checker.PropertiesRaised, Is.EqualTo(new []
                {
                "SelectedNode",
                "SelectedElement",
                "SelectedForce",
                "SelectedMaterial",
                "AvailableNodesForSelectedForces",
                "AvailableOriginNodesForSelectedElements",
                "AvailableDestinationNodesForSelectedElements"
                }));
        }

        [Test]
        public void SelectedForce_SetForce_ExpectNodeIsSelectedIsTrue()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var nodeViewModel = problemViewModel.AddNode();
            var forceViewModel = problemViewModel.AddForce(nodeViewModel);
            problemViewModel.SelectedForce = null;

            //Act
            problemViewModel.SelectedForce = forceViewModel;

            //Assert
            Assert.That(forceViewModel.IsSelected, Is.True);
        }

        [Test]
        public void SelectedForce_SetNullForce_ExpetNodeIsSelectedIsFalse()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var nodeViewModel = problemViewModel.AddNode();
            var forceViewModel = problemViewModel.AddForce(nodeViewModel);

            //Act
            problemViewModel.SelectedForce = null;

            //Assert
            Assert.That(forceViewModel.IsSelected, Is.False);
        }

        [Test]
        public void SelectedForce_SetForce_ExpectOtherPropertiesAreNull()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var nodeViewModel = problemViewModel.AddNode();
            var forceViewModel = problemViewModel.AddForce(nodeViewModel);
            problemViewModel.SelectedForce = null;

            //Act
            problemViewModel.SelectedForce = forceViewModel;

            //Assert
            Assert.That(problemViewModel.SelectedElement, Is.Null);
            Assert.That(problemViewModel.SelectedNode, Is.Null);
            Assert.That(problemViewModel.SelectedMaterial, Is.Null);
        }

        [Test]
        public void SelectedForce_SetForce_ExpectPropertyChangedEventRaised()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var nodeViewModel = problemViewModel.AddNode();
            var forceViewModel = problemViewModel.AddForce(nodeViewModel);
            problemViewModel.SelectedForce = null;

            var checker = new PropertyChangedEventChecker(problemViewModel);

            //Act
            problemViewModel.SelectedForce = forceViewModel;

            //Assert
            Assert.That(checker.PropertiesRaised, Is.EqualTo(new[]
                {
                "SelectedNode",
                "SelectedElement",
                "SelectedForce",
                "SelectedMaterial",
                "AvailableNodesForSelectedForces",
                "AvailableOriginNodesForSelectedElements",
                "AvailableDestinationNodesForSelectedElements"
                }));
        }

        [Test]
        public void SelectedMaterial_SetMaterial_ExpectMaterialIsSelectedIsTrue()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var materialViewModel = problemViewModel.AddMaterial();
            problemViewModel.SelectedMaterial = null;

            //Act
            problemViewModel.SelectedMaterial = materialViewModel;

            //Assert
            Assert.That(materialViewModel.IsSelected, Is.True);
        }

        [Test]
        public void SelectedMaterial_SetNullMaterial_ExpetMaterialIsSelectedIsFalse()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var materialViewModel = problemViewModel.AddMaterial();

            //Act
            problemViewModel.SelectedMaterial = null;

            //Assert
            Assert.That(materialViewModel.IsSelected, Is.False);
        }

        [Test]
        public void SelectedMaterial_SetMaterial_ExpectOtherPropertiesAreNull()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var materialViewModel = problemViewModel.AddMaterial();
            problemViewModel.SelectedMaterial = null;

            //Act
            problemViewModel.SelectedMaterial = materialViewModel;

            //Assert
            Assert.That(problemViewModel.SelectedElement, Is.Null);
            Assert.That(problemViewModel.SelectedForce, Is.Null);
            Assert.That(problemViewModel.SelectedNode, Is.Null);
        }

        [Test]
        public void SelectedMaterial_SetMaterial_ExpectPropertyChangedEventRaised()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var materialViewModel = problemViewModel.AddMaterial();
            problemViewModel.SelectedMaterial = null;

            var checker = new PropertyChangedEventChecker(problemViewModel);

            //Act
            problemViewModel.SelectedMaterial = materialViewModel;

            //Assert
            Assert.That(checker.PropertiesRaised, Is.EqualTo(new[]
                {
                "SelectedNode",
                "SelectedElement",
                "SelectedForce",
                "SelectedMaterial",
                "AvailableNodesForSelectedForces",
                "AvailableOriginNodesForSelectedElements",
                "AvailableDestinationNodesForSelectedElements"
                }));
        }

        [Test]
        public void SelectedElement_SetElement_ExpectNodeIsSelectedIsTrue()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var node1 = problemViewModel.AddNode();
            var node2 = problemViewModel.AddNode();
            var elementViewModel = problemViewModel.AddBarElement(node1, node2);
            problemViewModel.SelectedElement = null;

            //Act
            problemViewModel.SelectedElement = elementViewModel;

            //Assert
            Assert.That(elementViewModel.IsSelected, Is.True);
        }

        [Test]
        public void SelectedElement_SetNullElement_ExpetNodeIsSelectedIsFalse()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var node1 = problemViewModel.AddNode();
            var node2 = problemViewModel.AddNode();
            var elementViewModel = problemViewModel.AddBarElement(node1, node2);

            //Act
            problemViewModel.SelectedElement = null;

            //Assert
            Assert.That(elementViewModel.IsSelected, Is.False);
        }

        [Test]
        public void SelectedElement_SetElement_ExpectOtherPropertiesAreNull()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var node1 = problemViewModel.AddNode();
            var node2 = problemViewModel.AddNode();
            var elementViewModel = problemViewModel.AddBarElement(node1, node2);
            problemViewModel.SelectedElement = null;

            //Act
            problemViewModel.SelectedElement = elementViewModel;

            //Assert
            Assert.That(problemViewModel.SelectedNode, Is.Null);
            Assert.That(problemViewModel.SelectedForce, Is.Null);
            Assert.That(problemViewModel.SelectedMaterial, Is.Null);
        }

        [Test]
        public void SelectedElement_SetElement_ExpectPropertyChangedEventRaised()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var node1 = problemViewModel.AddNode();
            var node2 = problemViewModel.AddNode();
            var elementViewModel = problemViewModel.AddBarElement(node1, node2);
            problemViewModel.SelectedElement = null;

            var checker = new PropertyChangedEventChecker(problemViewModel);

            //Act
            problemViewModel.SelectedElement = elementViewModel;

            //Assert
            Assert.That(checker.PropertiesRaised, Is.EqualTo(new[]
                {
                "SelectedNode",
                "SelectedElement",
                "SelectedForce",
                "SelectedMaterial",
                "AvailableNodesForSelectedForces",
                "AvailableOriginNodesForSelectedElements",
                "AvailableDestinationNodesForSelectedElements"
                }));
        }

        [Test]
        public void SelectedElement_SelectedElementOriginNodeChanged_ExpectPropertyChangedEventRaised()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var node1 = problemViewModel.AddNode();
            var node2 = problemViewModel.AddNode();
            var elementViewModel = problemViewModel.AddBarElement(node1, node2);
            problemViewModel.SelectedElement = null;

            problemViewModel.SelectedElement = elementViewModel;
            var checker = new PropertyChangedEventChecker(problemViewModel);

            //Act
            elementViewModel.Origin.X = 100;

            //Assert
            Assert.That(checker.PropertiesRaised, Is.EqualTo(new[]
                {
                "AvailableDestinationNodesForSelectedElements"
                }));
        }

        [Test]
        public void SelectedElement_SelectedElementDestinationNodeChanged_ExpectPropertyChangedEventRaised()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var node1 = problemViewModel.AddNode();
            var node2 = problemViewModel.AddNode();
            var elementViewModel = problemViewModel.AddBarElement(node1, node2);
            problemViewModel.SelectedElement = null;

            problemViewModel.SelectedElement = elementViewModel;
            var checker = new PropertyChangedEventChecker(problemViewModel);

            //Act
            elementViewModel.Destination.X = 100;

            //Assert
            Assert.That(checker.PropertiesRaised, Is.EqualTo(new[]
                {
                "AvailableOriginNodesForSelectedElements"
                }));
        }

        [Test]
        public void SelectedElement_ElementUnselectedAndElementNodesChanged_ExpectPropertyChangedEventNotRaised()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var node1 = problemViewModel.AddNode();
            var node2 = problemViewModel.AddNode();
            var elementViewModel = problemViewModel.AddBarElement(node1, node2);
            problemViewModel.SelectedElement = null;

            var checker = new PropertyChangedEventChecker(problemViewModel);

            //Act
            elementViewModel.Origin.X = 100;
            elementViewModel.Destination.X = 300; ;

            //Assert
            Assert.That(checker.PropertiesRaised, Is.Empty);
        }

        [Test]
        public void AddNode_EmptyProblem_ExpectNewNodeIsSelected()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();

            //Act
            var nodeViewModel = problemViewModel.AddNode();

            //Assert
            Assert.That(nodeViewModel.IsSelected, Is.True);
        }

        [Test]
        public void AddNode_EmptyProblem_ExpectNewNodeIsSelectedNode()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();

            //Act
            var nodeViewModel = problemViewModel.AddNode();

            //Assert
            Assert.That(problemViewModel.SelectedNode, Is.EqualTo(nodeViewModel));
        }

        [Test]
        public void AddNode_EmptyProblem_ExpectNewNodeAddedToNodesCollection()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();

            //Act
            var nodeViewModel = problemViewModel.AddNode();

            //Assert
            var nodeContained = problemViewModel.Nodes.Contains(nodeViewModel);
            Assert.That(nodeContained, Is.True);
        }

        [Test]
        public void DeleteSelectedNode_NullSelectedNode_ExpectNothingHappened()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var nodeViewModel = problemViewModel.AddNode();
            problemViewModel.SelectedNode = null;

            //Act
            //Assert
            Assert.DoesNotThrow(() => problemViewModel.DeleteSelectedNode());
        }

        [Test]
        public void DeleteSelectedNode_SelectedNode_ExpectNodeRemovedFromCollection()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var nodeViewModel = problemViewModel.AddNode();

            //Act
            problemViewModel.DeleteSelectedNode();

            //Assert
            var containsNode = problemViewModel.Nodes.Contains(nodeViewModel);
            Assert.That(containsNode, Is.False);
        }

        [Test]
        public void DeleteSelectedNode_SelectedNode_ExpectSelectedNodeIsNull()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            problemViewModel.AddNode();

            //Act
            problemViewModel.DeleteSelectedNode();

            //Assert
            Assert.That(problemViewModel.SelectedNode, Is.Null);
        }

        [Test]
        public void AddForce_EmptyProblem_ExpectNewForceIsSelected()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var nodeViewModel = problemViewModel.AddNode();

            //Act
            var forceViewModel = problemViewModel.AddForce(nodeViewModel);

            //Assert
            Assert.That(forceViewModel.IsSelected, Is.True);
        }

        [Test]
        public void AddForce_EmptyProblem_ExpectNewForceIsSelectedForce()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var nodeViewModel = problemViewModel.AddNode();

            //Act
            var forceViewModel = problemViewModel.AddForce(nodeViewModel);

            //Assert
            Assert.That(problemViewModel.SelectedForce, Is.EqualTo(forceViewModel));
        }

        [Test]
        public void AddForce_EmptyProblem_ExpectNewNodeAddedToForcesCollection()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var nodeViewModel = problemViewModel.AddNode();

            //Act
            var forceViewModel = problemViewModel.AddForce(nodeViewModel);

            //Assert
            var forceContained = problemViewModel.Forces.Contains(forceViewModel);
            Assert.That(forceContained, Is.True);
        }

        [Test]
        public void DeleteSelectedForce_NullSelectedForce_ExpectNothingHappened()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var nodeViewModel = problemViewModel.AddNode();
            var forceViewModel = problemViewModel.AddForce(nodeViewModel);
            problemViewModel.SelectedForce = null;

            //Act
            //Assert
            Assert.DoesNotThrow(() => problemViewModel.DeleteSelectedForce());
        }

        [Test]
        public void DeleteSelectedForce_SelectedForce_ExpectForceRemovedFromCollection()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var nodeViewModel = problemViewModel.AddNode();
            var forceViewModel = problemViewModel.AddForce(nodeViewModel);

            //Act
            problemViewModel.DeleteSelectedForce();

            //Assert
            var containsForce = problemViewModel.Forces.Contains(forceViewModel);
            Assert.That(containsForce, Is.False);
        }

        [Test]
        public void DeleteSelectedForce_SelectedForce_ExpectSelectedForceIsNull()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            problemViewModel.AddNode();

            //Act
            problemViewModel.DeleteSelectedForce();

            //Assert
            Assert.That(problemViewModel.SelectedForce, Is.Null);
        }

        [Test]
        public void AddBarElement_EmptyProblem_ExpectNewElementIsSelected()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var node1 = problemViewModel.AddNode();
            var node2 = problemViewModel.AddNode();

            //Act
            var elementViewModel = problemViewModel.AddBarElement(node1, node2);

            //Assert
            Assert.That(elementViewModel.IsSelected, Is.True);
        }

        [Test]
        public void AddBarElement_EmptyProblem_ExpectNewElementIsSelectedElement()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var node1 = problemViewModel.AddNode();
            var node2 = problemViewModel.AddNode();

            //Act
            var elementViewModel = problemViewModel.AddBarElement(node1, node2);

            //Assert
            Assert.That(problemViewModel.SelectedElement, Is.EqualTo(elementViewModel));
        }

        [Test]
        public void AddBarElement_EmptyProblem_ExpectNewElementAddedToElementsCollection()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var node1 = problemViewModel.AddNode();
            var node2 = problemViewModel.AddNode();

            //Act
            var elementViewModel = problemViewModel.AddBarElement(node1, node2);

            //Assert
            var elementContained = problemViewModel.Elements.Contains(elementViewModel);
            Assert.That(elementContained, Is.True);
        }

        [Test]
        public void DeleteSelectedElement_NullSelectedElement_ExpectNothingHappened()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var node1 = problemViewModel.AddNode();
            var node2 = problemViewModel.AddNode();
            var elementViewModel = problemViewModel.AddBarElement(node1, node2);
            problemViewModel.SelectedElement = null;

            //Act
            //Assert
            Assert.DoesNotThrow(() => problemViewModel.DeleteSelectedElement());
        }

        [Test]
        public void DeleteSelectedElement_SelectedElement_ExpectElementRemovedFromCollection()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var node1 = problemViewModel.AddNode();
            var node2 = problemViewModel.AddNode();
            var elementViewModel = problemViewModel.AddBarElement(node1, node2);

            //Act
            problemViewModel.DeleteSelectedElement();

            //Assert
            var containsElement = problemViewModel.Elements.Contains(elementViewModel);
            Assert.That(containsElement, Is.False);
        }

        [Test]
        public void DeleteSelectedElement_SelectedElement_ExpectSelectedElementIsNull()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var node1 = problemViewModel.AddNode();
            var node2 = problemViewModel.AddNode();
            var elementViewModel = problemViewModel.AddBarElement(node1, node2);

            //Act
            problemViewModel.DeleteSelectedElement();

            //Assert
            Assert.That(problemViewModel.SelectedElement, Is.Null);
        }

        [Test]
        public void AddMaterial_EmptyProblem_ExpectNewMaterialIsSelected()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();

            //Act
            var materialViewModel = problemViewModel.AddMaterial();

            //Assert
            Assert.That(materialViewModel.IsSelected, Is.True);
        }

        [Test]
        public void AddMaterial_EmptyProblem_ExpectNewMaterialIsSelectedMaterial()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();

            //Act
            var materialViewModel = problemViewModel.AddMaterial();

            //Assert
            Assert.That(problemViewModel.SelectedMaterial, Is.EqualTo(materialViewModel));
        }

        [Test]
        public void AddMaterial_EmptyProblem_ExpectNewMaterialAddedToMaterialsCollection()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();

            //Act
            var materialViewModel = problemViewModel.AddMaterial();

            //Assert
            var materialContained = problemViewModel.Materials.Contains(materialViewModel);
            Assert.That(materialContained, Is.True);
        }

        [Test]
        public void DeleteSelectedMaterial_NullSelectedMaterial_ExpectNothingHappened()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var materialViewModel = problemViewModel.AddMaterial();
            problemViewModel.SelectedMaterial = null;

            //Act
            //Assert
            Assert.DoesNotThrow(() => problemViewModel.DeleteSelectedMaterial());
        }

        [Test]
        public void DeleteSelectedMaterial_SelectedMaterial_ExpectMaterialRemovedFromCollection()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            var materialViewModel = problemViewModel.AddMaterial();

            //Act
            problemViewModel.DeleteSelectedMaterial();

            //Assert
            var containsMaterial = problemViewModel.Materials.Contains(materialViewModel);
            Assert.That(containsMaterial, Is.False);
        }

        [Test]
        public void DeleteSelectedMaterial_SelectedMaterial_ExpectSelectedMaterialIsNull()
        {
            //Arrange
            var problemViewModel = new ProblemViewModel();
            problemViewModel.AddMaterial();

            //Act
            problemViewModel.DeleteSelectedMaterial();

            //Assert
            Assert.That(problemViewModel.SelectedMaterial, Is.Null);
        }
    }
}
