using System.Collections.Generic;

namespace POC3D.Model
{
    public interface IModelProblem
    {
        IEnumerable<IModelNode> Nodes { get; }

        IEnumerable<IModelElement> Elements { get; }

        IEnumerable<IModelForce> Forces { get; }

        IModelNode AddNode();

        IModelElement AddElement(IModelNode node1, IModelNode node2);

        IModelForce AddForce(IModelNode node);

        void DeleteNode(IModelNode selectedNodeNode);

        void DeleteElement(IModelElement element);

        void DeleteForce(IModelForce force);
    }
}
