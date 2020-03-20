using POC3D.ViewModel.Base;

namespace POC3D.ViewModel.Calculations
{
    public class ProblemCalculationViewModel : Observable
    {
        private readonly ProblemViewModel _problemViewModel;

        private CorrespondenceMatrix _correspondenceMatrix;
        private NumericMatrix _globalStiffnessMatrix;
        private NumericMatrix _compactedMatrix;
        private NumericMatrix _compactedForcesVector;
        private NumericMatrix _solvedDisplacementsVector;
        private NumericMatrix _fullSolvedDisplacementsVector;
        private NumericMatrix _solvedReactionForces;
        private bool? _canBeSolved;

        public ProblemCalculationViewModel(ProblemViewModel problemViewModel)
        {
            _problemViewModel = problemViewModel;

            _problemViewModel.PropertyChanged += (_, __) => ProblemViewModelChanged();
            ProblemViewModelChanged();
        }

        public CorrespondenceMatrix CorrespondenceMatrix =>
            _correspondenceMatrix ??= MatrixHelper.BuildCorrespondenceMatrix(_problemViewModel);

        public NumericMatrix GlobalStiffnessMatrix =>
            _globalStiffnessMatrix ??= MatrixHelper.BuildGlobalStiffnessMatrix(_problemViewModel);

        public NumericMatrix CompactedMatrix => _compactedMatrix ??= MatrixHelper.BuildCompactedMatrix(_problemViewModel);

        public NumericMatrix CompactedForcesVector =>
            _compactedForcesVector ??= MatrixHelper.BuildCompactedForcesVector(_problemViewModel);

        public NumericMatrix SolvedDisplacementsVector =>
            _solvedDisplacementsVector ??= MatrixHelper.SolveForDisplacements(this);

        public NumericMatrix FullSolvedDisplacementsVector =>
            _fullSolvedDisplacementsVector ??= MatrixHelper.BuildFullSolvedDisplacementsVector(_problemViewModel);

        public NumericMatrix SolvedReactionForces =>
            _solvedReactionForces ??= MatrixHelper.SolveForReactionForces(this);

        public bool CanBeSolved => _canBeSolved ??= MatrixHelper.CanProblemBeSolved(_problemViewModel);

        public void ProblemViewModelChanged()
        {
            _correspondenceMatrix = null;
            _globalStiffnessMatrix = null;
            _compactedMatrix = null;
            _compactedForcesVector = null;
            _solvedDisplacementsVector = null;
            _fullSolvedDisplacementsVector = null;
            _solvedReactionForces = null;
            _canBeSolved = null;

            OnPropertyChanged(nameof(CorrespondenceMatrix));
            OnPropertyChanged(nameof(GlobalStiffnessMatrix));
            OnPropertyChanged(nameof(CompactedMatrix));
            OnPropertyChanged(nameof(CompactedForcesVector));
            OnPropertyChanged(nameof(SolvedDisplacementsVector));
            OnPropertyChanged(nameof(FullSolvedDisplacementsVector));
            OnPropertyChanged(nameof(SolvedReactionForces));
            OnPropertyChanged(nameof(CanBeSolved));
        }
    }
}
