using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ElementViewModel _selectedElement;
        private ForceViewModel _selectedForce;
        private NodeViewModel _selectedNode;

        public ProblemViewModel()
        {
            _modelProblem = new ModelProblem("Problem1");

            Nodes = new ObservableCollection<NodeViewModel>();
            Elements = new ObservableCollection<ElementViewModel>();
            Forces = new ObservableCollection<ForceViewModel>();
            Materials = new ObservableCollection<MaterialViewModel>();

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

                if (_selectedElement != null)
                {
                    _selectedElement.IsSelected = false;
                    _selectedElement.PropertyChanged -= SelectedElementChanged;
                }

                _selectedElement = value;

                if (_selectedElement != null)
                {
                    _selectedElement.IsSelected = true;
                    _selectedElement.PropertyChanged += SelectedElementChanged;
                    SelectedNode = null;
                    SelectedForce = null;
                }

                OnPropertyChanged(nameof(SelectedElement));
                OnPropertyChanged(nameof(AvailableOriginNodesForSelectedElements));
                OnPropertyChanged(nameof(AvailableDestinationNodesForSelectedElements));
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
                OnPropertyChanged(nameof(AvailableNodesForSelectedForces));
            }
        }

        public IEnumerable<NodeViewModel> AvailableNodesForSelectedForces
        {
            get
            {
                var forcesNodes = Forces.Select(force => force.Node);

                if (SelectedForce != null)
                {
                    forcesNodes = forcesNodes.Except(new[] { SelectedForce.Node });
                }

                return Nodes.Except(forcesNodes);
            }
        }

        public IEnumerable<NodeViewModel> AvailableOriginNodesForSelectedElements
        {
            get
            {
                var origin = SelectedElement?.Origin;
                var destination = SelectedElement?.Destination;

                foreach (var possibleOriginNode in Nodes)
                {
                    if(possibleOriginNode == origin)
                    {
                        yield return possibleOriginNode;
                    }

                    if (possibleOriginNode == destination)
                    {
                        continue;
                    }

                    if(Elements.Any(element => 
                        element.Origin == possibleOriginNode && element.Destination == destination ||
                        element.Origin == destination && element.Destination == possibleOriginNode))
                    {
                        continue;
                    }

                    yield return possibleOriginNode;
                }
            }
        }

        public IEnumerable<NodeViewModel> AvailableDestinationNodesForSelectedElements
        {
            get
            {
                var origin = SelectedElement?.Origin;
                var destination = SelectedElement?.Destination;

                foreach (var possibleDestinationNode in Nodes)
                {
                    if (possibleDestinationNode == destination)
                    {
                        yield return possibleDestinationNode;
                    }

                    if (possibleDestinationNode == origin)
                    {
                        continue;
                    }

                    if (Elements.Any(element =>
                         element.Origin == origin && element.Destination == possibleDestinationNode ||
                         element.Origin == possibleDestinationNode && element.Destination == origin))
                    {
                        continue;
                    }

                    yield return possibleDestinationNode;
                }
            }
        }

        public NewElementViewModel NewElementViewModel { get; }

        public NewForceViewModel NewForceViewModel { get; }

        public ObservableCollection<NodeViewModel> Nodes { get; }

        public ObservableCollection<ElementViewModel> Elements { get; }

        public ObservableCollection<ForceViewModel> Forces { get; }

        public ObservableCollection<MaterialViewModel> Materials { get; }

        public ICommand AddNodeCommand => new AddNodeCommand(this);

        public ICommand DeleteNodeCommand => new DeleteNodeCommand(this);

        public ICommand DeleteElementCommand => new DeleteElementCommand(this);

        public ICommand DeleteForceCommand => new DeleteForceCommand(this);

        public int NumberOfNodes => Nodes.Count;

        public int NumberOfElements => Elements.Count;

        public int NumberOfDirichletBoundaryConditions => Nodes.Count(x => x.IsFixed);

        public ProblemCalculationViewModel ProblemCalculationViewModel { get; }

        public NodeViewModel AddNode()
        {
            var modelNode = _modelProblem.AddNode();
            var nodeViewModel = new NodeViewModel(modelNode);

            Nodes.Add(nodeViewModel);
            SelectedNode = nodeViewModel;

            ProblemChanged();

            return nodeViewModel;
        }

        public void DeleteSelectedNode()
        {
            var selectedNode = SelectedNode;

            _modelProblem.DeleteNode(selectedNode.Node);
            Nodes.Remove(selectedNode);

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

        private void SelectedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ElementViewModel.Origin))
            {
                OnPropertyChanged(nameof(AvailableDestinationNodesForSelectedElements));
            }
            if (e.PropertyName == nameof(ElementViewModel.Destination))
            {
                OnPropertyChanged(nameof(AvailableOriginNodesForSelectedElements));
            }
        }
    }
}