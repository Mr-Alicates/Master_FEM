using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using POC3D.Model;
using POC3D.Model.Serialization;
using POC3D.ViewModel.Base;
using POC3D.ViewModel.Calculations;
using POC3D.ViewModel.Commands;
using POC3D.ViewModel.Dialog;

namespace POC3D.ViewModel.Implementation
{
    public class ProblemViewModel : Observable
    {
        private IModelProblem _modelProblem;
        private IProblemSerializer _problemSerializer;
        private IDialogService _dialogService;
        private SelectableViewModel _selectedItem;

        public ProblemViewModel()
            : this(new ModelProblem("Problem1"), new ProblemSerializer(new FileSystem()), new DialogService())
        {
        }

        public ProblemViewModel(IModelProblem modelProblem, IProblemSerializer problemSerializer, IDialogService dialogService)
        {
            Nodes = new ObservableCollection<NodeViewModel>();
            Elements = new ObservableCollection<ElementViewModel>();
            Forces = new ObservableCollection<ForceViewModel>();
            Materials = new ObservableCollection<MaterialViewModel>();

            _dialogService = dialogService;
            _problemSerializer = problemSerializer;
            LoadProblem(modelProblem);

            Nodes.CollectionChanged += CollectionChanged;
            Elements.CollectionChanged += CollectionChanged;
            Forces.CollectionChanged += CollectionChanged;
            Materials.CollectionChanged += CollectionChanged;

            ProblemCalculationViewModel = new ProblemCalculationViewModel(this);

            SaveProblemCommand = new Command(SaveProblem);
            LoadProblemCommand = new Command(LoadProblem);
        }

        public ICommand SaveProblemCommand { get; }

        public ICommand LoadProblemCommand { get; }

        public string Name => _modelProblem.Name;

        private SelectableViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != null)
                {
                    _selectedItem.IsSelected = false;
                    _selectedItem.PropertyChanged -= SelectedItemChanged;
                }

                _selectedItem = value;

                if (_selectedItem != null)
                {
                    _selectedItem.IsSelected = true;
                    _selectedItem.PropertyChanged += SelectedItemChanged;
                }

