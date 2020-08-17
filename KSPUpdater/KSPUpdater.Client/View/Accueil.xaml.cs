using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using KSPUpdater.Client.ViewModel;
using KSPUpdater.Common;
using Microsoft.Win32;

namespace KSPUpdater.Client.View
{
    /// <summary>
    /// Interaction logic for Accueil.xaml
    /// </summary>
    public partial class Accueil : Window
    {
        private AccueilViewModel _vm;

        public Accueil()
        {
            _vm = new AccueilViewModel();
            this.DataContext = _vm;
            InitializeComponent();
        }


        private void Update_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _vm.IsUpdateInProgress = true;
                var param = new UpdateOrchestraMasterParams()
                {
                    Webview = new MyWebView(this.ToolkitWebView),
                    GameDataPath = _vm.GameDataFolderPath
                };

                var th = new Thread(new ParameterizedThreadStart(UpdaterOrchestraMaster.LaunchUpdate));
                th.Name = "Update Thread";
                th.Start(param);
                Task.Run(() =>
                {
                    th.Join();
                    _vm.IsUpdateInProgress = false;
                });
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private void SelectFolder_OnClick(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();

                _vm.GameDataFolderPath = dialog.SelectedPath;
            }
        }
    }
}
