using NUnit.Framework;
using POC3D.ViewModel.InterfaceManagement.CameraControl;
using POC3D.ViewModel.Tests.Helper;
using System.Windows;
using System.Windows.Media.Media3D;

namespace POC3D.ViewModel.Tests
{
    [TestFixture]
    public class MouseCameraControlViewModelTests
    {
        [Test]
        public void UpdateCamera_NoPressedButtons_ExpectNoMovementOrRotation()
        {
            //Arrange
            var cameraViewModel = new FakeCameraViewModel();

            var testSubject = new MouseCameraControlViewModel(cameraViewModel);

            //Act
            testSubject.UpdateCamera();

            //Assert
            Assert.That(cameraViewModel.LookAtPoint, Is.EqualTo(new Point3D()));
            Assert.That(cameraViewModel.MovePoint, Is.EqualTo(new Point3D()));
            Assert.That(cameraViewModel.MoveVector, Is.EqualTo(new Vector3D()));
            Assert.That(cameraViewModel.RotationX, Is.EqualTo(0));
            Assert.That(cameraViewModel.RotationY, Is.EqualTo(0));
            Assert.That(cameraViewModel.RotationZ, Is.EqualTo(0));
        }

        [Test]
        public void UpdateCamera_MouseWheelMoved_ExpectMovementAndNoRotation()
        {
            //Arrange
            var cameraViewModel = new FakeCameraViewModel();

            var testSubject = new MouseCameraControlViewModel(cameraViewModel);
            testSubject.ReactToMouseWheelMovement(100);            

            //Act
            testSubject.UpdateCamera();

            //Assert
            Assert.That(cameraViewModel.LookAtPoint, Is.EqualTo(new Point3D()));
            Assert.That(cameraViewModel.MovePoint, Is.EqualTo(new Point3D()));
            Assert.That(cameraViewModel.MoveVector, Is.Not.EqualTo(new Vector3D()));
            Assert.That(cameraViewModel.RotationX, Is.EqualTo(0));
            Assert.That(cameraViewModel.RotationY, Is.EqualTo(0));
            Assert.That(cameraViewModel.RotationZ, Is.EqualTo(0));
        }

        [Test]
        public void UpdateCamera_LeftButtonPressedAndMovedThenReleased_ExpectNoMovementAndNoRotation()
        {
            //Arrange
            var cameraViewModel = new FakeCameraViewModel();

            var testSubject = new MouseCameraControlViewModel(cameraViewModel);

            testSubject.ReactToMouseMovement(new Point(0, 0));
            testSubject.ReactToMouseDown(System.Windows.Input.MouseButton.Left);
            testSubject.ReactToMouseMovement(new Point(10, 10));
            testSubject.ReactToMouseUp(System.Windows.Input.MouseButton.Left);


            //Act
            testSubject.UpdateCamera();

            //Assert
            Assert.That(cameraViewModel.LookAtPoint, Is.EqualTo(new Point3D()));
            Assert.That(cameraViewModel.MovePoint, Is.EqualTo(new Point3D()));
            Assert.That(cameraViewModel.MoveVector, Is.EqualTo(new Vector3D()));
            Assert.That(cameraViewModel.RotationX, Is.EqualTo(0));
            Assert.That(cameraViewModel.RotationY, Is.EqualTo(0));
            Assert.That(cameraViewModel.RotationZ, Is.EqualTo(0));
        }

        [Test]
        public void UpdateCamera_LeftButtonPressedAndMoved_ExpectMovementAndNoRotation()
        {
            //Arrange
            var cameraViewModel = new FakeCameraViewModel();

            var testSubject = new MouseCameraControlViewModel(cameraViewModel);

            testSubject.ReactToMouseMovement(new Point(0, 0));
            testSubject.ReactToMouseDown(System.Windows.Input.MouseButton.Left);
            testSubject.ReactToMouseMovement(new Point(10, 10));


            //Act
            testSubject.UpdateCamera();

            //Assert
            Assert.That(cameraViewModel.LookAtPoint, Is.EqualTo(new Point3D()));
            Assert.That(cameraViewModel.MovePoint, Is.EqualTo(new Point3D()));
            Assert.That(cameraViewModel.MoveVector, Is.Not.EqualTo(new Vector3D()));
            Assert.That(cameraViewModel.RotationX, Is.EqualTo(0));
            Assert.That(cameraViewModel.RotationY, Is.EqualTo(0));
            Assert.That(cameraViewModel.RotationZ, Is.EqualTo(0));
        }

        [Test]
        public void UpdateCamera_CenterButtonPressedAndMoved_ExpectNoMovementAndRotation()
        {
            //Arrange
            var cameraViewModel = new FakeCameraViewModel();

            var testSubject = new MouseCameraControlViewModel(cameraViewModel);

            testSubject.ReactToMouseMovement(new Point(0, 0));
            testSubject.ReactToMouseDown(System.Windows.Input.MouseButton.Middle);
            testSubject.ReactToMouseMovement(new Point(10, 10));


            //Act
            testSubject.UpdateCamera();

            //Assert
            Assert.That(cameraViewModel.LookAtPoint, Is.EqualTo(new Point3D()));
            Assert.That(cameraViewModel.MovePoint, Is.EqualTo(new Point3D()));
            Assert.That(cameraViewModel.MoveVector, Is.EqualTo(new Vector3D()));
            Assert.That(cameraViewModel.RotationX, Is.EqualTo(0));
            Assert.That(cameraViewModel.RotationY, Is.Not.EqualTo(0));
            Assert.That(cameraViewModel.RotationZ, Is.Not.EqualTo(0));
        }

        [Test]
        public void UpdateCamera_RightButtonPressedAndMoved_ExpectMovementAndRotation()
        {
            //Arrange
            var cameraViewModel = new FakeCameraViewModel();
            cameraViewModel.Position = new Point3D(10, 10, 10);

            var testSubject = new MouseCameraControlViewModel(cameraViewModel);

            testSubject.ReactToMouseMovement(new Point(0, 0));
            testSubject.ReactToMouseDown(System.Windows.Input.MouseButton.Right);
            testSubject.ReactToMouseMovement(new Point(10, 10));


            //Act
            testSubject.UpdateCamera();

            //Assert
            Assert.That(cameraViewModel.LookAtPoint, Is.EqualTo(new Point3D()));
            Assert.That(cameraViewModel.MovePoint, Is.Not.EqualTo(new Point3D()));
            Assert.That(cameraViewModel.MoveVector, Is.EqualTo(new Vector3D()));
            Assert.That(cameraViewModel.Rotation, Is.EqualTo(0));
        }
    }
}
