using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC3D.ViewModel
{
    public class BodyViewModel
    {
        private ObservableCollection<ElementViewModel> _elements = new ObservableCollection<ElementViewModel>();



        public ObservableCollection<ElementViewModel> Elements => _elements;
    }
}
