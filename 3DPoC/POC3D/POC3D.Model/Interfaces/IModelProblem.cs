using System.Collections.Generic;

namespace POC3D.Model
{
    public interface IModelProblem
    {
        IEnumerable<IModelElement> Elements { get; }

        List<IModelNode> Nodes { get; }

        List<IModelForce> Forces { get; }

        IModelNode AddNode();
        
        IModelForce AddForce(IModelNode node);

        IModelElement AddBarElement(IModelNode node1, IModelNode node2);

        void DeleteNode(IModelNode selectedNodeNode);

        void DeleteElement(IModelElement element);

        void DeleteForce(IModelForce force);
    }
}
