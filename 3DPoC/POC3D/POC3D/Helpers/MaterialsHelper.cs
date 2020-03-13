using System.Collections.Generic;
using POC3D.Model;

namespace POC3D.Helpers
{
    public static class MaterialsHelper
    {
        private static readonly IModelMaterial[] Materials =
        {
            ModelMaterial.None,

            //Data extracted from https://www.engineeringtoolbox.com/young-modulus-d_417.html and https://www.engineeringtoolbox.com/poissons-ratio-d_1224.html
            new ModelMaterial("Aluminum", 69000000000, 0.334),
            new ModelMaterial("Copper", 117000000000, 0.355),
            new ModelMaterial("Grey Cast Iron", 210000000000, 0.211)
        };

        public static IEnumerable<IModelMaterial> GetAvailableMaterials()
        {
            return Materials;
        }
    }
}