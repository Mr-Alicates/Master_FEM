namespace POC3D.ViewModel.Base
{
    public abstract class SelectableViewModel : Observable
    {
        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }
}
