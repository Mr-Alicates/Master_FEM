using NUnit.Framework;
using POC3D.ViewModel.InterfaceManagement.CameraControl;
using POC3D.ViewModel.Tests.Helper;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace POC3D.ViewModel.Tests
{
    [TestFixture]
    public class KeyboardCameraControlViewModelTests
    {
        [Test]
        public void UpdateCamera_NoPressedKeys_ExpectNoMovementOrRotation()
        {
            //Arrange
            var cameraViewModel = new FakeCameraViewModel();

            var testSubject = new KeyboardCameraControlViewModel(cameraViewModel);

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
        public void UpdateCamera_KeyPressedThenReleased_ExpectNoMovementOrRotation()
        {
            //Arrange
            var cameraViewModel = new FakeCameraViewModel();

            var testSubject = new KeyboardCameraControlViewModel(cameraViewModel);

            testSubject.ReactToKeyboardKeyDown(Key.W);
            testSubject.ReactToKeyboardKeyUp(Key.W);

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
        [TestCase(Key.W)]
        [TestCase(Key.A)]
        [TestCase(Key.S)]
        [TestCase(Key.D)]
        [TestCase(Key.R)]
        [TestCase(Key.F)]
        public void UpdateCamera_MovementKeyPressed_ExpectMovementAndNoRotation(Key pressedKey)
        {
            //Arrange
            var cameraViewModel = new FakeCameraViewModel();

            var testSubject = new KeyboardCameraControlViewModel(cameraViewModel);

            testSubject.ReactToKeyboardKeyDown(pressedKey);

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
        [TestCase(Key.W)]
        [TestCase(Key.A)]
        [TestCase(Key.S)]
        [TestCase(Key.D)]
        [TestCase(Key.Q)]
        [TestCase(Key.E)]
        public void UpdateCamera_RotationKeyPressedAndSpecialKeyPressed_ExpectNoMovementAndRotation(Key pressedKey)
        {
            //Arrange
            var cameraViewModel = new FakeCameraViewModel();

            var testSubject = new KeyboardCameraControlViewModel(cameraViewModel);

            testSubject.ReactToKeyboardKeyDown(pressedKey);
            testSubject.ReactToKeyboardKeyDown(Key.LeftShift);

            //Act
            testSubject.UpdateCamera();

            //Assert
            Assert.That(cameraViewModel.LookAtPoint, Is.EqualTo(new Point3D()));
            Assert.That(cameraViewModel.MovePoint, Is.EqualTo(new Point3D()));
            Assert.That(cameraViewModel.MoveVector, Is.EqualTo(new Vector3D()));
            Assert.That(cameraViewModel.Rotation, Is.Not.EqualTo(0));
        }
    }
}
