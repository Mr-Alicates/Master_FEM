using POC3D.ViewModel.Base;
using POC3D.ViewModel.Implementation;

namespace POC3D.ViewModel.Calculations
{
    public class ElementCalculationViewModel : Observable
    {
        private double? _cx;
        private double? _cy;
        private double? _cz;
        private NumericMatrix _transformationMatrix;
        private NumericMatrix _transformationMatrixTransposed;
        private NumericMatrix _localStiffnessMatrix;
        private NumericMatrix _globalStiffnessMatrix;

        private readonly ElementViewModel _elementViewModel;

        public ElementCalculationViewModel(ElementViewModel elementViewModel)
        {
            _elementViewModel = elementViewModel;

            _elementViewModel.PropertyChanged += ElementViewModelChanged; 
        }

        public NumericMatrix TransformationMatrix =>
    _transformationMatrix ??= MatrixHelper.BuildTransformationMatrix(this);

        public NumericMatrix TransformationMatrixTransposed =>
            _transformationMatrixTransposed ??= TransformationMatrix.Transpose();

        public NumericMatrix LocalStiffnessMatrix =>
            _localStiffnessMatrix ??= MatrixHelper.BuildElementLocalStiffnessMatrix(_elementViewModel);

        public NumericMatrix GlobalStiffnessMatrix =>
            _globalStiffnessMatrix ??= MatrixHelper.BuildElementGlobalStiffnessMatrix(this);

        public double Cx => _cx ??= (_elementViewModel.Destination.Coordinates.X - _elementViewModel.Origin.Coordinates.X) / _elementViewModel.Length;

        public double Cy => _cy ??= (_elementViewModel.Destination.Coordinates.Y - _elementViewModel.Origin.Coordinates.Y) / _elementViewModel.Length;

        public double Cz => _cz ??= (_elementViewModel.Destination.Coordinates.Z - _elementViewModel.Origin.Coordinates.Z) / _elementViewModel.Length;

        private void ElementViewModelChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName != nameof(ElementViewModel.Geometry))
            {
                return;
            }

            _cx = null;
            _cy = null;
            _cz = null;

            _transformationMatrix = null;
            _transformationMatrixTransposed = null;
            _localStiffnessMatrix = null;
            _globalStiffnessMatrix = null;

            OnPropertyChanged(nameof(Cx));
            OnPropertyChanged(nameof(Cy));
            OnPropertyChanged(nameof(Cz));
            OnPropertyChanged(nameof(TransformationMatrix));
            OnPropertyChanged(nameof(TransformationMatrixTransposed));
            OnPropertyChanged(nameof(LocalStiffnessMatrix));
            OnPropertyChanged(nameof(GlobalStiffnessMatrix));
        }
    }
}
