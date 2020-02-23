using POC3D.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC3D.ViewModel
{
    public class ResultNodeViewModel : NodeViewModel
    {
        public ResultNodeViewModel(ModelNode modelNode)
            : base(modelNode)
        {

        }

        public double DisplacementX
        {
            get => DisplacementOffsetX;
            set => DisplacementOffsetX = value;
        }

        public double DisplacementY
        {
            get => DisplacementOffsetY;
            set => DisplacementOffsetY = value;
        }

        public double DisplacementZ
        {
            get => DisplacementOffsetZ;
            set => DisplacementOffsetZ = value;
        }
    }
}
