using NUnit.Framework;
using POC3D.Model;
using POC3D.ViewModel.Implementation;
using POC3D.ViewModel.Tests.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC3D.ViewModel.Tests
{
    [TestFixture]
    public class MaterialViewModelTests
    {
        [Test]
        public void Ctor_ExpectPropertiesCorrectlyInitialized()
        {
            //Arrange
            var modelMaterial = BuildModelMaterial();

            //Act
            var materialViewModel = new MaterialViewModel(modelMaterial);

            //Assert
            Assert.That(materialViewModel.ModelMaterial, Is.EqualTo(modelMaterial));
        }

        [Test]
        public void Name_NameChanged_ExpectModelUpdated()
        {
            //Arrange
            var modelMaterial = BuildModelMaterial();
            var materialViewModel = new MaterialViewModel(modelMaterial);

            //Act
            materialViewModel.Name = "name";

            //Assert
            Assert.That(modelMaterial.Name, Is.EqualTo("name"));
        }

        [Test]
        public void Name_NameChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var modelMaterial = BuildModelMaterial();
            var materialViewModel = new MaterialViewModel(modelMaterial);

            var eventChecker = new PropertyChangedEventChecker(materialViewModel);

            //Act
            materialViewModel.Name = "name";

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "Name"
            }));
        }

        [Test]
        public void YoungsModulus_YoungsModulusChanged_ExpectModelUpdated()
        {
            //Arrange
            var modelMaterial = BuildModelMaterial();
            var materialViewModel = new MaterialViewModel(modelMaterial);

            //Act
            materialViewModel.YoungsModulus = 132;

            //Assert
            Assert.That(modelMaterial.YoungsModulus, Is.EqualTo(132));
        }

        [Test]
        public void YoungsModulus_YoungsModulusChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var modelMaterial = BuildModelMaterial();
            var materialViewModel = new MaterialViewModel(modelMaterial);

            var eventChecker = new PropertyChangedEventChecker(materialViewModel);

            //Act
            materialViewModel.YoungsModulus = 123;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "YoungsModulus"
            }));
        }

        private static IModelMaterial BuildModelMaterial()
        {
            var modelProblem = new ModelProblem("test");

            return modelProblem.AddMaterial();
        }
    }
}
