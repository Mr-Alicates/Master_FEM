using System.IO;

namespace POC3D.Model.Serialization
{
    public class FileSystem : IFileSystem
    {
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public void FileDelete(string path)
        {
            File.Delete(path);
        }

        public void FileWrite(string path, string content)
        {
            File.WriteAllText(path, content);
        }

        public string FileRead(string path)
        {
            return File.ReadAllText(path);
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }
    }
}
