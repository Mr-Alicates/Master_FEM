using NUnit.Framework;
using POC3D.Model;
using POC3D.ViewModel.Implementation;
using POC3D.ViewModel.Tests.Helper;
using System;

namespace POC3D.ViewModel.Tests
{
    [TestFixture]
    public class ForceViewModelTests
    {
        [Test]
        public void Ctor_ExpectPropertiesCorrectlyInitialized()
        {
            //Arrange
            var (nodeViewModel, modelForce) = BuildTestForce();

            //Act
            var forceViewModel = new ForceViewModel(modelForce, nodeViewModel);

            //Assert
            Assert.That(forceViewModel.Force, Is.EqualTo(modelForce));
            Assert.That(forceViewModel.Node, Is.EqualTo(nodeViewModel));
            Assert.That(forceViewModel.Name, Is.EqualTo("(1) ---> (0,00/0,00/0,00) (0,00)"));
            Assert.That(forceViewModel.Geometry, Is.Not.Null);
            Assert.That(forceViewModel.ResultGeometry, Is.Not.Null);
        }

        [Test]
        public void Node_XPropertyOfNodeChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var (nodeViewModel, modelForce) = BuildTestForce();
            var forceViewModel = new ForceViewModel(modelForce, nodeViewModel);

            var eventChecker = new PropertyChangedEventChecker(forceViewModel);

