using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC3D.Model.Serialization
{
    public class MaterialMemento
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public double YoungsModulus { get; set; }
    }
}
