using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC3D.ViewModel.Geometry
{
    public class ResultForceGeometryViewModel : ForceGeometryViewModel
    {
        public ResultForceGeometryViewModel(ForceViewModel forceViewModel)
            : base(forceViewModel)
        {
        }

        protected override void UpdateGeometryMesh()
        {
            base.UpdateGeometryMesh();

            OffsetX += ViewModel.Node.DisplacementX;
            OffsetY += ViewModel.Node.DisplacementY;
            OffsetZ += ViewModel.Node.DisplacementZ;
        }
    }
}
