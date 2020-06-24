using NUnit.Framework;
using POC3D.Model;
using POC3D.Model.Serialization;
using POC3D.Model.Tests.Helper;
using POC3D.ViewModel.Implementation;
using POC3D.ViewModel.Tests.Helper;
using System.Linq;

namespace POC3D.ViewModel.Tests
{
    [TestFixture]
    public class ProblemViewModelIntegrationTests
    {
        [Test]
        public void ProblemViewModel_SaveAndLoadIntegrationTest()
        {
            //Arrange
            var fileSystemMock = new FakeFileSystem();
            var problemSerializer = new ProblemSerializer(fileSystemMock);
            var dialogService = new FakeDialogService()
            {
                FilePath = "c:\\Test\\IntegrationTest.3DPoC"
            };

            var problemViewModel = new ProblemViewModel(new ModelProblem("OLD"), problemSerializer, dialogService);
            SetupProblemViewModel(problemViewModel);

            //Act
            problemViewModel.SaveProblemCommand.Execute(null);
            problemViewModel.LoadProblemCommand.Execute(null);

            //Assert
            Assert.That(fileSystemMock.ExistingFile, Is.Not.Null);
            Assert.That(fileSystemMock.ExistingFileContent, Is.Not.Null);
            Assert.That(fileSystemMock.CreatedDirectory, Is.Not.Null);
            AssertProblemViewModel(problemViewModel);
        }

        private void SetupProblemViewModel(ProblemViewModel problemViewModel)
        {
            var node1 = problemViewModel.AddNode();
            var node2 = problemViewModel.AddNode();
            var node3 = problemViewModel.AddNode();
            var node4 = problemViewModel.AddNode();

            #region Node properties

            node2.IsXFixed = true;
            node3.IsYFixed = true;
            node4.IsZFixed = true;

            node1.X = 1;
            node2.X = 2;
            node3.X = 3;
            node4.X = 4;

            node1.Y = 11;
            node2.Y = 12;
            node3.Y = 13;
            node4.Y = 14;

            node1.Z = 21;
            node2.Z = 22;
            node3.Z = 23;
            node4.Z = 24;

            #endregion

            var element1 = problemViewModel.AddBarElement(node1, node2);
            var element2 = problemViewModel.AddBarElement(node2, node3);
            var element3 = problemViewModel.AddBarElement(node3, node4);

            var force = problemViewModel.AddForce(node1);

            force.ApplicationVectorX = 100;
            force.ApplicationVectorY = 200;
            force.ApplicationVectorZ = 300;

            var material1 = element1.Material;
            var material2 = problemViewModel.AddMaterial();
            var material3 = problemViewModel.AddMaterial();
            var material4 = problemViewModel.AddMaterial();

            #region Material properties

            material1.Name = "Aluminum1";
            material2.Name = "Aluminum2";
            material3.Name = "Aluminum3";
            material4.Name = "Aluminum4";

            material1.YoungsModulus = 1;
            material2.YoungsModulus = 12;
            material3.YoungsModulus = 123;
            material4.YoungsModulus = 1234;

            #endregion

            element1.Material = material2;
            element2.Material = material3;
            element3.Material = material4;

            #region Element properties

            element1.CrossSectionArea = 101;
            element2.CrossSectionArea = 202;
            element3.CrossSectionArea = 303;

            #endregion
        }

