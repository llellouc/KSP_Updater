using System;
using System.Threading;
using System.Windows;
using KSPUpdater.Extensions;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.Toolkit.Wpf.UI.Controls;

namespace KSPUpdater
{
    /// <summary>
    /// Interaction logic for Accueil.xaml
    /// </summary>
    public partial class Accueil : Window
    {
        private AccueilViewModel _vm;

        private MyWebView myWebView;

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
