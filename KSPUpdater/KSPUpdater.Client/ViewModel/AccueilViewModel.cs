using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using KSPUpdater.Annotations;
using KSPUpdater.Client.UpdateDisplay;
using KSPUpdater.Common;

namespace KSPUpdater.Client.ViewModel
{
    class AccueilViewModel : INotifyPropertyChanged
    {
        private bool _isUpdateInProgress;
        private string _gameDataFolderPath;
        private ObservableDictionary<string, UpdateDetails> _logs;

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

        public ObservableDictionary<string, UpdateDetails> Logs
        {
            get => _logs;
            set
            {
                if (_logs != value)
                {
                    if (_logs != null)
                        _logs.CollectionChanged -= OnLogsCollectionChanged;
                    _logs = value;
                    _logs.CollectionChanged += OnLogsCollectionChanged;
                    this.OnPropertyChanged(nameof(Logs));
                }
            }
        }

        private void OnLogsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(Logs));
        }

        public AccueilViewModel()
        {
            IsUpdateInProgress = false;
            Logs = new ObservableDictionary<string, UpdateDetails>();
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
