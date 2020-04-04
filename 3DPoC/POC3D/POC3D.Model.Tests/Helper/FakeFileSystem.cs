using Newtonsoft.Json;
using POC3D.Model.Serialization;
using System;

namespace POC3D.Model.Tests.Helper
{
    public class FakeFileSystem : IFileSystem
    {
        public string DeletedFile { get; set; }
        public string ExistingFile { get; set; }
        public string ExistingFileContent { get; set; }
        public string CreatedDirectory { get; set; }

        public void FileDelete(string path)
        {
            DeletedFile = path;
            ExistingFile = null;
        }

        public bool FileExists(string path)
        {
            return ExistingFile == path;
        }

        public string FileRead(string path)
        {
            if(ExistingFile == path)
            {
                return ExistingFileContent;
            }

            return null;
        }

        public void FileWrite(string path, string content)
        {
            ExistingFile = path;
            ExistingFileContent = content;
        }

        public void CreateDirectory(string path)
        {
            CreatedDirectory = path;
        }

        public bool DirectoryExists(string path)
        {
            return CreatedDirectory == path;
        }

        public ProblemMemento TryGetProblemMemento()
        {
            if (string.IsNullOrEmpty(ExistingFileContent))
            {
                return null;
            }

            try
            {
                return JsonConvert.DeserializeObject<ProblemMemento>(ExistingFileContent);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
