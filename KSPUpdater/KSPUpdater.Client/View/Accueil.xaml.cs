using System;
using System.Threading;
using System.Windows;
using KSPUpdater.Client.ViewModel;
using KSPUpdater.Common;

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
                var th = new Thread(new ParameterizedThreadStart(UpdaterOrchestraMaster.LaunchUpdate));
                th.Name = "Update Thread";
                th.Start(new MyWebView(this.ToolkitWebView));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
