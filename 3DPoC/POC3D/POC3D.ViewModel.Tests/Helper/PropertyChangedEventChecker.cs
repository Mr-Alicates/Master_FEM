using POC3D.ViewModel.Base;
using System.Collections.Generic;

namespace POC3D.ViewModel.Tests.Helper
{
    public class PropertyChangedEventChecker
    {
        private Observable _observable;

        public PropertyChangedEventChecker(Observable observable)
        {
            _observable = observable;

            _observable.PropertyChanged += (_ , e) => PropertiesRaised.Add(e.PropertyName);
        }

        public List<string> PropertiesRaised { get; } = new List<string>();
    }
}
