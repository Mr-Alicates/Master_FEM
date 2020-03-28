using NUnit.Framework;
using System;
using System.Linq;

namespace POC3D.Model.Tests
{
    [TestFixture]
    public class ModelProblemTests
    {
        [Test]
        public void Ctor_ExpectPropertiesCorrectlyInitialized()
        {
            //Arrange
            //Act
            var modelProblem = new ModelProblem("problem name");

            //Assert
            Assert.That(modelProblem.Name, Is.EqualTo("problem name"));
            Assert.That(modelProblem.Nodes, Is.Empty);
            Assert.That(modelProblem.Elements, Is.Empty);
            Assert.That(modelProblem.Forces, Is.Empty);
        }

        [Test]
        public void AddNode_EmptyProblem_ExpectNodeAddedToCollection()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            //Act
            var node = modelProblem.AddNode();

            //Assert
            Assert.That(modelProblem.Nodes.Contains(node), Is.True);
        }

        [Test]
        public void AddNode_EmptyProblem_ExpectNodeIdCorrectlyInitialized()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            //Act
            var node = modelProblem.AddNode();

            //Assert
            Assert.That(node.Id, Is.EqualTo(1));
        }

        [Test]
        public void AddNode_ProblemWithNodes_ExpectNodeIdCorrectlyInitialized()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");
            modelProblem.AddNode();
            modelProblem.AddNode();
            modelProblem.AddNode();

            //Act
            var node = modelProblem.AddNode();

            //Assert
            Assert.That(node.Id, Is.EqualTo(4));
        }

        [Test]
        public void AddElement_NullOriginNode_ExpectArgumentNullException()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var node = modelProblem.AddNode();

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => modelProblem.AddElement(null, node));
        }

        [Test]
        public void AddElement_NullDestinationNode_ExpectArgumentNullException()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var node = modelProblem.AddNode();

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => modelProblem.AddElement(node, null));
        }

        [Test]
        public void AddElement_InvalidOriginNode_ExpectInvalidOperationException()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var origin = new ModelNode(new ModelProblem("otherProblem"), 123);
            var destination = modelProblem.AddNode();

            //Act
            //Assert
            Assert.Throws<InvalidOperationException>(() => modelProblem.AddElement(origin, destination));
        }

        [Test]
        public void AddElement_InvalidDestinationNode_ExpectInvalidOperationException()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var origin = modelProblem.AddNode();
            var destination = new ModelNode(new ModelProblem("otherProblem"), 123);

            //Act
            //Assert
            Assert.Throws<InvalidOperationException>(() => modelProblem.AddElement(origin, destination));
        }

        [Test]
        public void AddElement_BothNodesAreTheSame_ExpectInvalidOperationException()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var origin = modelProblem.AddNode();

            //Act
            //Assert
            Assert.Throws<InvalidOperationException>(() => modelProblem.AddElement(origin, origin));
        }

        [Test]
        public void AddElement_ElementWithSameNodesAlreadyExists_ExpectInvalidOperationException()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var origin = modelProblem.AddNode();
            var destination = modelProblem.AddNode();
            modelProblem.AddElement(origin, destination);

            //Act
            //Assert
            Assert.Throws<InvalidOperationException>(() => modelProblem.AddElement(origin, destination));
        }

        [Test]
        public void AddElement_EmptyProblem_ExpectElementAddedToCollection()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var origin = modelProblem.AddNode();
            var destination = modelProblem.AddNode();

            //Act
            var element = modelProblem.AddElement(origin, destination);

            //Assert
            Assert.That(modelProblem.Elements.Contains(element), Is.True);
        }

        [Test]
        public void AddElement_EmptyProblem_ExpectElementMaterialAddedToCollection()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var origin = modelProblem.AddNode();
            var destination = modelProblem.AddNode();

            //Act
            var element = modelProblem.AddElement(origin, destination);

            //Assert
            Assert.That(modelProblem.Materials.Contains(element.Material), Is.True);
        }

        [Test]
        public void AddElement_ProblemWithMaterials_ExpectNoNewMaterialWasCreated()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var origin = modelProblem.AddNode();
            var destination = modelProblem.AddNode();

            modelProblem.AddElement(modelProblem.AddNode(), modelProblem.AddNode());

            //Act
            var element = modelProblem.AddElement(origin, destination);

            //Assert
            Assert.That(modelProblem.Materials.Count(), Is.EqualTo(1));
        }

        [Test]
        public void AddElement_ProblemWithMaterials_ExpectElementMaterialIsExistingMaterial()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var origin = modelProblem.AddNode();
            var destination = modelProblem.AddNode();

            var otherElement = modelProblem.AddElement(modelProblem.AddNode(), modelProblem.AddNode());

            //Act
            var element = modelProblem.AddElement(origin, destination);

            //Assert
            Assert.That(element.Material, Is.EqualTo(otherElement.Material));
        }

        [Test]
        public void AddElement_EmptyProblem_ExpectElementIdCorrectlyInitialized()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var origin = modelProblem.AddNode();
            var destination = modelProblem.AddNode();

            //Act
            var element = modelProblem.AddElement(origin, destination);

            //Assert
            Assert.That(element.Id, Is.EqualTo(1));
        }

        [Test]
        public void AddElement_ProblemWithElements_ExpectElementIdCorrectlyInitialized()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var origin = modelProblem.AddNode();
            var destination = modelProblem.AddNode();

            var origin2 = modelProblem.AddNode();
            var destination2 = modelProblem.AddNode();

            modelProblem.AddElement(origin, destination);

            //Act
            var element = modelProblem.AddElement(origin2, destination2);

            //Assert
            Assert.That(element.Id, Is.EqualTo(2));
        }

        [Test]
        public void AddForce_NullNode_ExpectArgumentNullException()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => modelProblem.AddForce(null));
        }

        [Test]
        public void AddForce_InvalidOriginNode_ExpectInvalidOperationException()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var node = new ModelNode(new ModelProblem("otherProblem"), 123);

            //Act
            //Assert
            Assert.Throws<InvalidOperationException>(() => modelProblem.AddForce(node));
        }

        [Test]
        public void AddForce_ForceWithSameNodeAlreadyExists_ExpectInvalidOperationException()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var node = modelProblem.AddNode();
            modelProblem.AddForce(node);

            //Act
            //Assert
            Assert.Throws<InvalidOperationException>(() => modelProblem.AddForce(node));
        }

        [Test]
        public void AddForce_EmptyProblem_ExpectForceAddedToCollection()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var node = modelProblem.AddNode();

            //Act
            var force = modelProblem.AddForce(node);

            //Assert
            Assert.That(modelProblem.Forces.Contains(force), Is.True);
        }

        [Test]
        public void AddForce_EmptyProblem_ExpectElementIdCorrectlyInitialized()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var node = modelProblem.AddNode();

            //Act
            var force = modelProblem.AddForce(node);

            //Assert
            Assert.That(force.Id, Is.EqualTo(1));
        }

        [Test]
        public void AddForce_ProblemWithForces_ExpectElementIdCorrectlyInitialized()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var node = modelProblem.AddNode();
            var node2 = modelProblem.AddNode();
            var node3 = modelProblem.AddNode();

            modelProblem.AddForce(node);
            modelProblem.AddForce(node2);

            //Act
            var force = modelProblem.AddForce(node3);

            //Assert
            Assert.That(force.Id, Is.EqualTo(3));
        }

        [Test]
        public void AddMaterial_EmptyProblem_ExpectMaterialAddedToCollection()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            //Act
            var material = modelProblem.AddMaterial();

            //Assert
            Assert.That(modelProblem.Materials.Contains(material), Is.True);
        }

        [Test]
        public void AddMaterial_EmptyProblem_ExpectMaterialIdCorrectlyInitialized()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            //Act
            var material = modelProblem.AddMaterial();

            //Assert
            Assert.That(material.Id, Is.EqualTo(1));
        }

        [Test]
        public void AddMaterial_ProblemWithMaterials_ExpectMaterialIdCorrectlyInitialized()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");
            modelProblem.AddMaterial();
            modelProblem.AddMaterial();
            modelProblem.AddMaterial();

            //Act
            var material = modelProblem.AddMaterial();

            //Assert
            Assert.That(material.Id, Is.EqualTo(4));
        }

        [Test]
        public void DeleteNode_NullNode_ExpectArgumentNullException()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => modelProblem.DeleteNode(null));
        }

        [Test]
        public void DeleteNode_NodeIsUsedByElementInOriginNode_ExpectInvalidOperationException()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var node = modelProblem.AddNode();
            var element = modelProblem.AddElement(node, modelProblem.AddNode());

            //Act
            //Assert
            Assert.Throws<InvalidOperationException>(() => modelProblem.DeleteNode(node));
        }

        [Test]
        public void DeleteNode_NodeIsUsedByElementInForce_ExpectInvalidOperationException()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var node = modelProblem.AddNode();
            var force = modelProblem.AddForce(node);

            //Act
            //Assert
            Assert.Throws<InvalidOperationException>(() => modelProblem.DeleteNode(node));
        }

        [Test]
        public void DeleteNode_NodeIsUsedByElementInDestinationNode_ExpectInvalidOperationException()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var node = modelProblem.AddNode();
            var element = modelProblem.AddElement(modelProblem.AddNode(), node);

            //Act
            //Assert
            Assert.Throws<InvalidOperationException>(() => modelProblem.DeleteNode(node));
        }

        [Test]
        public void DeleteNode_NodeDoesNotExistInProblem_ExpectNothingHappens()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var strayNode = new ModelNode(new ModelProblem("otherProblem"), 666);

            //Act
            modelProblem.DeleteNode(strayNode);

            //Assert
            Assert.That(modelProblem.Nodes, Is.Empty);
        }

        [Test]
        public void DeleteNode_NodeDoesExistInProblem_ExpectNodeRemovedFromCollection()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var node = modelProblem.AddNode();

            //Act
            modelProblem.DeleteNode(node);

            //Assert
            Assert.That(modelProblem.Nodes, Is.Empty);
        }

        [Test]
        public void DeleteNode_ProblemWithSeveralNodes_ExpectNodeIdsAreCorrect()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var node1 = modelProblem.AddNode();
            var node2 = modelProblem.AddNode();
            var node3 = modelProblem.AddNode();
            var node4 = modelProblem.AddNode();
            var node5 = modelProblem.AddNode();

            //Act
            modelProblem.DeleteNode(node1);

            //Assert
            Assert.AreEqual(node2.Id, 1);
            Assert.AreEqual(node3.Id, 2);
            Assert.AreEqual(node4.Id, 3);
            Assert.AreEqual(node5.Id, 4);
        }

        [Test]
        public void DeleteElement_NullElement_ExpectArgumentNullException()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => modelProblem.DeleteElement(null));
        }

        [Test]
        public void DeleteElement_ElementDoesNotExistInProblem_ExpectNothingHappens()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var otherProblem = new ModelProblem("otherProblem");
            var strayElement = new ModelBarElement(otherProblem, 1234, new ModelNode(otherProblem, 123), new ModelNode(otherProblem, 1234));

            //Act
            modelProblem.DeleteElement(strayElement);

            //Assert
            Assert.That(modelProblem.Elements, Is.Empty);
        }

        [Test]
        public void DeleteElement_ElementDoesExistInProblem_ExpectElementRemovedFromCollection()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var origin = modelProblem.AddNode();
            var destination = modelProblem.AddNode();

            var element = modelProblem.AddElement(origin, destination);

            //Act
            modelProblem.DeleteElement(element);

            //Assert
            Assert.That(modelProblem.Elements, Is.Empty);
        }

        [Test]
        public void DeleteElement_ProblemWithSeveralElements_ExpectElementIdsAreCorrect()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var element1 = modelProblem.AddElement(modelProblem.AddNode(), modelProblem.AddNode());
            var element2 = modelProblem.AddElement(modelProblem.AddNode(), modelProblem.AddNode());
            var element3 = modelProblem.AddElement(modelProblem.AddNode(), modelProblem.AddNode());
            var element4 = modelProblem.AddElement(modelProblem.AddNode(), modelProblem.AddNode());

            //Act
            modelProblem.DeleteElement(element1);

            //Assert
            Assert.AreEqual(element2.Id, 1);
            Assert.AreEqual(element3.Id, 2);
            Assert.AreEqual(element4.Id, 3);
        }

        [Test]
        public void DeleteElement_ElementDoesExistInProblem_ExpectElementMaterialRemovedFromCollection()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var origin = modelProblem.AddNode();
            var destination = modelProblem.AddNode();

            var element = modelProblem.AddElement(origin, destination);

            //Act
            modelProblem.DeleteElement(element);

            //Assert
            Assert.That(modelProblem.Elements, Is.Empty);
        }

        [Test]
        public void DeleteForce_NullForce_ExpectArgumentNullException()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => modelProblem.DeleteForce(null));
        }

        [Test]
        public void DeleteForce_ForceDoesNotExistInProblem_ExpectNothingHappens()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var otherProblem = new ModelProblem("otherProblem");
            var strayForce = new ModelForce(otherProblem, 345, new ModelNode(otherProblem, 1234));

            //Act
            modelProblem.DeleteForce(strayForce);

            //Assert
            Assert.That(modelProblem.Forces, Is.Empty);
        }

        [Test]
        public void DeleteForce_ForceDoesExistInProblem_ExpectForceRemovedFromCollection()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var force = modelProblem.AddForce(modelProblem.AddNode());

            //Act
            modelProblem.DeleteForce(force);

            //Assert
            Assert.That(modelProblem.Forces, Is.Empty);
        }

        [Test]
        public void DeleteForce_ProblemWithSeveralForces_ExpectForceIdsAreCorrect()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var force1 = modelProblem.AddForce(modelProblem.AddNode());
            var force2 = modelProblem.AddForce(modelProblem.AddNode());
            var force3 = modelProblem.AddForce(modelProblem.AddNode());
            var force4 = modelProblem.AddForce(modelProblem.AddNode());
            var force5 = modelProblem.AddForce(modelProblem.AddNode());

            //Act
            modelProblem.DeleteForce(force1);

            //Assert
            Assert.AreEqual(force2.Id, 1);
            Assert.AreEqual(force3.Id, 2);
            Assert.AreEqual(force4.Id, 3);
            Assert.AreEqual(force5.Id, 4);
        }

        [Test]
        public void DeleteMaterial_NullMaterial_ExpectArgumentNullException()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => modelProblem.DeleteMaterial(null));
        }

        [Test]
        public void DeleteMaterial_MaterialIsUsedByElement_ExpectInvalidOperationException()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var element = modelProblem.AddElement(modelProblem.AddNode(), modelProblem.AddNode());

            //Act
            //Assert
            Assert.Throws<InvalidOperationException>(() => modelProblem.DeleteMaterial(element.Material));
        }

        [Test]
        public void DeleteMaterial_MaterialDoesNotExistInProblem_ExpectNothingHappens()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var otherProblem = new ModelProblem("otherProblem");
            var strayMaterial = new ModelMaterial(otherProblem, 666, "strayMaterial", 1);

            //Act
            modelProblem.DeleteMaterial(strayMaterial);

            //Assert
            Assert.That(modelProblem.Materials, Is.Empty);
        }

        [Test]
        public void DeleteMaterial_MaterialDoesExistInProblem_ExpectMaterialRemovedFromCollection()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var material = modelProblem.AddMaterial();

            //Act
            modelProblem.DeleteMaterial(material);

            //Assert
            Assert.That(modelProblem.Materials, Is.Empty);
        }

        [Test]
        public void DeleteMaterial_ProblemWithSeveralMaterials_ExpectMaterialIdsAreCorrect()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var material1 = modelProblem.AddMaterial();
            var material2 = modelProblem.AddMaterial();
            var material3 = modelProblem.AddMaterial();
            var material4 = modelProblem.AddMaterial();
            var material5 = modelProblem.AddMaterial();

            //Act
            modelProblem.DeleteMaterial(material1);

            //Assert
            Assert.AreEqual(material2.Id, 1);
            Assert.AreEqual(material3.Id, 2);
            Assert.AreEqual(material4.Id, 3);
            Assert.AreEqual(material5.Id, 4);
        }
    }
}
