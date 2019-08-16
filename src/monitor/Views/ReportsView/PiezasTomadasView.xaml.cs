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
    /// Interaction logic for TiempoCicloView.xaml
    /// </summary>
    public partial class PiezasTomadasView : Page
    {
        public PiezasTomadasView()
        {
            InitializeComponent();
            Loaded += PiezasTomadasView_Loaded;
        }

        private void PiezasTomadasView_Loaded(object sender, RoutedEventArgs e)
        {
            Data.ModeloRepository _modelo = new Data.ModeloRepository();
            Data.EstacionRepository _estacion = new Data.EstacionRepository();
            List<ComboBoxItem> modelos = new List<ComboBoxItem>();
            List<ComboBoxItem> estaciones = new List<ComboBoxItem>();

            dpDesde.SelectedDate = DateTime.Now;
            dpHasta.SelectedDate = DateTime.Now.AddDays(1);

            foreach (var item in _modelo.GetModelos())
            {
                modelos.Add(new ComboBoxItem() { IsSelected = false, DisplayValue = item.NumeroModelo, Id = item.ModeloId });
            }

            foreach (var item in _estacion.GetEstaciones())
            {
                estaciones.Add(new ComboBoxItem() { IsSelected = false, DisplayValue = item.Nombre, Id = item.EstacionId.ToString() });
            }

            cbModelo.ItemsSource = modelos; 
        }

        private void btnGenerar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidarCampos())
            {
                return;
            }
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string modelo = string.Empty;
            string estacion = string.Empty;
            foreach (ComboBoxItem item in cbModelo.ItemsSource)
            {
                if (item.IsSelected)
                {
                    modelo += item.Id + ",";
                }
            } 
            modelo = modelo.Remove(modelo.Length - 1, 1); 
            parameters["desde"] = dpDesde.ToString();
            parameters["hasta"] = dpHasta.ToString(); 
            parameters["modelo"] = modelo; 

            Reports.ReportViewer reportViewer = new Reports.ReportViewer(6, parameters);

            reportViewer.Show();
        }

        private bool ValidarCampos()
        {
            DateTime helper = new DateTime();
            if (!DateTime.TryParse(dpDesde.ToString(), out helper))
            {
                MessageBox.Show("Seleccione una fecha inicial valida.");
                return false;
            }
            if (!DateTime.TryParse(dpHasta.ToString(), out helper))
            {
                MessageBox.Show("Seleccione una fecha final valida.");
                return false;
            }
            if (!(cbModelo.ItemsSource as List<ComboBoxItem>).Any(a => a.IsSelected))
            {
                MessageBox.Show("Seleccione por lo menos un modelo.");
                return false;
            }
             
            return true;
        }

    }
}
