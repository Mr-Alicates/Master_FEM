namespace POC3D.Model
{
    public interface IModelNode
    {
        int Id { get; }
        
        ModelPoint Coordinates { get; }

        bool IsFixed { get; set; }
    }
}
