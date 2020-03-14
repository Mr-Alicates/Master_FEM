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

            var origin = new ModelNode(123);
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
            var destination = new ModelNode(123);

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

            var node = new ModelNode(123);

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
        public void DeleteNode_NullNode_ExpectArgumentNullException()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => modelProblem.DeleteNode(null));
        }

        [Test]
        public void DeleteNode_NodeDoesNotExistInProblem_ExpectNothingHappens()
        {
            //Arrange
            var modelProblem = new ModelProblem("problem");

            var strayNode = new ModelNode(666);

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

            var strayElement = new ModelBarElement(1234, new ModelNode(123), new ModelNode(1234));

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

            var strayForce = new ModelForce(345, new ModelNode(1234));

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
    }
}
