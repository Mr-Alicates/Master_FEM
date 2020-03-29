namespace POC3D.Model.Serialization
{
    public interface IFileSystem
    {
        bool FileExists(string path);

        void FileDelete(string path);

        void FileWrite(string path, string content);

        string FileRead(string path);

        bool DirectoryExists(string path);

        void CreateDirectory(string path);
    }
}
