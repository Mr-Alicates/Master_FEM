using NUnit.Framework;
using POC3D.Model;
using POC3D.Model.Serialization;
using POC3D.Model.Tests.Helper;
using POC3D.ViewModel.Implementation;
using POC3D.ViewModel.Tests.Helper;

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

            //Act
            problemViewModel.SaveProblemCommand.Execute(null);
            problemViewModel.LoadProblemCommand.Execute(null);

            //Assert
            Assert.That(fileSystemMock.ExistingFile, Is.Not.Null);
            Assert.That(fileSystemMock.ExistingFileContent, Is.Not.Null);
            Assert.That(fileSystemMock.CreatedDirectory, Is.Not.Null);
            Assert.That(problemViewModel.Name, Is.EqualTo("IntegrationTest"));
        }
    }
}
