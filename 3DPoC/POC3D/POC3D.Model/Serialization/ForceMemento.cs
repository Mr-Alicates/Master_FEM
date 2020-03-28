using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC3D.Model.Serialization
{
    public class ForceMemento
    {
        public int Id { get; set; }

        public int NodeId { get; set; }

        public double ApplicationVectorX { get; set; }

        public double ApplicationVectorY { get; set; }

        public double ApplicationVectorZ { get; set; }
    }
}
