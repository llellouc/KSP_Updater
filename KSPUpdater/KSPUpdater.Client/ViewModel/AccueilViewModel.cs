using System.ComponentModel;
using System.Runtime.CompilerServices;
using KSPUpdater.Annotations;

namespace KSPUpdater.Client.ViewModel
{
    class AccueilViewModel : INotifyPropertyChanged
    {
        private bool _isUpdateInProgress;
        public bool IsUpdateInProgress
        {
            get => _isUpdateInProgress;
            set
            {
                _isUpdateInProgress = value;
                this.OnPropertyChanged(nameof(IsUpdateInProgress));
            }
        }
        public AccueilViewModel()
        {
            IsUpdateInProgress = false;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
