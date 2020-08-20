using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using KSPUpdater.Annotations;
using KSPUpdater.Client.UpdateDisplay;

namespace KSPUpdater.Client.ViewModel
{
    class AccueilViewModel : INotifyPropertyChanged
    {
        private bool _isUpdateInProgress;
        private string _gameDataFolderPath;
        //private ObservableCollection<UpdateDetails> _successfullyUpdated;
        //private ObservableCollection<UpdateDetails> _alreadyUpdated;
        //private ObservableCollection<UpdateDetails> _failedToUpdate;
        private ObservableCollection<UpdateDetails> _logs;

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

        //public ObservableCollection<UpdateDetails> SuccessfullyUpdated
        //{
        //    get => _successfullyUpdated;
        //    set
        //    {
        //        if (_successfullyUpdated != value)
        //        {
        //            if (_successfullyUpdated != null)
        //                _successfullyUpdated.CollectionChanged -= OnUpdateStatusCollectionChanged;
        //            _successfullyUpdated = value;
        //            _successfullyUpdated.CollectionChanged += OnUpdateStatusCollectionChanged;
        //            this.OnPropertyChanged(nameof(SuccessfullyUpdated));
        //        }
        //    }
        //}

        //public ObservableCollection<UpdateDetails> AlreadyUpdated
        //{
        //    get => _alreadyUpdated;
        //    set
        //    {
        //        if (_alreadyUpdated != value)
        //        {
        //            if (_alreadyUpdated != null)
        //                _alreadyUpdated.CollectionChanged -= OnUpdateStatusCollectionChanged;
        //            _alreadyUpdated = value;
        //            _alreadyUpdated.CollectionChanged += OnUpdateStatusCollectionChanged;
        //            this.OnPropertyChanged(nameof(AlreadyUpdated));
        //        }
        //    }
        //}

        //public ObservableCollection<UpdateDetails> FailedToUpdate
        //{
        //    get => _failedToUpdate;
        //    set
        //    {
        //        if (_failedToUpdate != value)
        //        {
        //            if (_failedToUpdate != null)
        //                _failedToUpdate.CollectionChanged -= OnUpdateStatusCollectionChanged;
        //            _failedToUpdate = value;
        //            _failedToUpdate.CollectionChanged += OnUpdateStatusCollectionChanged;
        //            this.OnPropertyChanged(nameof(FailedToUpdate));
        //        }
        //    }
        //}

        //private void OnUpdateStatusCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    this.OnPropertyChanged(nameof(SuccessfullyUpdated));
        //    this.OnPropertyChanged(nameof(AlreadyUpdated));
        //    this.OnPropertyChanged(nameof(FailedToUpdate));
        //}

        public ObservableCollection<UpdateDetails> Logs
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
            Logs = new ObservableCollection<UpdateDetails>();
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
