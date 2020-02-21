﻿using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using POC3D.Helpers;
using POC3D.Model;

namespace POC3D.ViewModel
{
    public class ForceViewModel : GeometryViewModel
    {
        private static readonly Vector3D VerticalVector = new Vector3D(0, 0, 1);
        private static readonly Brush ForceBrush = Brushes.Yellow;
        private static readonly Brush SelectedForceBrush = Brushes.Red;

        private bool _isSelected;
        private NodeViewModel _nodeViewModel;

        public ForceViewModel(ModelForce force, NodeViewModel node)
        {
            Force = force;
            Node = node;
            UpdateGeometryMesh();
        }

        public ModelForce Force { get; }

        public NodeViewModel Node
        {
            get => _nodeViewModel;
            set
            {
                if(_nodeViewModel != null)
                {
                    _nodeViewModel.PropertyChanged -= NodeChanged;
                }

                value.PropertyChanged += NodeChanged;
                _nodeViewModel = value;

                Force.Node = _nodeViewModel.Node;
                NodeChanged();

                OnPropertyChanged(nameof(Node));
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                MaterialBrush = IsSelected ? SelectedForceBrush : ForceBrush;
            }
        }

        public double ApplicationVectorX
        {
            get => Force.ApplicationVector.X;
            set
            {
                Force.ApplicationVector.X = value;
                VectorChanged();
                OnPropertyChanged(nameof(ApplicationVector));
                OnPropertyChanged(nameof(Magnitude));
            }
        }

        public double ApplicationVectorY
        {
            get => Force.ApplicationVector.Y;
            set
            {
                Force.ApplicationVector.Y = value;
                VectorChanged();
                OnPropertyChanged(nameof(ApplicationVector));
                OnPropertyChanged(nameof(Magnitude));
            }
        }

        public double ApplicationVectorZ
        {
            get => Force.ApplicationVector.Z;
            set
            {
                Force.ApplicationVector.Z = value;
                VectorChanged();
                OnPropertyChanged(nameof(ApplicationVector));
                OnPropertyChanged(nameof(Magnitude));
            }
        }

        public Vector3D ApplicationVector => new Vector3D(Force.ApplicationVector.X, Force.ApplicationVector.Y, Force.ApplicationVector.Z);

        public double Magnitude
        {
            get => Force.Magnitude;
            set => Force.Magnitude = value;
        }

        public string Name =>
            $"({Node.Id}) ---> ({ApplicationVectorX:N}/{ApplicationVectorY:N}/{ApplicationVectorZ:N}) ({Magnitude:N})";

        private void NodeChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(_nodeViewModel.Coordinates))
            {
                NodeChanged();
            }
        }

        private void NodeChanged()
        {
            OffsetX = _nodeViewModel.X;
            OffsetY = _nodeViewModel.Y;
            OffsetZ = _nodeViewModel.Z;
        }

        private void VectorChanged()
        {
            RotationAngle = Vector3D.AngleBetween(VerticalVector, -ApplicationVector);
            RotationAxis = Vector3D.CrossProduct(VerticalVector, -ApplicationVector);
        }

        protected override void UpdateGeometryMesh()
        {
            GraphicsHelper.BuildForceArrow(MeshGeometry3D, 10, 2);
        }
    }
}