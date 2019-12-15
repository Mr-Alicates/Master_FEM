﻿using POC3D.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace POC3D.ViewModel
{
    public class ProblemViewModel : Observable
    {
        private NodeViewModel _selectedNode;
        private ElementViewModel _selectedElement;
        private ForceViewModel _selectedForce;
        private readonly ModelProblem _modelProblem;

        public ProblemViewModel()
        {
            _modelProblem = new ModelProblem("Problem1");

            Nodes = new ObservableCollection<NodeViewModel>();
            Elements = new ObservableCollection<ElementViewModel>();
            Forces = new ObservableCollection<ForceViewModel>();

            NewElementViewModel = new NewElementViewModel(this);
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
                }

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

                if (_selectedForce != null)
                {
                    _selectedForce.IsSelected = false;
                }

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
                
        public ObservableCollection<NodeViewModel> Nodes { get; }

        public ObservableCollection<ElementViewModel> Elements { get; }

        public ObservableCollection<ForceViewModel> Forces { get; }

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

        public ICommand AddNodeCommand => new AddNodeCommand(this);

        public ICommand DeleteNodeCommand => new DeleteNodeCommand(this);

        public ICommand DeleteElementCommand => new DeleteElementCommand(this);

        public ICommand DeleteForceCommand => new DeleteForceCommand(this);
    }

    public class DeleteForceCommand : ICommand
    {
        private readonly ProblemViewModel _problemViewModel;
        public event EventHandler CanExecuteChanged;
        private bool _canExecute;

        public DeleteForceCommand(ProblemViewModel problemViewModel)
        {
            _problemViewModel = problemViewModel;

            problemViewModel.PropertyChanged += PropertiesChanged;
        }

        private void PropertiesChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _canExecute = _problemViewModel.SelectedForce != null;
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public void Execute(object parameter)
        {
            _problemViewModel.DeleteSelectedForce();
        }
    }

    public class DeleteElementCommand : ICommand
    {
        private readonly ProblemViewModel _problemViewModel;
        public event EventHandler CanExecuteChanged;
        private bool _canExecute;

        public DeleteElementCommand(ProblemViewModel problemViewModel)
        {
            _problemViewModel = problemViewModel;

            problemViewModel.PropertyChanged += PropertiesChanged;
        }

        private void PropertiesChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _canExecute = _problemViewModel.SelectedElement != null;
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public void Execute(object parameter)
        {
            _problemViewModel.DeleteSelectedElement();
        }
    }

    public class DeleteNodeCommand : ICommand
    {
        private readonly ProblemViewModel _problemViewModel;
        public event EventHandler CanExecuteChanged;
        private bool _canExecute;

        public DeleteNodeCommand(ProblemViewModel problemViewModel)
        {
            _problemViewModel = problemViewModel;

            problemViewModel.PropertyChanged += PropertiesChanged;
        }

        private void PropertiesChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _canExecute = _problemViewModel.SelectedNode != null;
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public void Execute(object parameter)
        {
            _problemViewModel.DeleteSelectedNode();
        }
    }

    public class AddNodeCommand : ICommand
    {
        private readonly ProblemViewModel _problemViewModel;
        public event EventHandler CanExecuteChanged;

        public AddNodeCommand(ProblemViewModel problemViewModel)
        {
            _problemViewModel = problemViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _problemViewModel.AddNode();
        }
    }
}
