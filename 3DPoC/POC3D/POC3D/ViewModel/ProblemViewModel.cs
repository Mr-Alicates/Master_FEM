using POC3D.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace POC3D.ViewModel
{
    public class ProblemViewModel : Observable
    {
        public EventHandler SelectedNodeChanged;

        private NodeViewModel _selectedNode;
        private readonly ModelProblem _modelProblem;

        public ProblemViewModel()
        {
            _modelProblem = new ModelProblem("Problem1");

            Nodes = new ObservableCollection<NodeViewModel>();
            Elements = new ObservableCollection<ElementViewModel>();

            Nodes.CollectionChanged += NodeAdded;
        }

        public NodeViewModel SelectedNode
        {
            get => _selectedNode;
            set
            {
                if (_selectedNode == value) return;

                if (_selectedNode != null)
                {
                    _selectedNode.IsSelected = false;
                }

                _selectedNode = value;

                if (_selectedNode != null)
                {
                    _selectedNode.IsSelected = true;
                }

                SelectedNodeChanged?.Invoke(null, null);
            }
        }

        private void NodeAdded(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (NodeViewModel node in e.NewItems)
                {
                    node.PropertyChanged += NodePropertyChanged;
                }
            }
        }

        private void NodePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            SelectedNodeChanged?.Invoke(null, null);
        }

        public ObservableCollection<NodeViewModel> Nodes { get; }
        public ObservableCollection<ElementViewModel> Elements { get; }

        public NodeViewModel AddNode(Point3D point)
        {
            var modelNode = _modelProblem.AddNode(point.X, point.Y, point.Z);

            var nodeViewModel = new NodeViewModel(modelNode);

            Nodes.Add(nodeViewModel);

            return nodeViewModel;
        }

        public ElementViewModel AddBarElement(NodeViewModel node1, NodeViewModel node2)
        {
            var element = _modelProblem.AddBarElement(node1.Node, node2.Node);

            var result = new ElementViewModel(element);

            Elements.Add(result);

            return result;
        }
    }
}
