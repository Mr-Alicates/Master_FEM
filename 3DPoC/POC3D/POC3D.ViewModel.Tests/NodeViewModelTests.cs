using NUnit.Framework;
using POC3D.Model;
using POC3D.ViewModel.Implementation;
using POC3D.ViewModel.Tests.Helper;

namespace POC3D.ViewModel.Tests
{
    [TestFixture]
    public class NodeViewModelTests
    {
        [Test]
        public void Ctor_ExpectPropertiesCorrectlyInitialized()
        {
            //Arrange
            var modelNode = BuildTestNode();

            //Act
            var nodeViewModel = new NodeViewModel(modelNode);

            //Assert
            Assert.That(nodeViewModel.Node, Is.EqualTo(modelNode));
            Assert.That(nodeViewModel.Id, Is.EqualTo(modelNode.Id));
            Assert.That(nodeViewModel.Name, Is.EqualTo("1 (0;0;0)"));
            Assert.That(nodeViewModel.Geometry, Is.Not.Null);
            Assert.That(nodeViewModel.ResultGeometry, Is.Not.Null);
        }

        [Test]
        public void X_ValueChanged_ExpectModelUpdated()
        {
            //Arrange
            var modelNode = BuildTestNode();
            var nodeViewModel = new NodeViewModel(modelNode);

            //Act
            nodeViewModel.X = 1234;

            //Assert
            Assert.That(modelNode.Coordinates.X, Is.EqualTo(1234));
        }

        [Test]
        public void X_ValueChanged_ExpectCoordinatesUpdated()
        {
            //Arrange
            var modelNode = BuildTestNode();
            var nodeViewModel = new NodeViewModel(modelNode);

            //Act
            nodeViewModel.X = 1234;

            //Assert
            Assert.That(nodeViewModel.Coordinates.X, Is.EqualTo(1234));
        }

        [Test]
        public void X_ValueChanged_ExpectNameUpdated()
        {
            //Arrange
            var modelNode = BuildTestNode();
            var nodeViewModel = new NodeViewModel(modelNode);

            //Act
            nodeViewModel.X = 1234;

            //Assert
            Assert.That(nodeViewModel.Name, Is.EqualTo("1 (1234;0;0)"));
        }

        [Test]
        public void X_ValueChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var modelNode = BuildTestNode();
            var nodeViewModel = new NodeViewModel(modelNode);

            var eventChecker = new PropertyChangedEventChecker(nodeViewModel);

