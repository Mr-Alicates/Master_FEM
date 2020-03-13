namespace POC3D.Model
{
    public interface IModelNode : IEntity
    {        
        ModelPoint Coordinates { get; }

        bool IsFixed { get; set; }
    }
}
