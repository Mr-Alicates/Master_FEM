using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC3D.ViewModel.Geometry
{
    public class ResultNodeGeometryViewModel : NodeGeometryViewModel
    {
        public ResultNodeGeometryViewModel(NodeViewModel nodeViewModel) 
            : base(nodeViewModel)
        {
        }

        protected override void UpdateGeometryMesh()
        {
            base.UpdateGeometryMesh();

            OffsetX += ViewModel.DisplacementX;
            OffsetY += ViewModel.DisplacementY;
            OffsetZ += ViewModel.DisplacementZ;
        }
    }
}
