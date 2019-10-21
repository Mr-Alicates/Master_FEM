using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC3D.Model
{
    public class ModelNode
    {
        public ModelNode(ModelPoint coordinates)
        {
            Coordinates = coordinates;
        }

        public ModelPoint Coordinates { get; }

        public bool IsFixed { get; set; }

        public void SetAsFixed()
        {
            IsFixed = true;
        }

        public void SetAsFree()
        {
            IsFixed = false;
        }
    }
}
