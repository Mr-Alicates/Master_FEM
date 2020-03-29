using POC3D.ViewModel.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
