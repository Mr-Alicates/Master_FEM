using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using POC3D.Helpers;
using POC3D.Model;
using POC3D.Model.Calculations;
using POC3D.ViewModel.Commands;

namespace POC3D.ViewModel
{
    public class ProblemViewModel : Observable
    {
        private readonly ModelProblem _modelProblem;
        private ElementViewModel _selectedElement;
        private ForceViewModel _selectedForce;
        private NodeViewModel _selectedNode;

        private bool _showProblem = true;
        private int _displacementsMultiplier = 1;

        private CorrespondenceMatrix _correspondenceMatrix;
        private NumericMatrix _globalStiffnessMatrix;
        private NumericMatrix _compactedMatrix;
        private NumericMatrix _compactedForcesVector;
        private NumericMatrix _solvedDisplacementsVector;
        private bool? _canBeSolved;

        public ProblemViewModel()
        {
            _modelProblem = new ModelProblem("Problem1");

            Nodes = new ObservableCollection<NodeViewModel>();
            Elements = new ObservableCollection<ElementViewModel>();
            Forces = new ObservableCollection<ForceViewModel>();
            Materials = new ObservableCollection<MaterialViewModel>();
            InitializeMaterials();

            ResultNodes = new ObservableCollection<ResultNodeViewModel>();

            NewElementViewModel = new NewElementViewModel(this);
            NewForceViewModel = new NewForceViewModel(this);
        }

        public NodeViewModel SelectedNode
        {
            get => _selectedNode;
            set
            {
                if (_selectedNode == value) return;

                if (_selectedNode != null) _selectedNode.IsSelected = false;

                _selectedNode = value;

                if (_selectedNode != null)
                {
                    _selectedNode.IsSelected = true;
                    SelectedElement = null;
                    SelectedForce = null;
                }

                OnPropertyChanged(nameof(SelectedNode));
            }
        }

        public ElementViewModel SelectedElement
        {
            get => _selectedElement;
            set
            {
                if (_selectedElement == value) return;

                if (_selectedElement != null) _selectedElement.IsSelected = false;

                _selectedElement = value;

                if (_selectedElement != null)
                {
                    _selectedElement.IsSelected = true;
                    SelectedNode = null;
                    SelectedForce = null;
                }

                OnPropertyChanged(nameof(SelectedElement));
            }
        }

        public ForceViewModel SelectedForce
        {
            get => _selectedForce;
            set
            {
                if (_selectedForce == value) return;

                if (_selectedForce != null) _selectedForce.IsSelected = false;

                _selectedForce = value;

                if (_selectedForce != null)
                {
                    _selectedForce.IsSelected = true;
                    SelectedNode = null;
                    SelectedElement = null;
                }

                OnPropertyChanged(nameof(SelectedForce));
            }
        }

        public NewElementViewModel NewElementViewModel { get; }

        public NewForceViewModel NewForceViewModel { get; }

        public ObservableCollection<NodeViewModel> Nodes { get; }

        public ObservableCollection<ElementViewModel> Elements { get; }

        public ObservableCollection<ForceViewModel> Forces { get; }

        public ObservableCollection<MaterialViewModel> Materials { get; }

        public ObservableCollection<ResultNodeViewModel> ResultNodes { get; }

        public ICommand AddNodeCommand => new AddNodeCommand(this);

        public ICommand DeleteNodeCommand => new DeleteNodeCommand(this);

        public ICommand DeleteElementCommand => new DeleteElementCommand(this);

        public ICommand DeleteForceCommand => new DeleteForceCommand(this);

        public CorrespondenceMatrix CorrespondenceMatrix => _correspondenceMatrix ??= MatrixHelper.BuildCorrespondenceMatrix(this);

        public NumericMatrix GlobalStiffnessMatrix => _globalStiffnessMatrix ??= MatrixHelper.BuildGlobalStiffnessMatrix(this);

        public NumericMatrix CompactedMatrix => _compactedMatrix ??= MatrixHelper.BuildCompactedMatrix(this);

        public NumericMatrix CompactedForcesVector => _compactedForcesVector ??= MatrixHelper.BuildCompactedForcesVector(this);

        public NumericMatrix SolvedDisplacementsVector => _solvedDisplacementsVector ??= MatrixHelper.SolveForDisplacements(this);

        public bool CanBeSolved => _canBeSolved ??= MatrixHelper.CanProblemBeSolved(this);

        public int NumberOfNodes => Nodes.Count;

        public int NumberOfElements => Elements.Count;

        public int NumberOfDirichletBoundaryConditions => Nodes.Count(x => x.IsFixed);

        public bool ShowProblem
        {
            get => _showProblem;
            set
            {
                _showProblem = value;
                UpdateDisplacementsInResultNodes();
                OnPropertyChanged(nameof(ShowProblem));
            }
        }

