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

namespace monitor.Views.ReportsView
{
    /// <summary>
    /// Interaction logic for PiezasPorModeloView.xaml
    /// </summary>
    public partial class PiezasPorModeloView : Page
    {
        public PiezasPorModeloView()
        {
            InitializeComponent();
        }

        private void btnGenerar_Click(object sender, RoutedEventArgs e)
        {
            Reports.ReportViewer reportViewer = new Reports.ReportViewer(1);

            reportViewer.Show();
        }
    }
}
