using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC3D.Model.Tests
{
    [TestFixture]
    public class ModelProblemValidationTests
    {
        [Test]
        public void ValidateForces_MakeTwoForcesBeAppliedOnSameNode_ExpectInvalidOperationException()
        {
            //Arrange
            ModelProblem problem = new ModelProblem("problem");

            var node1 = problem.AddNode();
            var node2 = problem.AddNode();

            var force1 = problem.AddForce(node1);
            var force2 = problem.AddForce(node2);

            //Act
            //Assert
            Assert.Throws<InvalidOperationException>(() => force2.Node = node1);
        }

        [Test]
        public void ValidateForces_SetStrayNodeAsApplicationNode_ExpectInvalidOperationException()
        {
            //Arrange
            ModelProblem problem = new ModelProblem("problem");

            var node1 = problem.AddNode();
            var force = problem.AddForce(node1);

            var otherProblem = new ModelProblem("otherProblem");
            var strayNode = otherProblem.AddNode();

            //Act
            //Assert
            Assert.Throws<InvalidOperationException>(() => force.Node = strayNode);
        }

        [Test]
        public void ValidateElements_MakeElementHaveSameNodeAsOriginAndDestination_ExpectInvalidOperationException()
        {
            //Arrange
            ModelProblem problem = new ModelProblem("problem");

            var node1 = problem.AddNode();
            var node2 = problem.AddNode();

            var element = problem.AddElement(node1, node2);

            //Act
            //Assert
            Assert.Throws<InvalidOperationException>(() => element.DestinationNode = node1);
        }

        [Test]
        public void ValidateElements_MakeElementHaveSameNodeAsDestinationAndOrigin_ExpectInvalidOperationException()
        {
            //Arrange
            ModelProblem problem = new ModelProblem("problem");

            var node1 = problem.AddNode();
            var node2 = problem.AddNode();

            var element = problem.AddElement(node1, node2);

            //Act
            //Assert
            Assert.Throws<InvalidOperationException>(() => element.OriginNode = node2);
        }

        [Test]
        public void ValidateElements_MakeElementHaveSameNodesAsOtherElementBySettingOriginNode_ExpectInvalidOperationException()
        {
            //Arrange
            ModelProblem problem = new ModelProblem("problem");

            var node1 = problem.AddNode();
            var node2 = problem.AddNode();
            var node3 = problem.AddNode();

            var element1 = problem.AddElement(node1, node2);
            var element2 = problem.AddElement(node3, node2);

            //Act
            //Assert
            Assert.Throws<InvalidOperationException>(() => element2.OriginNode = node1);
        }

        [Test]
        public void ValidateElements_MakeElementHaveSameNodesAsOtherElementBySettingDestinationNode_ExpectInvalidOperationException()
        {
            //Arrange
            ModelProblem problem = new ModelProblem("problem");

            var node1 = problem.AddNode();
            var node2 = problem.AddNode();
            var node3 = problem.AddNode();

            var element1 = problem.AddElement(node2, node1);
            var element2 = problem.AddElement(node2, node3);

            //Act
            //Assert
            Assert.Throws<InvalidOperationException>(() => element2.DestinationNode = node1);
        }

        [Test]
        public void ValidateElement_SetStrayNodeAsOriginNode_ExpectInvalidOperationException()
        {
            //Arrange
            ModelProblem problem = new ModelProblem("problem");

            var node1 = problem.AddNode();
            var node2 = problem.AddNode();
            var element = problem.AddElement(node1, node2);

            var otherProblem = new ModelProblem("otherProblem");
            var strayNode = otherProblem.AddNode();

            //Act
            //Assert
            Assert.Throws<InvalidOperationException>(() => element.OriginNode = strayNode);
        }

        [Test]
        public void ValidateElement_SetStrayNodeAsDestinationNode_ExpectInvalidOperationException()
        {
            //Arrange
            ModelProblem problem = new ModelProblem("problem");

            var node1 = problem.AddNode();
            var node2 = problem.AddNode();
            var element = problem.AddElement(node1, node2);

            var otherProblem = new ModelProblem("otherProblem");
            var strayNode = otherProblem.AddNode();

            //Act
            //Assert
            Assert.Throws<InvalidOperationException>(() => element.DestinationNode = strayNode);
        }

        [Test]
        public void ValidateElement_SetStraMaterial_ExpectInvalidOperationException()
        {
            //Arrange
            ModelProblem problem = new ModelProblem("problem");

            var node1 = problem.AddNode();
            var node2 = problem.AddNode();
            var element = problem.AddElement(node1, node2);

            var otherProblem = new ModelProblem("otherProblem");
            var material = otherProblem.AddMaterial();

            //Act
            //Assert
            Assert.Throws<InvalidOperationException>(() => element.Material = material);
        }
    }
}
