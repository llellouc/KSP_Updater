using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KSPUpdater
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
            _vm.Update();
        }
    }
}