            //Act
            nodeViewModel.X = 100;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "Node",
                "ResultGeometry"
            }));
        }

        [Test]
        public void Node_YPropertyOfNodeChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var (nodeViewModel, modelForce) = BuildTestForce();
            var forceViewModel = new ForceViewModel(modelForce, nodeViewModel);

            var eventChecker = new PropertyChangedEventChecker(forceViewModel);

            //Act
            nodeViewModel.Y = 100;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "Node",
                "ResultGeometry"
            }));
        }

        [Test]
        public void Node_ZPropertyOfNodeChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var (nodeViewModel, modelForce) = BuildTestForce();
            var forceViewModel = new ForceViewModel(modelForce, nodeViewModel);

            var eventChecker = new PropertyChangedEventChecker(forceViewModel);

            //Act
            nodeViewModel.X = 100;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "Node",
                "ResultGeometry"
            }));
        }

        [Test]
        public void Node_NonRelevantPropertyOfNodeChanged_ExpectPropertiesChangedEventNotRaised()
        {
            //Arrange
            var (nodeViewModel, modelForce) = BuildTestForce();
            var forceViewModel = new ForceViewModel(modelForce, nodeViewModel);

            var eventChecker = new PropertyChangedEventChecker(forceViewModel);

            //Act
            nodeViewModel.IsXFixed = true;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.Empty);
        }

        [Test]
        public void Node_SetNull_ExpectArgumentNullExceptionThrown()
        {
            //Arrange
            var (nodeViewModel, modelForce) = BuildTestForce();
            var forceViewModel = new ForceViewModel(modelForce, nodeViewModel);

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => forceViewModel.Node = null);
        }

        [Test]
        public void Node_NodeChanged_ExpectModelUpdated()
        {
            //Arrange
            var modelProblem = new ModelProblem("test");

            var node1 = modelProblem.AddNode();
            var node2 = modelProblem.AddNode();

            var modelForce = modelProblem.AddForce(node1);

            var nodeViewModel1 = new NodeViewModel(node1);
            var nodeViewModel2 = new NodeViewModel(node2);

            var forceViewModel = new ForceViewModel(modelForce, nodeViewModel1);

            //Act
            forceViewModel.Node = nodeViewModel2;

            //Assert
            Assert.That(modelForce.Node, Is.EqualTo(node2));
        }

        [Test]
        public void Node_NodeChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var modelProblem = new ModelProblem("test");

            var node1 = modelProblem.AddNode();
            var node2 = modelProblem.AddNode();

            var modelForce = modelProblem.AddForce(node1);

            var nodeViewModel1 = new NodeViewModel(node1);
            var nodeViewModel2 = new NodeViewModel(node2);

            var forceViewModel = new ForceViewModel(modelForce, nodeViewModel1);

            var eventChecker = new PropertyChangedEventChecker(forceViewModel);

            //Act
            forceViewModel.Node = nodeViewModel2;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "Node"
            }));
        }

        [Test]
        public void Node_NodeChangedAndRelevantPropertyOfNewNodeChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var modelProblem = new ModelProblem("test");

            var node1 = modelProblem.AddNode();
            var node2 = modelProblem.AddNode();

            var modelForce = modelProblem.AddForce(node1);

            var nodeViewModel1 = new NodeViewModel(node1);
            var nodeViewModel2 = new NodeViewModel(node2);

            var forceViewModel = new ForceViewModel(modelForce, nodeViewModel1);

            forceViewModel.Node = nodeViewModel2;
            var eventChecker = new PropertyChangedEventChecker(forceViewModel);

            //Act
            nodeViewModel2.X = 123;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "Node",
                "ResultGeometry"
            }));
        }

        [Test]
        public void Node_NodeChangedAndRelevantPropertyOfOldNodeChanged_ExpectPropertiesChangedEventNotRaised()
        {
            //Arrange
            var modelProblem = new ModelProblem("test");

            var node1 = modelProblem.AddNode();
            var node2 = modelProblem.AddNode();

            var modelForce = modelProblem.AddForce(node1);

            var nodeViewModel1 = new NodeViewModel(node1);
            var nodeViewModel2 = new NodeViewModel(node2);

            var forceViewModel = new ForceViewModel(modelForce, nodeViewModel1);

            forceViewModel.Node = nodeViewModel2;
            var eventChecker = new PropertyChangedEventChecker(forceViewModel);

            //Act
            nodeViewModel1.X = 123;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.Empty);
        }

        [Test]
        public void ApplicationVectorX_ValueChanged_ExpectModelUpdated()
        {
            //Arrange
            var (nodeViewModel, modelForce) = BuildTestForce();
            var forceViewModel = new ForceViewModel(modelForce, nodeViewModel);

            //Act
            forceViewModel.ApplicationVectorX = 3124;

            //Assert
            Assert.That(modelForce.ApplicationVector.X, Is.EqualTo(3124));
            Assert.That(modelForce.Magnitude, Is.EqualTo(3124));
        }

        [Test]
        public void ApplicationVectorX_ValueChanged_ExpectApplicationVectorUpdated()
        {
            //Arrange
            var (nodeViewModel, modelForce) = BuildTestForce();
            var forceViewModel = new ForceViewModel(modelForce, nodeViewModel);

            //Act
            forceViewModel.ApplicationVectorX = 3124;

            //Assert
            Assert.That(forceViewModel.ApplicationVector.X, Is.EqualTo(3124));
        }

        [Test]
        public void ApplicationVectorX_ValueChanged_ExpectMagnitudeUpdated()
        {
            //Arrange
            var (nodeViewModel, modelForce) = BuildTestForce();
            var forceViewModel = new ForceViewModel(modelForce, nodeViewModel);

            //Act
            forceViewModel.ApplicationVectorX = 3124;

            //Assert
            Assert.That(forceViewModel.Magnitude, Is.EqualTo(3124));
        }

        [Test]
        public void ApplicationVectorX_ValueChanged_ExpectNameUpdated()
        {
            //Arrange
            var (nodeViewModel, modelForce) = BuildTestForce();
            var forceViewModel = new ForceViewModel(modelForce, nodeViewModel);

            //Act
            forceViewModel.ApplicationVectorX = 3124;

            //Assert
            Assert.That(forceViewModel.Name, Is.EqualTo("(1) ---> (3.124,00/0,00/0,00) (3.124,00)"));
        }

        [Test]
        public void ApplicationVectorX_ValueChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var (nodeViewModel, modelForce) = BuildTestForce();
            var forceViewModel = new ForceViewModel(modelForce, nodeViewModel);

            var eventChecker = new PropertyChangedEventChecker(forceViewModel);

            //Act
            forceViewModel.ApplicationVectorX = 3124;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "ApplicationVectorX",
                "ApplicationVector",
                "Magnitude",
                "Name"
            }));
        }

        [Test]
        public void ApplicationVectorY_ValueChanged_ExpectModelUpdated()
        {
            //Arrange
            var (nodeViewModel, modelForce) = BuildTestForce();
            var forceViewModel = new ForceViewModel(modelForce, nodeViewModel);

            //Act
            forceViewModel.ApplicationVectorY = 3124;

            //Assert
            Assert.That(modelForce.ApplicationVector.Y, Is.EqualTo(3124));
            Assert.That(modelForce.Magnitude, Is.EqualTo(3124));
        }

        [Test]
        public void ApplicationVectorY_ValueChanged_ExpectApplicationVectorUpdated()
        {
            //Arrange
            var (nodeViewModel, modelForce) = BuildTestForce();
            var forceViewModel = new ForceViewModel(modelForce, nodeViewModel);

            //Act
            forceViewModel.ApplicationVectorY = 3124;

            //Assert
            Assert.That(forceViewModel.ApplicationVector.Y, Is.EqualTo(3124));
        }

        [Test]
        public void ApplicationVectorY_ValueChanged_ExpectMagnitudeUpdated()
        {
            //Arrange
            var (nodeViewModel, modelForce) = BuildTestForce();
            var forceViewModel = new ForceViewModel(modelForce, nodeViewModel);

            //Act
            forceViewModel.ApplicationVectorY = 3124;

            //Assert
            Assert.That(forceViewModel.Magnitude, Is.EqualTo(3124));
        }

        [Test]
        public void ApplicationVectorY_ValueChanged_ExpectNameUpdated()
        {
            //Arrange
            var (nodeViewModel, modelForce) = BuildTestForce();
            var forceViewModel = new ForceViewModel(modelForce, nodeViewModel);

            //Act
            forceViewModel.ApplicationVectorY = 3124;

            //Assert
            Assert.That(forceViewModel.Name, Is.EqualTo("(1) ---> (0,00/3.124,00/0,00) (3.124,00)"));
        }

        [Test]
        public void ApplicationVectorY_ValueChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var (nodeViewModel, modelForce) = BuildTestForce();
            var forceViewModel = new ForceViewModel(modelForce, nodeViewModel);

            var eventChecker = new PropertyChangedEventChecker(forceViewModel);

            //Act
            forceViewModel.ApplicationVectorY = 3124;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "ApplicationVectorY",
                "ApplicationVector",
                "Magnitude",
                "Name"
            }));
        }

        [Test]
        public void ApplicationVectorZ_ValueChanged_ExpectModelUpdated()
        {
            //Arrange
            var (nodeViewModel, modelForce) = BuildTestForce();
            var forceViewModel = new ForceViewModel(modelForce, nodeViewModel);

            //Act
            forceViewModel.ApplicationVectorZ = 3124;

            //Assert
            Assert.That(modelForce.ApplicationVector.Z, Is.EqualTo(3124));
            Assert.That(modelForce.Magnitude, Is.EqualTo(3124));
        }

        [Test]
        public void ApplicationVectorZ_ValueChanged_ExpectApplicationVectorUpdated()
        {
            //Arrange
            var (nodeViewModel, modelForce) = BuildTestForce();
            var forceViewModel = new ForceViewModel(modelForce, nodeViewModel);

            //Act
            forceViewModel.ApplicationVectorZ = 3124;

            //Assert
            Assert.That(forceViewModel.ApplicationVector.Z, Is.EqualTo(3124));
        }

        [Test]
        public void ApplicationVectorZ_ValueChanged_ExpectMagnitudeUpdated()
        {
            //Arrange
            var (nodeViewModel, modelForce) = BuildTestForce();
            var forceViewModel = new ForceViewModel(modelForce, nodeViewModel);

            //Act
            forceViewModel.ApplicationVectorZ = 3124;

            //Assert
            Assert.That(forceViewModel.Magnitude, Is.EqualTo(3124));
        }

        [Test]
        public void ApplicationVectorZ_ValueChanged_ExpectNameUpdated()
        {
            //Arrange
            var (nodeViewModel, modelForce) = BuildTestForce();
            var forceViewModel = new ForceViewModel(modelForce, nodeViewModel);

            //Act
            forceViewModel.ApplicationVectorZ = 3124;

            //Assert
            Assert.That(forceViewModel.Name, Is.EqualTo("(1) ---> (0,00/0,00/3.124,00) (3.124,00)"));
        }

        [Test]
        public void ApplicationVectorZ_ValueChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var (nodeViewModel, modelForce) = BuildTestForce();
            var forceViewModel = new ForceViewModel(modelForce, nodeViewModel);

            var eventChecker = new PropertyChangedEventChecker(forceViewModel);

            //Act
            forceViewModel.ApplicationVectorZ = 3124;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "ApplicationVectorZ",
                "ApplicationVector",
                "Magnitude",
                "Name"
            }));
        }

        private static (NodeViewModel, IModelForce) BuildTestForce()
        {
            var modelProblem = new ModelProblem("test");

            var node = modelProblem.AddNode();

            var force = modelProblem.AddForce(node);

            var nodeViewModel = new NodeViewModel(node);

            return (nodeViewModel, force);
        }
    }
}
