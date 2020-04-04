using POC3D.ViewModel.Dialog;

namespace POC3D.ViewModel.Tests.Helper
{
    public class FakeDialogService : IDialogService
    {
        public string FilePath { get; set; }

        public string ShowOpenFileDialog()
        {
            return FilePath;
        }

        public string ShowSaveFileDialog()
        {
            return FilePath;
        }
    }
}
