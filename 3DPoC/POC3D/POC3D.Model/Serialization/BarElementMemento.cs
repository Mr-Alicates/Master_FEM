using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC3D.Model.Serialization
{
    public class BarElementMemento
    {
        public int Id { get; set; }

        public int OriginNodeId { get; set; }

        public int DestinationNodeId { get; set; }

        public int MaterialId { get; set; }

        public double CrossSectionArea { get; set; }
    }
}
