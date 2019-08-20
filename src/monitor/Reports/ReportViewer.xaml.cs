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
            DateTime desde;
            DateTime hasta;
            string modelo;
            string nModelo;
            string estacion;
             
            switch (value)
            {
                case 1:
                    desde = DateTime.Parse(parameters["desde"].ToString());
                    hasta = DateTime.Parse(parameters["hasta"].ToString());
                    modelo = parameters["modelo"].ToString();
                    estacion = parameters["estacion"].ToString(); 
                    DataContext = new ViewModels.PiezasPorModeloViewModel(desde,hasta,modelo,estacion);
                    break;  
                case 2:
                    desde = DateTime.Parse(parameters["desde"].ToString());
                    hasta = DateTime.Parse(parameters["hasta"].ToString());
                    modelo = parameters["modelo"].ToString();
                    estacion = parameters["estacion"].ToString(); 
                    nModelo = parameters["nModelo"].ToString(); 
                    DataContext = new ViewModels.PiezasDetalladoViewModel(desde, hasta, modelo, estacion, nModelo);
                    break;
                case 3:
                    desde = DateTime.Parse(parameters["desde"].ToString());
                    hasta = DateTime.Parse(parameters["hasta"].ToString()); 
                    estacion = parameters["estacion"].ToString();
                    modelo = parameters["modelo"].ToString();
                    nModelo = parameters["nModelo"].ToString();
                    DataContext = new ViewModels.SoldadoraResultadoViewModel(desde, hasta, modelo, estacion, nModelo);
                    break;
                case 4:
                    desde = DateTime.Parse(parameters["desde"].ToString());
                    hasta = DateTime.Parse(parameters["hasta"].ToString());
                    modelo = parameters["modelo"].ToString();
                    estacion = parameters["estacion"].ToString();
                    DataContext = new ViewModels.PiezasMalasViewModel(desde, hasta, modelo, estacion);
                    break;
                case 5:
                    desde = DateTime.Parse(parameters["desde"].ToString());
                    hasta = DateTime.Parse(parameters["hasta"].ToString());
                    modelo = parameters["modelo"].ToString();
                    estacion = parameters["estacion"].ToString();
                    DataContext = new ViewModels.TiempoCicloViewModel(desde, hasta, modelo, estacion);
                    break;
                case 6:
                    desde = DateTime.Parse(parameters["desde"].ToString());
                    hasta = DateTime.Parse(parameters["hasta"].ToString());
                    modelo = parameters["modelo"].ToString(); 
                    DataContext = new ViewModels.PiezasTomadasViewModel(desde, hasta, modelo);
                    break;

            }
        }
    }
}
