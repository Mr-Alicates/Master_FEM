﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC3D.Model.Serialization
{
    public class NodeMemento
    {
        public int Id { get; set; }

        public double X { get; set; }
        
        public double Y { get; set; }

        public double Z { get; set; }

        public bool IsFixed { get; set; }
    }
}