        private void AssertProblemViewModel(ProblemViewModel problemViewModel)
        {
            Assert.That(problemViewModel.Nodes.Count, Is.EqualTo(4));
            Assert.That(problemViewModel.Forces.Count, Is.EqualTo(1));
            Assert.That(problemViewModel.Elements.Count, Is.EqualTo(3));
            Assert.That(problemViewModel.Materials.Count, Is.EqualTo(4));

            var node1 = problemViewModel.Nodes.First(x => x.Id == 1);
            var node2 = problemViewModel.Nodes.First(x => x.Id == 2);
            var node3 = problemViewModel.Nodes.First(x => x.Id == 3);
            var node4 = problemViewModel.Nodes.First(x => x.Id == 4);

            #region NodeProperties

            Assert.That(node1.IsXFixed, Is.False);
            Assert.That(node1.IsYFixed, Is.False);
            Assert.That(node1.IsZFixed, Is.False);

            Assert.That(node2.IsXFixed, Is.True);
            Assert.That(node2.IsYFixed, Is.False);
            Assert.That(node2.IsZFixed, Is.False);

            Assert.That(node3.IsXFixed, Is.False);
            Assert.That(node3.IsYFixed, Is.True);
            Assert.That(node3.IsZFixed, Is.False);

            Assert.That(node4.IsXFixed, Is.False);
            Assert.That(node4.IsYFixed, Is.False);
            Assert.That(node4.IsZFixed, Is.True);

            Assert.That(node1.X, Is.EqualTo(1));
            Assert.That(node2.X, Is.EqualTo(2));
            Assert.That(node3.X, Is.EqualTo(3));
            Assert.That(node4.X, Is.EqualTo(4));

            Assert.That(node1.Y, Is.EqualTo(11));
            Assert.That(node2.Y, Is.EqualTo(12));
            Assert.That(node3.Y, Is.EqualTo(13));
            Assert.That(node4.Y, Is.EqualTo(14));

            Assert.That(node1.Z, Is.EqualTo(21));
            Assert.That(node2.Z, Is.EqualTo(22));
            Assert.That(node3.Z, Is.EqualTo(23));
            Assert.That(node4.Z, Is.EqualTo(24));

            #endregion

            var material1 = problemViewModel.Materials.First(x => x.Id == 1);
            var material2 = problemViewModel.Materials.First(x => x.Id == 2);
            var material3 = problemViewModel.Materials.First(x => x.Id == 3);
            var material4 = problemViewModel.Materials.First(x => x.Id == 4);

            #region MaterialProperties

            Assert.That(material1.Name, Is.EqualTo("Aluminum1"));
            Assert.That(material2.Name, Is.EqualTo("Aluminum2"));
            Assert.That(material3.Name, Is.EqualTo("Aluminum3"));
            Assert.That(material4.Name, Is.EqualTo("Aluminum4"));

            Assert.That(material1.YoungsModulus, Is.EqualTo(1));
            Assert.That(material2.YoungsModulus, Is.EqualTo(12));
            Assert.That(material3.YoungsModulus, Is.EqualTo(123));
            Assert.That(material4.YoungsModulus, Is.EqualTo(1234));

            #endregion

            var force = problemViewModel.Forces.First(x => x.Id == 1);

            #region ForceProperties

            Assert.That(force.ApplicationVectorX, Is.EqualTo(100));
            Assert.That(force.ApplicationVectorY, Is.EqualTo(200));
            Assert.That(force.ApplicationVectorZ, Is.EqualTo(300));
            Assert.That(force.Node, Is.EqualTo(node1));

            #endregion

            var element1 = problemViewModel.Elements.First(x => x.Id == 1);
            var element2 = problemViewModel.Elements.First(x => x.Id == 2);
            var element3 = problemViewModel.Elements.First(x => x.Id == 3);

            #region ElementProperties

            Assert.That(element1.CrossSectionArea, Is.EqualTo(101));
            Assert.That(element2.CrossSectionArea, Is.EqualTo(202));
            Assert.That(element3.CrossSectionArea, Is.EqualTo(303));

            Assert.That(element1.Origin, Is.EqualTo(node1));
            Assert.That(element2.Origin, Is.EqualTo(node2));
            Assert.That(element3.Origin, Is.EqualTo(node3));

            Assert.That(element1.Destination, Is.EqualTo(node2));
            Assert.That(element2.Destination, Is.EqualTo(node3));
            Assert.That(element3.Destination, Is.EqualTo(node4));

            Assert.That(element1.Material, Is.EqualTo(material2));
            Assert.That(element2.Material, Is.EqualTo(material3));
            Assert.That(element3.Material, Is.EqualTo(material4));

            #endregion
        }
    }
}
