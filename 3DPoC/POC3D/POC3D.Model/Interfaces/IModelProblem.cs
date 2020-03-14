using System.Collections.Generic;

namespace POC3D.Model
{
    public interface IModelProblem
    {
        IEnumerable<IModelNode> Nodes { get; }

        IEnumerable<IModelElement> Elements { get; }

        IEnumerable<IModelForce> Forces { get; }

        IEnumerable<IModelMaterial> Materials { get; }

        IModelNode AddNode();

        IModelElement AddElement(IModelNode origin, IModelNode destination);

        IModelForce AddForce(IModelNode node);

        IModelMaterial AddMaterial();

        void DeleteNode(IModelNode node);

        void DeleteElement(IModelElement element);

        void DeleteForce(IModelForce force);

        void DeleteMaterial(IModelMaterial material);
    }
}
