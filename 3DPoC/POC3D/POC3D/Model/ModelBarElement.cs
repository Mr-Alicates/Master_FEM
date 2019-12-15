using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC3D.Model
{
    public class ModelBarElement : IModelElement
    {
        public ModelBarElement(ModelNode origin, ModelNode destination)
        {
            OriginNode = origin;
            DestinationNode = destination;
        }

        public string Type => "Bar";

        public int NumberOfNodes => 2;

        public ModelNode OriginNode { get; set; }

        public ModelNode DestinationNode { get; set; }

        public double YoungsModulus { get; set; }

        public double PoissonRatio { get; set; }

        public double CrossSectionArea { get; set; }

        public double Length => new ModelVector(DestinationNode.Coordinates, OriginNode.Coordinates).Modulus;

        public double K => (YoungsModulus * CrossSectionArea) / Length;
    }
}
