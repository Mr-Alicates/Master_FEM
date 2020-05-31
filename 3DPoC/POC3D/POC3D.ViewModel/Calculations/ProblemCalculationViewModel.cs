using POC3D.ViewModel.Base;
using POC3D.ViewModel.Implementation;
using System.Threading.Tasks;
using System.Windows;

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


        private bool _displacementAnimation;
        private double _displacementsMultiplier = 1;
        private bool _showProblem = true;

        public ProblemCalculationViewModel(ProblemViewModel problemViewModel)
        {
            _problemViewModel = problemViewModel;

            _problemViewModel.PropertyChanged += (sender, e) => ProblemViewModelChanged(sender, e);
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

        private void ProblemViewModelChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(ProblemViewModel.ProblemCalculationViewModel))
            {
                return;
            }

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

        public double DisplacementsMultiplier
        {
            get => _displacementsMultiplier;
            set
            {
                _displacementsMultiplier = value;
                if (!_showProblem)
                    UpdateDisplacementsInNodes();
                OnPropertyChanged(nameof(DisplacementsMultiplier));
            }
        }

        public bool DisplacementAnimation
        {
            get => _displacementAnimation;
            set
            {
                _displacementAnimation = value;

                if (_displacementAnimation) Application.Current.Dispatcher.InvokeAsync(RunAnimation);
            }
        }

        public bool ShowProblem
        {
            get => _showProblem;
            set
            {
                _showProblem = value;
                if (!_showProblem)
                    UpdateDisplacementsInNodes();
                OnPropertyChanged(nameof(ShowProblem));
            }
        }

        private void UpdateDisplacementsInNodes()
        {
            var index = 0;

            foreach (var node in _problemViewModel.Nodes)
            {
                if (node.IsFixed)
                {
                    node.DisplacementX = 0;
                    node.DisplacementY = 0;
                    node.DisplacementZ = 0;
                }
                else
                {
                    node.DisplacementX = SolvedDisplacementsVector[index + 0, 0] * DisplacementsMultiplier;
                    node.DisplacementY = SolvedDisplacementsVector[index + 1, 0] * DisplacementsMultiplier;
                    node.DisplacementZ = SolvedDisplacementsVector[index + 2, 0] * DisplacementsMultiplier;
                    index = index + 3;
                }
            }
        }


        private async Task RunAnimation()
        {
            const int delay = 100;
            var savedMultiplier = DisplacementsMultiplier;

            var delta = savedMultiplier / 10.0;

            while (DisplacementAnimation)
            {
                for (var i = 0; i < 10; i++)
                {
                    DisplacementsMultiplier -= delta;
                    await Task.Delay(delay);
                }

                for (var i = 0; i < 10; i++)
                {
                    DisplacementsMultiplier += delta;
                    await Task.Delay(delay);
                }
            }

            DisplacementsMultiplier = savedMultiplier;
        }
    }
}
