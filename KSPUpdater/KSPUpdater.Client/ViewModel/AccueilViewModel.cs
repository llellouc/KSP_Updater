using System.ComponentModel;
using System.Runtime.CompilerServices;
using KSPUpdater.Annotations;

namespace KSPUpdater.Client.ViewModel
{
    class AccueilViewModel : INotifyPropertyChanged
    {
        private bool _isUpdateInProgress;
        private string _gameDataFolderPath;

        public string GameDataFolderPath
        {
            get => _gameDataFolderPath;
            set
            {
                _gameDataFolderPath = value;
                this.OnPropertyChanged(nameof(CanClickOnUpdate));
            }
        }

        public bool IsUpdateInProgress
        {
            get => _isUpdateInProgress;
            set
            {
                _isUpdateInProgress = value;
                this.OnPropertyChanged(nameof(IsUpdateInProgress));
                this.OnPropertyChanged(nameof(CanClickOnUpdate));
            }
        }

        public bool CanClickOnUpdate
        {
            get => !IsUpdateInProgress && !string.IsNullOrEmpty(GameDataFolderPath);
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
