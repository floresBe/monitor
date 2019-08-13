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
using System.Windows.Shapes;

namespace monitor.Reports
{
    /// <summary>
    /// Interaction logic for ReportViewer.xaml
    /// </summary>
    public partial class ReportViewer : Window
    {
        public ReportViewer(int reportContext, Dictionary<string, string> parameters)
        {
            InitializeComponent();
            SetDataContext(reportContext,parameters);
        }

        private void SetDataContext(int value, Dictionary<string,string> parameters)
        {
            switch (value)
            {
                case 1:
                    DateTime desde = DateTime.Parse(parameters["desde"].ToString());
                    DateTime hasta = DateTime.Parse(parameters["hasta"].ToString());
                    string modelo = parameters["modelo"].ToString();
                    string estacion = parameters["estacion"].ToString();
                    int descontadosIng = int.Parse(parameters["descontadosIng"].ToString());
                    int calidad = int.Parse(parameters["calidad"].ToString());
                    int produccion = int.Parse(parameters["produccion"].ToString());
                    DataContext = new ViewModels.PiezasPorModeloViewModel(desde,hasta,modelo,estacion, descontadosIng,calidad,produccion);
                    break;


            }
        }
    }
}
