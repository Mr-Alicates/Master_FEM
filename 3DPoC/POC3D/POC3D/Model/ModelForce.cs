using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC3D.Model
{
    public class ModelForce
    {
        public ModelForce(ModelNode applicationNode)
        {
            Node = applicationNode;
            ApplicationVector = new ModelVector();
        }

        public ModelNode Node { get; }

        public ModelVector ApplicationVector { get; }

        public double Magnitude { get; }
    }
}
