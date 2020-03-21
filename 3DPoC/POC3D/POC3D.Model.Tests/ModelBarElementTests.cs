using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC3D.Model.Tests
{
    [TestFixture]
    public class ModelBarElementTests
    {
        [Test]
        public void Length_OriginNodeModified_ExpectLengthModified()
        {
            //Arrange
            var modelElement = BuildElement();

            //Act
            modelElement.OriginNode.Coordinates.X = 10;

            //Assert
            Assert.That(modelElement.Length, Is.EqualTo(10));
        }

        [Test]
        public void Length_DestinationNodeModified_ExpectLengthModified()
        {
            //Arrange
            var modelElement = BuildElement();

            //Act
            modelElement.DestinationNode.Coordinates.X = 10;

            //Assert
            Assert.That(modelElement.Length, Is.EqualTo(10));
        }

        [Test]
        public void K_OriginNodeModified_ExpectKModified()
        {
            //Arrange
            var modelElement = BuildElement();

            modelElement.Material.YoungsModulus = 100;
            modelElement.CrossSectionArea = 100;

            //Act
            modelElement.OriginNode.Coordinates.X = 10;

            //Assert
            Assert.That(modelElement.K, Is.EqualTo(1000));
        }

        [Test]
        public void K_DestinationNodeModified_ExpectKModified()
        {
            //Arrange
            var modelElement = BuildElement();

            modelElement.Material.YoungsModulus = 100;
            modelElement.CrossSectionArea = 100;

            //Act
            modelElement.DestinationNode.Coordinates.X = 10;

            //Assert
            Assert.That(modelElement.K, Is.EqualTo(1000));
        }

        [Test]
        public void K_CrossSectionAreaModified_ExpectKModified()
        {
            //Arrange
            var modelElement = BuildElement();

            modelElement.Material.YoungsModulus = 100;
            modelElement.DestinationNode.Coordinates.X = 10;

            //Act
            modelElement.CrossSectionArea = 100;

            //Assert
            Assert.That(modelElement.K, Is.EqualTo(1000));
        }

        [Test]
        public void K_MaterialModified_ExpectKModified()
        {
            //Arrange
            var modelElement = BuildElement();

            modelElement.DestinationNode.Coordinates.X = 10;
            modelElement.CrossSectionArea = 100;

            //Act
            modelElement.Material.YoungsModulus = 100;

            //Assert
            Assert.That(modelElement.K, Is.EqualTo(1000));
        }

        [TestCase(1234, 665, 1258, 652.31319554848972)]
        [TestCase(57334, 345, 3, 6593410)]
        [TestCase(23513, 52, 41258, 29.634882931795048)]
        [TestCase(755, 74878, 5345, 10576.780168381665)]
        [TestCase(75, 665, 789, 63.21292775665399)]
        public void K_ValidParameters_ExpectValidK(double youngsModulus, double crossSectionArea, double length, double expectedK)
        {
            //Arrange
            var modelElement = BuildElement();

            modelElement.DestinationNode.Coordinates.X = length;
            modelElement.CrossSectionArea = crossSectionArea;
            modelElement.Material.YoungsModulus = youngsModulus;

            //Act
            var k = modelElement.K;

            //Assert
            Assert.That(k, Is.EqualTo(expectedK));
        }

        private static IModelElement BuildElement()
        {
            var modelProblem = new ModelProblem("test");

            var node1 = modelProblem.AddNode();
            var node2 = modelProblem.AddNode();

            return modelProblem.AddElement(node1, node2);
        }
    }
}
