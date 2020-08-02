namespace POC3D.Model.Serialization
{
    public class NodeMemento
    {
        public int Id { get; set; }

        public double X { get; set; }
        
        public double Y { get; set; }

        public double Z { get; set; }

        public bool IsXFixed { get; set; }

        public bool IsYFixed { get; set; }

        public bool IsZFixed { get; set; }
    }
}
