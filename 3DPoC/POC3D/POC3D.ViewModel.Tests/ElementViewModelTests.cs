using NUnit.Framework;
using POC3D.Model;
using POC3D.ViewModel.Tests.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC3D.ViewModel.Tests
{
    [TestFixture]
    public class ElementViewModelTests
    {
        [Test]
        public void Ctor_ExpectPropertiesCorrectlyInitialized()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, materialViewModel, modelElement) = BuildTestElement();

            //Act
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            //Assert
            Assert.That(elementViewModel.Element, Is.EqualTo(modelElement));
            Assert.That(elementViewModel.Origin, Is.EqualTo(originNodeViewModel));
            Assert.That(elementViewModel.Destination, Is.EqualTo(destinationNodeViewModel));
            Assert.That(elementViewModel.Id, Is.EqualTo(modelElement.Id));
            Assert.That(elementViewModel.Description, Is.EqualTo(modelElement.Description));
            Assert.That(elementViewModel.Material, Is.EqualTo(materialViewModel));
            Assert.That(elementViewModel.Geometry, Is.Not.Null);
            Assert.That(elementViewModel.ResultGeometry, Is.Not.Null);
            Assert.That(elementViewModel.ElementCalculationViewModel, Is.Not.Null);
        }

        [Test]
        public void Origin_XPropertyOfOriginNodeChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, materialViewModel, modelElement) = BuildTestElement();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            var eventChecker = new PropertyChangedEventChecker(elementViewModel);

            //Act
            originNodeViewModel.X = 100;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "Origin"
            }));
        }

        [Test]
        public void Origin_YPropertyOfOriginNodeChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, materialViewModel, modelElement) = BuildTestElement();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            var eventChecker = new PropertyChangedEventChecker(elementViewModel);

            //Act
            originNodeViewModel.Y = 100;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "Origin"
            }));
        }

        [Test]
        public void Origin_ZPropertyOfOriginNodeChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, materialViewModel, modelElement) = BuildTestElement();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            var eventChecker = new PropertyChangedEventChecker(elementViewModel);

            //Act
            originNodeViewModel.Z = 100;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "Origin"
            }));
        }

        [Test]
        public void Origin_IsFixedPropertyOfOriginNodeChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, materialViewModel, modelElement) = BuildTestElement();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            var eventChecker = new PropertyChangedEventChecker(elementViewModel);

            //Act
            originNodeViewModel.IsFixed = true;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "Origin"
            }));
        }

        [Test]
        public void Origin_NonRelevantPropertyOfNodeChanged_ExpectPropertiesChangedEventNotRaised()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, materialViewModel, modelElement) = BuildTestElement();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            var eventChecker = new PropertyChangedEventChecker(elementViewModel);

            //Act
            originNodeViewModel.IsSelected = true;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.Empty);
        }

        [Test]
        public void Origin_SetNull_ExpectArgumentNullExceptionThrown()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, materialViewModel, modelElement) = BuildTestElement();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => elementViewModel.Origin = null);
        }

        [Test]
        public void Origin_NodeChanged_ExpectModelUpdated()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, additionalNodeViewModel, materialViewModel, modelElement) = BuildTestElementWithAdditionalNode();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            //Act
            elementViewModel.Origin = additionalNodeViewModel;

            //Assert
            Assert.That(modelElement.OriginNode, Is.EqualTo(additionalNodeViewModel.Node));
        }

        [Test]
        public void Origin_NodeChanged_ExpectDescriptionChanged()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, additionalNodeViewModel, materialViewModel, modelElement) = BuildTestElementWithAdditionalNode();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            //Act
            elementViewModel.Origin = additionalNodeViewModel;

            //Assert
            Assert.That(modelElement.Description, Is.EqualTo("(3) ---> (2)"));
        }

        [Test]
        public void Origin_NodeChanged_ExpectLengthChanged()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, additionalNodeViewModel, materialViewModel, modelElement) = BuildTestElementWithAdditionalNode();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            additionalNodeViewModel.X = 100;

            //Act
            elementViewModel.Origin = additionalNodeViewModel;

            //Assert
            Assert.That(modelElement.Length, Is.EqualTo(100));
        }

        [Test]
        public void Origin_NodeChanged_ExpectKChanged()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, additionalNodeViewModel, materialViewModel, modelElement) = BuildTestElementWithAdditionalNode();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            elementViewModel.CrossSectionArea = 1;
            materialViewModel.YoungsModulus = 100000;

            additionalNodeViewModel.X = 100;

            //Act
            elementViewModel.Origin = additionalNodeViewModel;

            //Assert
            Assert.That(modelElement.K, Is.EqualTo(1000));
        }

        [Test]
        public void Origin_NodeChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, additionalNodeViewModel, materialViewModel, modelElement) = BuildTestElementWithAdditionalNode();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            var eventChecker = new PropertyChangedEventChecker(elementViewModel);

            //Act
            elementViewModel.Origin = additionalNodeViewModel;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "Origin",
                "Length",
                "K",
                "Description",
                "Geometry",
                "ResultGeometry"
            }));
        }

        [Test]
        public void Origin_NodeChangedAndRelevantPropertyOfNewNodeChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, additionalNodeViewModel, materialViewModel, modelElement) = BuildTestElementWithAdditionalNode();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            elementViewModel.Origin = additionalNodeViewModel;
            var eventChecker = new PropertyChangedEventChecker(elementViewModel);

            //Act
            additionalNodeViewModel.IsFixed = true;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "Origin"
            }));
        }

        [Test]
        public void Origin_NodeChangedAndRelevantPropertyOfOldNodeChanged_ExpectPropertiesChangedEventNotRaised()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, additionalNodeViewModel, materialViewModel, modelElement) = BuildTestElementWithAdditionalNode();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            elementViewModel.Origin = additionalNodeViewModel;
            var eventChecker = new PropertyChangedEventChecker(elementViewModel);

            //Act
            originNodeViewModel.IsFixed = true;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.Empty);
        }

        [Test]
        public void Destination_XPropertyOfDestinationNodeChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, materialViewModel, modelElement) = BuildTestElement();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            var eventChecker = new PropertyChangedEventChecker(elementViewModel);

            //Act
            destinationNodeViewModel.X = 100;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "Destination"
            }));
        }

        [Test]
        public void Destination_YPropertyOfDestinationNodeChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, materialViewModel, modelElement) = BuildTestElement();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            var eventChecker = new PropertyChangedEventChecker(elementViewModel);

            //Act
            destinationNodeViewModel.Y = 100;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "Destination"
            }));
        }

        [Test]
        public void Destination_ZPropertyOfDestinationNodeChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, materialViewModel, modelElement) = BuildTestElement();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            var eventChecker = new PropertyChangedEventChecker(elementViewModel);

            //Act
            destinationNodeViewModel.Z = 100;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "Destination"
            }));
        }

        [Test]
        public void Destination_IsFixedPropertyOfDestinationNodeChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, materialViewModel, modelElement) = BuildTestElement();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            var eventChecker = new PropertyChangedEventChecker(elementViewModel);

            //Act
            destinationNodeViewModel.IsFixed = true;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "Destination"
            }));
        }

        [Test]
        public void Destination_NonRelevantPropertyOfNodeChanged_ExpectPropertiesChangedEventNotRaised()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, materialViewModel, modelElement) = BuildTestElement();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            var eventChecker = new PropertyChangedEventChecker(elementViewModel);

            //Act
            originNodeViewModel.IsSelected = true;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.Empty);
        }

        [Test]
        public void Destination_SetNull_ExpectArgumentNullExceptionThrown()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, materialViewModel, modelElement) = BuildTestElement();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => elementViewModel.Destination = null);
        }

        [Test]
        public void Destination_NodeChanged_ExpectModelUpdated()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, additionalNodeViewModel, materialViewModel, modelElement) = BuildTestElementWithAdditionalNode();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            //Act
            elementViewModel.Destination = additionalNodeViewModel;

            //Assert
            Assert.That(modelElement.DestinationNode, Is.EqualTo(additionalNodeViewModel.Node));
        }

        [Test]
        public void Destination_NodeChanged_ExpectDescriptionChanged()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, additionalNodeViewModel, materialViewModel, modelElement) = BuildTestElementWithAdditionalNode();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            //Act
            elementViewModel.Destination = additionalNodeViewModel;

            //Assert
            Assert.That(modelElement.Description, Is.EqualTo("(1) ---> (3)"));
        }

        [Test]
        public void Destination_NodeChanged_ExpectLengthChanged()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, additionalNodeViewModel, materialViewModel, modelElement) = BuildTestElementWithAdditionalNode();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            additionalNodeViewModel.X = 100;

            //Act
            elementViewModel.Destination = additionalNodeViewModel;

            //Assert
            Assert.That(modelElement.Length, Is.EqualTo(100));
        }

        [Test]
        public void Destination_NodeChanged_ExpectKChanged()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, additionalNodeViewModel, materialViewModel, modelElement) = BuildTestElementWithAdditionalNode();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            elementViewModel.CrossSectionArea = 1;
            materialViewModel.YoungsModulus = 100000;

            additionalNodeViewModel.X = 100;

            //Act
            elementViewModel.Destination = additionalNodeViewModel;

            //Assert
            Assert.That(modelElement.K, Is.EqualTo(1000));
        }

        [Test]
        public void Destination_NodeChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, additionalNodeViewModel, materialViewModel, modelElement) = BuildTestElementWithAdditionalNode();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            var eventChecker = new PropertyChangedEventChecker(elementViewModel);

            //Act
            elementViewModel.Destination = additionalNodeViewModel;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "Destination",
                "Length",
                "K",
                "Description",
                "Geometry",
                "ResultGeometry"
            }));
        }

        [Test]
        public void Destination_NodeChangedAndRelevantPropertyOfNewNodeChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, additionalNodeViewModel, materialViewModel, modelElement) = BuildTestElementWithAdditionalNode();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            elementViewModel.Destination = additionalNodeViewModel;
            var eventChecker = new PropertyChangedEventChecker(elementViewModel);

            //Act
            additionalNodeViewModel.IsFixed = true;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "Destination"
            }));
        }

        [Test]
        public void Destination_NodeChangedAndRelevantPropertyOfOldNodeChanged_ExpectPropertiesChangedEventNotRaised()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, additionalNodeViewModel, materialViewModel, modelElement) = BuildTestElementWithAdditionalNode();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            elementViewModel.Destination = additionalNodeViewModel;
            var eventChecker = new PropertyChangedEventChecker(elementViewModel);

            //Act
            destinationNodeViewModel.IsFixed = true;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.Empty);
        }

        [Test]
        public void CrossSectionArea_ValueChanged_ExpectKChanged()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, materialViewModel, modelElement) = BuildTestElement();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            materialViewModel.YoungsModulus = 1000;

            destinationNodeViewModel.X = 100;

            //Act
            elementViewModel.CrossSectionArea = 1000;

            //Assert
            Assert.That(modelElement.K, Is.EqualTo(10000));
        }

        [Test]
        public void CrossSectionArea_ValueChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, materialViewModel, modelElement) = BuildTestElement();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            materialViewModel.YoungsModulus = 1000;

            destinationNodeViewModel.X = 100;

            var eventChecker = new PropertyChangedEventChecker(elementViewModel);

            //Act
            elementViewModel.CrossSectionArea = 1000;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "CrossSectionArea",
                "K"
            }));
        }

        [Test]
        public void Material_YoungsModulusPropertyOfMaterialChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, materialViewModel, modelElement) = BuildTestElement();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            var eventChecker = new PropertyChangedEventChecker(elementViewModel);

            //Act
            materialViewModel.YoungsModulus = 100;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "Material",
                "K"
            }));
        }

        [Test]
        public void Material_NonRelevantPropertyOfMaterialChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, materialViewModel, modelElement) = BuildTestElement();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            var eventChecker = new PropertyChangedEventChecker(elementViewModel);

            //Act
            materialViewModel.Name = "asdf";

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.Empty);
        }

        [Test]
        public void Material_SetNull_ExpectArgumentNullExceptionThrown()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, materialViewModel, modelElement) = BuildTestElement();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => elementViewModel.Material = null);
        }

        [Test]
        public void Material_MaterialChanged_ExpectModelUpdated()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, materialViewModel, additionalMaterialViewModel, modelElement) = BuildTestElementWithAdditionalMaterial();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            //Act
            elementViewModel.Material = additionalMaterialViewModel;

            //Assert
            Assert.That(modelElement.Material, Is.EqualTo(additionalMaterialViewModel.ModelMaterial));
        }

        [Test]
        public void Material_MaterialChanged_ExpectKUpdated()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, materialViewModel, additionalMaterialViewModel, modelElement) = BuildTestElementWithAdditionalMaterial();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            materialViewModel.YoungsModulus = 1;
            additionalMaterialViewModel.YoungsModulus = 100;
            elementViewModel.CrossSectionArea = 100;
            destinationNodeViewModel.X = 100;

            //Act
            elementViewModel.Material = additionalMaterialViewModel;

            //Assert
            Assert.That(modelElement.K, Is.EqualTo(100));
        }

        [Test]
        public void Material_MaterialChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, materialViewModel, additionalMaterialViewModel, modelElement) = BuildTestElementWithAdditionalMaterial();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);
            
            var eventChecker = new PropertyChangedEventChecker(elementViewModel);

            //Act
            elementViewModel.Material = additionalMaterialViewModel;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "Material",
                "K"
            }));
        }

        [Test]
        public void Material_MaterialChangedAndRelevantPropertyOfNewMaterialChanged_ExpectPropertiesChangedEventRaised()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, materialViewModel, additionalMaterialViewModel, modelElement) = BuildTestElementWithAdditionalMaterial();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            elementViewModel.Material = additionalMaterialViewModel;

            var eventChecker = new PropertyChangedEventChecker(elementViewModel);

            //Act
            additionalMaterialViewModel.YoungsModulus = 100;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.EqualTo(new[]
            {
                "Material",
                "K"
            }));
        }

        [Test]
        public void Material_MaterialChangedAndRelevantPropertyOfOldMaterialChanged_ExpectPropertiesChangedEventNotRaised()
        {
            //Arrange
            var (originNodeViewModel, destinationNodeViewModel, materialViewModel, additionalMaterialViewModel, modelElement) = BuildTestElementWithAdditionalMaterial();
            var elementViewModel = new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel);

            elementViewModel.Material = additionalMaterialViewModel;

            var eventChecker = new PropertyChangedEventChecker(elementViewModel);

            //Act
            materialViewModel.YoungsModulus = 100;

            //Assert
            Assert.That(eventChecker.PropertiesRaised, Is.Empty);
        }

        public (NodeViewModel, NodeViewModel, MaterialViewModel, MaterialViewModel, IModelElement) BuildTestElementWithAdditionalMaterial()
        {
            var modelProblem = new ModelProblem("test");

            var node1 = modelProblem.AddNode();
            var node2 = modelProblem.AddNode();

            var element = modelProblem.AddElement(node1, node2);

            var nodeViewModel1 = new NodeViewModel(node1);
            var nodeViewModel2 = new NodeViewModel(node2);

            var otherMaterial = modelProblem.AddMaterial();

            var materialViewModel1 = new MaterialViewModel(element.Material);
            var materialViewModel2 = new MaterialViewModel(otherMaterial);

            return (nodeViewModel1, nodeViewModel2, materialViewModel1, materialViewModel2, element);
        }

        public (NodeViewModel, NodeViewModel, NodeViewModel, MaterialViewModel, IModelElement) BuildTestElementWithAdditionalNode()
        {
            var modelProblem = new ModelProblem("test");

            var node1 = modelProblem.AddNode();
            var node2 = modelProblem.AddNode();
            var node3 = modelProblem.AddNode();

            var element = modelProblem.AddElement(node1, node2);

            var nodeViewModel1 = new NodeViewModel(node1);
            var nodeViewModel2 = new NodeViewModel(node2);
            var nodeViewModel3 = new NodeViewModel(node3);

            var materialViewModel = new MaterialViewModel(element.Material);

            return (nodeViewModel1, nodeViewModel2, nodeViewModel3, materialViewModel, element);
        }

        public (NodeViewModel, NodeViewModel, MaterialViewModel, IModelElement) BuildTestElement()
        {
            var modelProblem = new ModelProblem("test");

            var node1 = modelProblem.AddNode();
            var node2 = modelProblem.AddNode();

            var element = modelProblem.AddElement(node1, node2);

            var nodeViewModel1 = new NodeViewModel(node1);
            var nodeViewModel2 = new NodeViewModel(node2);

            var materialViewModel = new MaterialViewModel(element.Material);

            return (nodeViewModel1, nodeViewModel2, materialViewModel, element);
        }
    }
}
