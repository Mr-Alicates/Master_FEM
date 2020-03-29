using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC3D.ViewModel.Dialog
{
    public class DialogService : IDialogService
    {
        public string ShowOpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "3DPoC problem files (*.3DPoC)|*.3dPoc",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() != true)
            {
                return null;
            }

            return openFileDialog.FileName;
        }

        public string ShowSaveFileDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "3DPoC problem files (*.3DPoC)|*.3dPoc",
                AddExtension = true,
            };

            if (saveFileDialog.ShowDialog() != true)
            {
                return null;
            }

            return saveFileDialog.FileName;
        }
    }
}
