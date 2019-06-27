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

namespace monitor.Views.HomeView
{
    /// <summary>
    /// Interaction logic for StopModel.xaml
    /// </summary>
    public partial class StopModel : Page
    {
        public StopModel()
        {
            InitializeComponent();

            lblModelo.Content = App.modelo.NumeroModelo;
            lblPID.Content = App.PID;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FinishModel page = new FinishModel();
            NavigationService.Navigate(page);
        }
    }
}
