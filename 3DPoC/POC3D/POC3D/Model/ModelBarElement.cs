using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC3D.Model
{
    public class ModelBarElement : IModelElement
    {
        private readonly ModelNode[] _nodes;

        public ModelBarElement(ModelNode beggining, ModelNode end)
        {
            _nodes = new ModelNode[]
            {
                beggining,
                end
            };
        }

        public string Type => "Bar";

        public int NumberOfNodes => 2;

        public IEnumerable<ModelNode> Nodes => _nodes;
    }
}
