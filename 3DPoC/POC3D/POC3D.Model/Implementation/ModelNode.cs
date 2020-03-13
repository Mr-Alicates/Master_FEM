namespace POC3D.Model
{
    public class ModelNode : IModelNode
    {
        //I will cleanup this later;
        private static int IdCounter = 1;

        public ModelNode(int id, ModelPoint coordinates)
        {
            Coordinates = coordinates;
            Id = id;
        }

        public int Id { get; }

        public ModelPoint Coordinates { get; }

        public bool IsFixed { get; set; }

        public static IModelNode CreateNewNode()
        {
            return new ModelNode(IdCounter++, new ModelPoint(0, 0, 0));
        }
    }
}