            //Act
            nodeViewModel.X = 1234;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "X",
                "Coordinates",
                "Name",
                "Geometry",
                "ResultGeometry"
            }));
        }

        [Test]
        public void Y_ValueChanged_ExpectModelUpdated()
        {
            //Arrange
            var modelNode = BuildTestNode();
            var nodeViewModel = new NodeViewModel(modelNode);

            //Act
            nodeViewModel.Y = 1234;

            //Assert
            Assert.That(modelNode.Coordinates.Y, Is.EqualTo(1234));
        }

        [Test]
        public void Y_ValueChanged_ExpectCoordinatesUpdated()
        {
            //Arrange
            var modelNode = BuildTestNode();
            var nodeViewModel = new NodeViewModel(modelNode);

            //Act
            nodeViewModel.Y = 1234;

            //Assert
            Assert.That(nodeViewModel.Coordinates.Y, Is.EqualTo(1234));
        }

        [Test]
        public void Y_ValueChanged_ExpectNameUpdated()
        {
            //Arrange
            var modelNode = BuildTestNode();
            var nodeViewModel = new NodeViewModel(modelNode);

            //Act
            nodeViewModel.Y = 1234;

            //Assert
            Assert.That(nodeViewModel.Name, Is.EqualTo("1 (0;1234;0)"));
        }

        [Test]
        public void Y_ValueChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var modelNode = BuildTestNode();
            var nodeViewModel = new NodeViewModel(modelNode);

            var eventChecker = new PropertyChangedEventChecker(nodeViewModel);

            //Act
            nodeViewModel.Y = 1234;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "Y",
                "Coordinates",
                "Name",
                "Geometry",
                "ResultGeometry"
            }));
        }

        [Test]
        public void Z_ValueChanged_ExpectModelUpdated()
        {
            //Arrange
            var modelNode = BuildTestNode();
            var nodeViewModel = new NodeViewModel(modelNode);

            //Act
            nodeViewModel.Z = 1234;

            //Assert
            Assert.That(modelNode.Coordinates.Z, Is.EqualTo(1234));
        }

        [Test]
        public void Z_ValueChanged_ExpectCoordinatesUpdated()
        {
            //Arrange
            var modelNode = BuildTestNode();
            var nodeViewModel = new NodeViewModel(modelNode);

            //Act
            nodeViewModel.Z = 1234;

            //Assert
            Assert.That(nodeViewModel.Coordinates.Z, Is.EqualTo(1234));
        }

        [Test]
        public void Z_ValueChanged_ExpectNameUpdated()
        {
            //Arrange
            var modelNode = BuildTestNode();
            var nodeViewModel = new NodeViewModel(modelNode);

            //Act
            nodeViewModel.Z = 1234;

            //Assert
            Assert.That(nodeViewModel.Name, Is.EqualTo("1 (0;0;1234)"));
        }

        [Test]
        public void Z_ValueChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var modelNode = BuildTestNode();
            var nodeViewModel = new NodeViewModel(modelNode);

            var eventChecker = new PropertyChangedEventChecker(nodeViewModel);

            //Act
            nodeViewModel.Z = 1234;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "Z",
                "Coordinates",
                "Name",
                "Geometry",
                "ResultGeometry"
            }));
        }
                
        [Test]
        public void DisplacementX_ValueChanged_ExpectDisplacementUpdated()
        {
            //Arrange
            var modelNode = BuildTestNode();
            var nodeViewModel = new NodeViewModel(modelNode);

            //Act
            nodeViewModel.DisplacementX = 1234;

            //Assert
            Assert.That(nodeViewModel.Displacement.X, Is.EqualTo(1234));
        }

        [Test]
        public void DisplacementX_ValueChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var modelNode = BuildTestNode();
            var nodeViewModel = new NodeViewModel(modelNode);

            var eventChecker = new PropertyChangedEventChecker(nodeViewModel);

            //Act
            nodeViewModel.DisplacementX = 1234;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "DisplacementX",
                "Displacement",
                "ResultGeometry"
            }));
        }

        [Test]
        public void DisplacementY_ValueChanged_ExpectDisplacementUpdated()
        {
            //Arrange
            var modelNode = BuildTestNode();
            var nodeViewModel = new NodeViewModel(modelNode);

            //Act
            nodeViewModel.DisplacementY = 1234;

            //Assert
            Assert.That(nodeViewModel.Displacement.Y, Is.EqualTo(1234));
        }

        [Test]
        public void DisplacementY_ValueChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var modelNode = BuildTestNode();
            var nodeViewModel = new NodeViewModel(modelNode);

            var eventChecker = new PropertyChangedEventChecker(nodeViewModel);

            //Act
            nodeViewModel.DisplacementY = 1234;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "DisplacementY",
                "Displacement",
                "ResultGeometry"
            }));
        }

        [Test]
        public void DisplacementZ_ValueChanged_ExpectDisplacementUpdated()
        {
            //Arrange
            var modelNode = BuildTestNode();
            var nodeViewModel = new NodeViewModel(modelNode);

            //Act
            nodeViewModel.DisplacementZ = 1234;

            //Assert
            Assert.That(nodeViewModel.Displacement.Z, Is.EqualTo(1234));
        }

        [Test]
        public void DisplacementZ_ValueChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var modelNode = BuildTestNode();
            var nodeViewModel = new NodeViewModel(modelNode);

            var eventChecker = new PropertyChangedEventChecker(nodeViewModel);

            //Act
            nodeViewModel.DisplacementZ = 1234;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "DisplacementZ",
                "Displacement",
                "ResultGeometry"
            }));
        }

        private static IModelNode BuildTestNode()
        {
            var modelProblem = new ModelProblem("test");

            var node = modelProblem.AddNode();

            return node;
        }
    }
}
