﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using POC3D.Model;
using POC3D.ViewModel.Base;
using POC3D.ViewModel.Calculations;
using POC3D.ViewModel.Commands;

namespace POC3D.ViewModel
{
    public class ProblemViewModel : Observable
    {
        private readonly IModelProblem _modelProblem;

        private bool _displacementAnimation;
        private double _displacementsMultiplier = 1;
        private ElementViewModel _selectedElement;
        private ForceViewModel _selectedForce;
        private NodeViewModel _selectedNode;

        private bool _showProblem = true;

        public ProblemViewModel()
        {
            _modelProblem = new ModelProblem("Problem1");

            Nodes = new ObservableCollection<NodeViewModel>();
            Elements = new ObservableCollection<ElementViewModel>();
            Forces = new ObservableCollection<ForceViewModel>();
            Materials = new ObservableCollection<MaterialViewModel>();

            ResultNodes = new ObservableCollection<ResultNodeViewModel>();
            ResultElements = new ObservableCollection<ResultElementViewModel>();
            ResultForces = new ObservableCollection<ResultForceViewModel>();

            NewElementViewModel = new NewElementViewModel(this);
            NewForceViewModel = new NewForceViewModel(this);

            ProblemCalculationViewModel = new ProblemCalculationViewModel(this);
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

        public ObservableCollection<ResultElementViewModel> ResultElements { get; }

        public ObservableCollection<ResultForceViewModel> ResultForces { get; }

        public ICommand AddNodeCommand => new AddNodeCommand(this);

        public ICommand DeleteNodeCommand => new DeleteNodeCommand(this);

        public ICommand DeleteElementCommand => new DeleteElementCommand(this);

        public ICommand DeleteForceCommand => new DeleteForceCommand(this);

        public int NumberOfNodes => Nodes.Count;

        public int NumberOfElements => Elements.Count;

        public int NumberOfDirichletBoundaryConditions => Nodes.Count(x => x.IsFixed);

        public bool ShowProblem
        {
            get => _showProblem;
            set
            {
                _showProblem = value;
                if (!_showProblem)
                    UpdateDisplacementsInResultNodes();
                OnPropertyChanged(nameof(ShowProblem));
            }
        }

        public double DisplacementsMultiplier
        {
            get => _displacementsMultiplier;
            set
            {
                _displacementsMultiplier = value;
                if (!_showProblem)
                    UpdateDisplacementsInResultNodes();
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

        public ProblemCalculationViewModel ProblemCalculationViewModel { get; }

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
            var element = _modelProblem.AddElement(node1.Node, node2.Node);

            var materialViewModel = GetOrInitMaterialViewModel(element.Material);

            var result = new ElementViewModel(element, node1, node2, materialViewModel);

            Elements.Add(result);

            result.ElementCalculationViewModel.PropertyChanged += ElementPropertyChanged;
            ProblemChanged();

            return result;
        }

        public void DeleteSelectedElement()
        {
            var selectedElement = SelectedElement;

            _modelProblem.DeleteElement(selectedElement.Element);
            Elements.Remove(selectedElement);

            SelectedElement.ElementCalculationViewModel.PropertyChanged -= ElementPropertyChanged;
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
            if (e.PropertyName == nameof(ElementCalculationViewModel.GlobalStiffnessMatrix)) ProblemChanged();
        }

        private void ForcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ForceViewModel.Magnitude) ||
                e.PropertyName == nameof(ForceViewModel.Node))
                ProblemChanged();
        }

        private void ProblemChanged()
        {
            OnPropertyChanged(nameof(NumberOfNodes));
            OnPropertyChanged(nameof(NumberOfElements));
            OnPropertyChanged(nameof(NumberOfDirichletBoundaryConditions));
        }

        private void UpdateDisplacementsInResultNodes()
        {
            var index = 0;
            foreach (var resultNode in ResultNodes)
            {
                if (resultNode.NodeViewModel.IsFixed) continue;

                resultNode.DisplacementX = ProblemCalculationViewModel.SolvedDisplacementsVector[index + 0, 0] * DisplacementsMultiplier;
                resultNode.DisplacementY = ProblemCalculationViewModel.SolvedDisplacementsVector[index + 1, 0] * DisplacementsMultiplier;
                resultNode.DisplacementZ = ProblemCalculationViewModel.SolvedDisplacementsVector[index + 2, 0] * DisplacementsMultiplier;
                index = index + 3;
            }

            ResultElements.Clear();

            foreach (var element in Elements)
            {
                var originResultNode = ResultNodes.First(x => x.NodeViewModel == element.Origin);
                var destinationResultNode = ResultNodes.First(x => x.NodeViewModel == element.Destination);

                ResultElements.Add(new ResultElementViewModel(originResultNode, destinationResultNode));
            }

            foreach (var force in Forces)
            {
                var node = ResultNodes.First(x => x.NodeViewModel == force.Node);

                ResultForces.Add(new ResultForceViewModel(force, node));
            }
        }

        private MaterialViewModel GetOrInitMaterialViewModel(IModelMaterial modelMaterial)
        {
            var result = Materials.FirstOrDefault(vm => vm.ModelMaterial == modelMaterial);

            if(result == null)
            {
                result = new MaterialViewModel(modelMaterial);

                Materials.Add(result);
            }

            return result;
        }
    }
}