namespace POC3D.Model
{
    public interface IModelNode : IEntity
    {        
        ModelPoint Coordinates { get; }

        bool IsXFixed { get; set; }
        
        bool IsYFixed { get; set; }

        bool IsZFixed { get; set; }
    }
}
