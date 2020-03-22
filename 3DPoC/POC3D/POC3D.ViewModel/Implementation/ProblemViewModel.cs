using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using POC3D.Model;
using POC3D.ViewModel.Base;
using POC3D.ViewModel.Calculations;

namespace POC3D.ViewModel.Implementation
{
    public class ProblemViewModel : Observable
    {
        private readonly IModelProblem _modelProblem;
        private SelectableViewModel _selectedItem;

        public ProblemViewModel()
        {
            _modelProblem = new ModelProblem("Problem1");

            Nodes = new ObservableCollection<NodeViewModel>();
            Elements = new ObservableCollection<ElementViewModel>();
            Forces = new ObservableCollection<ForceViewModel>();
            Materials = new ObservableCollection<MaterialViewModel>();

            ProblemCalculationViewModel = new ProblemCalculationViewModel(this);
        }

        public SelectableViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != null)
                {
                    _selectedItem.IsSelected = false;
                }

                _selectedItem = value;

                if (_selectedItem != null)
                {
                    _selectedItem.IsSelected = true;
                }

                OnPropertyChanged(nameof(SelectedNode));
                OnPropertyChanged(nameof(SelectedElement));
                OnPropertyChanged(nameof(SelectedForce));

                OnPropertyChanged(nameof(AvailableNodesForSelectedForces));
                OnPropertyChanged(nameof(AvailableOriginNodesForSelectedElements));
                OnPropertyChanged(nameof(AvailableDestinationNodesForSelectedElements));
            }
        }

        public NodeViewModel SelectedNode
        {
            get => SelectedItem as NodeViewModel;
            set => SelectedItem = value;
        }

        public ElementViewModel SelectedElement
        {
            get => SelectedItem as ElementViewModel;
            set
            {
                if (SelectedItem != null)
                {
                    SelectedItem.PropertyChanged -= SelectedElementChanged;
                }

                SelectedItem = value;

                if (SelectedItem != null)
                {
                    SelectedItem.PropertyChanged += SelectedElementChanged;
                }
            }
        }

        public ForceViewModel SelectedForce
        {
            get => SelectedItem as ForceViewModel;
            set => SelectedItem = value;
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
                    if (possibleOriginNode == origin)
                    {
                        yield return possibleOriginNode;
                    }

                    if (possibleOriginNode == destination)
                    {
                        continue;
                    }

                    if (Elements.Any(element =>
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

        public ObservableCollection<NodeViewModel> Nodes { get; }

        public ObservableCollection<ElementViewModel> Elements { get; }

        public ObservableCollection<ForceViewModel> Forces { get; }

        public ObservableCollection<MaterialViewModel> Materials { get; }

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
            selectedElement.ElementCalculationViewModel.PropertyChanged -= ElementPropertyChanged;
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

            if (result == null)
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