using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC3D.Model.Serialization
{
    public class ProblemMemento
    {
        public string Name { get; set; }

        public List<NodeMemento> Nodes { get; } = new List<NodeMemento>();

        public List<ForceMemento> Forces { get; } = new List<ForceMemento>();

        public List<BarElementMemento> Elements { get; } = new List<BarElementMemento>();

        public List<MaterialMemento> Materials { get; } = new List<MaterialMemento>();
    }
}
