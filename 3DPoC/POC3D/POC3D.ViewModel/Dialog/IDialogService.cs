﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC3D.ViewModel.Dialog
{
    public interface IDialogService
    {
        string ShowOpenFileDialog();

        string ShowSaveFileDialog();
    }
}
