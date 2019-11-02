using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC3D.Model
{
    public class ModelNode
    {
        //I will cleanup this later;
        private static int IdCounter = 1;

        public static ModelNode CreateNewNode() => new ModelNode(IdCounter++, new ModelPoint(0, 0, 0));

        public ModelNode(int id, ModelPoint coordinates)
        {
            Coordinates = coordinates;
            Id = id;
        }

        public int Id { get; }

        public ModelPoint Coordinates { get; }

        public bool IsFixed { get; set; }
    }
}