        public int DisplacementsMultiplier
        {
            get => _displacementsMultiplier;
            set
            {
                _displacementsMultiplier = value;
                UpdateDisplacementsInResultNodes();
                OnPropertyChanged(nameof(DisplacementsMultiplier));
            }
        }

        private void InitializeMaterials()
        {
            foreach (var modelMaterial in MaterialsHelper.GetAvailableMaterials())
                Materials.Add(new MaterialViewModel(modelMaterial));
        }

        public NodeViewModel AddNode(Point3D point)
        {
            var nodeViewModel = AddNode();
            nodeViewModel.X = point.X;
            nodeViewModel.Y = point.Y;
            nodeViewModel.Z = point.Z;

            return nodeViewModel;
        }

        public NodeViewModel AddNode()
        {
            var modelNode = _modelProblem.AddNode();
            var nodeViewModel = new NodeViewModel(modelNode);
            var resultNodeViewModel = new ResultNodeViewModel(nodeViewModel);

            Nodes.Add(nodeViewModel);
            ResultNodes.Add(resultNodeViewModel);
            SelectedNode = nodeViewModel;

            ProblemChanged();

            return nodeViewModel;
        }

        public void DeleteSelectedNode()
        {
            var selectedNode = SelectedNode;

            _modelProblem.DeleteNode(selectedNode.Node);
            Nodes.Remove(selectedNode);
            var resultNode = ResultNodes.First(x => x.NodeViewModel == selectedNode);
            ResultNodes.Remove(resultNode);

            SelectedNode = null;

            ProblemChanged();
        }

        public ElementViewModel AddBarElement(NodeViewModel node1, NodeViewModel node2)
        {
            var element = _modelProblem.AddBarElement(node1.Node, node2.Node);

            var result = new ElementViewModel(element, node1, node2);

            //Test: to be removed
            result.CrossSectionArea = 10;
            result.Material = Materials.First();

            Elements.Add(result);
            SelectedElement = result;

            result.PropertyChanged += ElementPropertyChanged;
            ProblemChanged();

            return result;
        }

        public void DeleteSelectedElement()
        {
            var selectedElement = SelectedElement;

            _modelProblem.DeleteElement(selectedElement.Element);
            Elements.Remove(selectedElement);

            SelectedElement.PropertyChanged -= ElementPropertyChanged;
            SelectedElement = null;

            ProblemChanged();
        }

        public ForceViewModel AddForce(NodeViewModel node)
        {
            var force = _modelProblem.AddForce(node.Node);

            var result = new ForceViewModel(force, node);

            Forces.Add(result);
            SelectedForce = result;

            result.PropertyChanged += ForcePropertyChanged;
            ProblemChanged();

            return result;
        }

        public void DeleteSelectedForce()
        {
            var selectedForce = SelectedForce;

            _modelProblem.DeleteForce(selectedForce.Force);

            selectedForce.PropertyChanged -= ForcePropertyChanged;
            Forces.Remove(selectedForce);
            SelectedForce = null;

            ProblemChanged();
        }

        private void ElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ElementViewModel.GlobalStiffnessMatrix))
            {
                ProblemChanged();
            }
        }

        private void ForcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ForceViewModel.Magnitude) ||
                e.PropertyName == nameof(ForceViewModel.Node))
            {
                ProblemChanged();
            }
        }

        private void ProblemChanged()
        {
            _correspondenceMatrix = null;
            _globalStiffnessMatrix = null;
            _compactedMatrix = null;
            _compactedForcesVector = null;
            _solvedDisplacementsVector = null;
            _canBeSolved = null;

            OnPropertyChanged(nameof(CorrespondenceMatrix));
            OnPropertyChanged(nameof(GlobalStiffnessMatrix));
            OnPropertyChanged(nameof(CompactedMatrix));
            OnPropertyChanged(nameof(CompactedForcesVector));
            OnPropertyChanged(nameof(SolvedDisplacementsVector));
            OnPropertyChanged(nameof(CanBeSolved));
            OnPropertyChanged(nameof(NumberOfNodes));
            OnPropertyChanged(nameof(NumberOfElements));
            OnPropertyChanged(nameof(NumberOfDirichletBoundaryConditions));
        }


        private void UpdateDisplacementsInResultNodes()
        {
            int index = 0;
            foreach(var resultNode in ResultNodes)
            {
                if (resultNode.NodeViewModel.IsFixed)
                {
                    continue;
                }

                resultNode.DisplacementX = SolvedDisplacementsVector[index + 0, 0] * DisplacementsMultiplier;
                resultNode.DisplacementY = SolvedDisplacementsVector[index + 1, 0] * DisplacementsMultiplier;
                resultNode.DisplacementZ = SolvedDisplacementsVector[index + 2, 0] * DisplacementsMultiplier;
                index = index + 3;
            }
        }
    }
}