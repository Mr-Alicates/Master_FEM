using NUnit.Framework;
using POC3D.Model.Serialization;
using POC3D.Model.Tests.Helper;
using System;

namespace POC3D.Model.Tests.Serialization
{
    [TestFixture]
    public class ProblemSerializerTests
    {
        [Test]
        public void SerializeProblem_NullModelProblem_ThrowsArgumentNullException()
        {
            //Arrange
            var filesystem = new FakeFileSystem();
            var problemSerializer = new ProblemSerializer(filesystem);

            var filePath = "c:\\File.3dPoC";

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => problemSerializer.SerializeProblem(null, filePath));
        }

        [Test]
        public void SerializeProblem_NullFilePath_ThrowsArgumentException()
        {
            //Arrange
            var filesystem = new FakeFileSystem();
            var problemSerializer = new ProblemSerializer(filesystem);

            var modelProblem = new ModelProblem("test");

            //Act
            //Assert
            Assert.Throws<ArgumentException>(() => problemSerializer.SerializeProblem(modelProblem, null));
        }

        [Test]
        public void SerializeProblem_FileAlreadyExists_ExpectFileDeleted()
        {
            //Arrange
            var filesystem = new FakeFileSystem();
            var problemSerializer = new ProblemSerializer(filesystem);

            var modelProblem = new ModelProblem("test");
            var filePath = "c:\\File.3dPoC";

            filesystem.ExistingFile = filePath;

            //Act
            problemSerializer.SerializeProblem(modelProblem, filePath);

            //Assert
            Assert.That(filesystem.DeletedFile, Is.EqualTo(filePath));
        }

        [Test]
        public void SerializeProblem_ParentDirectoryDoesNotExist_ExpectDirectoryCreated()
        {
            //Arrange
            var filesystem = new FakeFileSystem();
            var problemSerializer = new ProblemSerializer(filesystem);

            var modelProblem = new ModelProblem("test");
            var directoryPath = "c:\\directory";
            var filePath = $"{directoryPath}\\File.3dPoC";

            //Act
            problemSerializer.SerializeProblem(modelProblem, filePath);

            //Assert
            Assert.That(filesystem.CreatedDirectory, Is.EqualTo(directoryPath));
        }

        [Test]
        public void SerializeProblem_FileDoesNotExist_ExpectFileCreated()
        {
            //Arrange
            var filesystem = new FakeFileSystem();
            var problemSerializer = new ProblemSerializer(filesystem);

            var modelProblem = new ModelProblem("test");
            var filePath = "c:\\File.3dPoC";

            //Act
            problemSerializer.SerializeProblem(modelProblem, filePath);

            //Assert
            Assert.That(filesystem.ExistingFile, Is.EqualTo(filePath));
        }

        [Test]
        public void SerializeProblem_FileDoesNotExist_ExpectValidMementoCreated()
        {
            //Arrange
            var filesystem = new FakeFileSystem();
            var problemSerializer = new ProblemSerializer(filesystem);

            var modelProblem = new ModelProblem("test");
            var filePath = "c:\\File.3dPoC";

            //Act
            problemSerializer.SerializeProblem(modelProblem, filePath);

            //Assert
            var memento = filesystem.TryGetProblemMemento();
            Assert.That(memento, Is.Not.Null);
        }

        [Test]
        public void DeserializeProblem_NullFilePath_ThrowsArgumentException()
        {
            //Arrange
            var filesystem = new FakeFileSystem();
            var problemSerializer = new ProblemSerializer(filesystem);

            //Act
            //Assert
            Assert.Throws<ArgumentException>(() => problemSerializer.DeserializeProblem(null));
        }

        [Test]
        public void DeserializeProblem_FileDoesNotExist_ThrowsInvalidOperationException()
        {
            //Arrange
            var filesystem = new FakeFileSystem();
            var problemSerializer = new ProblemSerializer(filesystem);
            var filePath = "c:\\File.3dPoC";

            //Act
            //Assert
            Assert.Throws<InvalidOperationException>(() => problemSerializer.DeserializeProblem(filePath));
        }

        [Test]
        public void DeserializeProblem_FileIsMalformed_ThrowsInvalidOperationException()
        {
            //Arrange
            var filesystem = new FakeFileSystem();
            var problemSerializer = new ProblemSerializer(filesystem);
            var filePath = "c:\\File.3dPoC";

            filesystem.ExistingFile = filePath;
            filesystem.ExistingFileContent = string.Empty;

            //Act
            //Assert
            Assert.Throws<InvalidOperationException>(() => problemSerializer.DeserializeProblem(filePath));
        }

        [Test]
        public void DeserializeProblem_FileIsValid_ExpectFileCorrectlyDeserialized()
        {
            //Arrange
            var filesystem = new FakeFileSystem();
            var problemSerializer = new ProblemSerializer(filesystem);
            var filePath = "c:\\File.3dPoC";
            string fileContent = "{ \"Name\" : \"DeserializeProblem\" }";

            filesystem.ExistingFile = filePath;
            filesystem.ExistingFileContent = fileContent;

            //Act
            var modelProblem = problemSerializer.DeserializeProblem(filePath);

            //Assert
            Assert.That(modelProblem.Name, Is.EqualTo("DeserializeProblem"));
        }
    }
}
