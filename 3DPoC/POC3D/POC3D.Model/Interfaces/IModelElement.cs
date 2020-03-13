namespace POC3D.Model
{
    public interface IModelElement : IEntity
    {
        string Description { get; }

        IModelNode OriginNode { get; set; }

        IModelNode DestinationNode { get; set; }

        IModelMaterial Material { get; set; }

        double CrossSectionArea { get; set; }

        double Length { get; }

        double K { get; }
    }
}