                OnPropertyChanged(nameof(SelectedNode));
                OnPropertyChanged(nameof(SelectedElement));
                OnPropertyChanged(nameof(SelectedForce));
                OnPropertyChanged(nameof(SelectedMaterial));

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
            set => SelectedItem = value;
        }

        public ForceViewModel SelectedForce
        {
            get => SelectedItem as ForceViewModel;
            set => SelectedItem = value;
        }

        public MaterialViewModel SelectedMaterial
        {
            get => SelectedItem as MaterialViewModel;
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

            return nodeViewModel;
        }

        public void DeleteSelectedNode()
        {
            if(SelectedNode == null)
            {
                return;
            }

            _modelProblem.DeleteNode(SelectedNode.Node);
            Nodes.Remove(SelectedNode);
            SelectedNode = null;
        }

        public ElementViewModel AddBarElement(NodeViewModel node1, NodeViewModel node2)
        {
            var element = _modelProblem.AddElement(node1.Node, node2.Node);

            var materialViewModel = GetOrInitMaterialViewModel(element.Material);

            var result = new ElementViewModel(element, node1, node2, materialViewModel);
            Elements.Add(result);
            SelectedElement = result;

            return result;
        }

        public void DeleteSelectedElement()
        {
            if(SelectedElement == null)
            {
                return;
            }

            _modelProblem.DeleteElement(SelectedElement.Element);
            Elements.Remove(SelectedElement);
            SelectedElement = null;
        }

        public ForceViewModel AddForce(NodeViewModel node)
        {
            var force = _modelProblem.AddForce(node.Node);

            var result = new ForceViewModel(force, node);

            Forces.Add(result);
            SelectedForce = result;

            return result;
        }

        public void DeleteSelectedForce()
        {
            if(SelectedForce == null)
            {
                return;
            }
            
            _modelProblem.DeleteForce(SelectedForce.Force);
            Forces.Remove(SelectedForce);
            SelectedForce = null;
        }

        public MaterialViewModel AddMaterial()
        {
            var modelMaterial = _modelProblem.AddMaterial();

            var result = new MaterialViewModel(modelMaterial);
            Materials.Add(result);

            SelectedMaterial = result;

            return result;
        }

        public void DeleteSelectedMaterial()
        {
            if (SelectedMaterial == null)
            {
                return;
            }

            _modelProblem.DeleteMaterial(SelectedMaterial.ModelMaterial);
            Materials.Remove(SelectedMaterial);
            SelectedMaterial = null;
        }

        private MaterialViewModel GetOrInitMaterialViewModel(IModelMaterial modelMaterial)
        {
            var result = Materials.FirstOrDefault(vm => vm.ModelMaterial == modelMaterial);

            if (result == null)
            {
                result = AddMaterial();
            }

            return result;
        }

        private void SelectedItemChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ElementViewModel.Origin):
                    OnPropertyChanged(nameof(AvailableDestinationNodesForSelectedElements));
                    break;

                case nameof(ElementViewModel.Destination):
                    OnPropertyChanged(nameof(AvailableOriginNodesForSelectedElements));
                    break;
            }
        }

        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ForceViewModel.Magnitude) ||
                e.PropertyName == nameof(ForceViewModel.Node) ||
                e.PropertyName == nameof(ElementCalculationViewModel.GlobalStiffnessMatrix))
            {
                OnPropertyChanged(nameof(NumberOfNodes));
                OnPropertyChanged(nameof(NumberOfElements));
                OnPropertyChanged(nameof(NumberOfDirichletBoundaryConditions));
            }
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (SelectableViewModel item in e.OldItems)
                {
                    item.PropertyChanged -= ItemPropertyChanged;
                }
            }

            if (e.NewItems != null)
            {
                foreach (SelectableViewModel item in e.NewItems)
                {
                    item.PropertyChanged += ItemPropertyChanged;
                }
            }

            OnPropertyChanged(nameof(NumberOfNodes));
            OnPropertyChanged(nameof(NumberOfElements));
            OnPropertyChanged(nameof(NumberOfDirichletBoundaryConditions));
        }

        private void SaveProblem()
        {
            var savePath = _dialogService.ShowSaveFileDialog();

            if (string.IsNullOrEmpty(savePath)) 
            {
                return;
            }

            try
            {
                _modelProblem.Name = Path.GetFileNameWithoutExtension(savePath);
                _problemSerializer.SerializeProblem(_modelProblem, savePath);
            }
            catch(Exception ex)
            {
                var message = $"There was an error saving the problem: {ex}";
                MessageBox.Show(message, "Error");
            }
        }

        private void LoadProblem()
        {
            var loadPath = _dialogService.ShowSaveFileDialog();

            if (string.IsNullOrEmpty(loadPath))
            {
                return;
            }

            try
            {
                var modelProblem = _problemSerializer.DeserializeProblem(loadPath);
                LoadProblem(modelProblem);
            }
            catch (Exception ex)
            {
                var message = $"There was an error saving the problem: {ex}";
                MessageBox.Show(message, "Error");
            }            
        }

        private void LoadProblem(IModelProblem modelProblem)
        {
            //Clear previous problem
            SelectedItem = null;
            Nodes.Clear();
            Forces.Clear();
            Elements.Clear();
            Materials.Clear();

            //Load the problem
            _modelProblem = modelProblem;

            foreach (var modelNode in _modelProblem.Nodes)
            {
                Nodes.Add(new NodeViewModel(modelNode));
            }

            foreach (var modelForce in _modelProblem.Forces)
            {
                var nodeViewModel = Nodes.First(x => x.Node == modelForce.Node);
                var result = new ForceViewModel(modelForce, nodeViewModel);
                Forces.Add(result);
            }

            foreach (var modelMaterial in _modelProblem.Materials)
            {
                Materials.Add(new MaterialViewModel(modelMaterial));
            }

            foreach (var modelElement in _modelProblem.Elements)
            {
                var originNodeViewModel = Nodes.First(x => x.Node == modelElement.OriginNode);
                var destinationNodeViewModel = Nodes.First(x => x.Node == modelElement.DestinationNode);
                var materialViewModel = Materials.First(x => x.ModelMaterial == modelElement.Material);

                Elements.Add(new ElementViewModel(modelElement, originNodeViewModel, destinationNodeViewModel, materialViewModel));
            }
        }
    }
}