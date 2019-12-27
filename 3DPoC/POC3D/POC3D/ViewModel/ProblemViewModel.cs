using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using POC3D.Helpers;
using POC3D.Model;
using POC3D.ViewModel.Commands;

namespace POC3D.ViewModel
{
    public class ProblemViewModel : Observable
    {
        private readonly ModelProblem _modelProblem;
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
            InitializeMaterials();

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

        public ICommand AddNodeCommand => new AddNodeCommand(this);

        public ICommand DeleteNodeCommand => new DeleteNodeCommand(this);

        public ICommand DeleteElementCommand => new DeleteElementCommand(this);

        public ICommand DeleteForceCommand => new DeleteForceCommand(this);

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

            Nodes.Add(nodeViewModel);
            SelectedNode = nodeViewModel;

            return nodeViewModel;
        }

        public void DeleteSelectedNode()
        {
            var selectedNode = SelectedNode;

            _modelProblem.DeleteNode(selectedNode.Node);
            Nodes.Remove(selectedNode);
            SelectedNode = null;
        }

        public ElementViewModel AddBarElement(NodeViewModel node1, NodeViewModel node2)
        {
            var element = _modelProblem.AddBarElement(node1.Node, node2.Node);

            var result = new ElementViewModel(element, node1, node2);

            Elements.Add(result);
            SelectedElement = result;

            return result;
        }

        public void DeleteSelectedElement()
        {
            var selectedElement = SelectedElement;

            _modelProblem.DeleteElement(selectedElement.Element);
            Elements.Remove(selectedElement);
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
            var selectedForce = SelectedForce;

            _modelProblem.DeleteForce(selectedForce.Force);

            Forces.Remove(selectedForce);
            SelectedForce = null;
        }
    }
}