namespace POC3D.Model.Serialization
{
    public interface IProblemSerializer
    {
        void SerializeProblem(IModelProblem modelProblem, string filePath);

        IModelProblem DeserializeProblem(string filePath);
    }
}
