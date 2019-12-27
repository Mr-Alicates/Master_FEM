using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC3D.Model
{
    public interface IModelElement
    {
        int Id { get; }

        string Description { get; }

        ModelNode OriginNode { get; set; }

        ModelNode DestinationNode { get; set; }

        ModelMaterial Material { get; set; }

        double CrossSectionArea { get; set; }

        double Length { get; }

        double K { get; }
    }
}
