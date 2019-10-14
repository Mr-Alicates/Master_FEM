using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC3D.Model
{
    public interface IModelElement
    {
        string Type { get; }

        int NumberOfNodes { get; }

        IEnumerable<ModelNode> Nodes { get; }
    }
}
