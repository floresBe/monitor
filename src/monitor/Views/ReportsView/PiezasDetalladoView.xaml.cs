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
    /// Interaction logic for PiezasDetalladoView.xaml
    /// </summary>
    public partial class PiezasDetalladoView : Page
    {
        public PiezasDetalladoView()
        {
            InitializeComponent();
            Loaded += PiezasDetalladoView_Loaded;
        }

        private void PiezasDetalladoView_Loaded(object sender, RoutedEventArgs e)
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
            cbEstacion.ItemsSource = estaciones;

        }

        private void btnGenerar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidarCampos())
            {
                return;
            }
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string modelo = string.Empty;
            string nModelo = string.Empty;
            string estacion = string.Empty; 

            modelo = (cbModelo.SelectedItem as ComboBoxItem).Id;
            nModelo = (cbModelo.SelectedItem as ComboBoxItem).DisplayValue;

            foreach (ComboBoxItem item in cbEstacion.ItemsSource)
            {
                if (item.IsSelected)
                {
                    estacion += item.Id + ",";
                }
            }
            estacion = estacion.Remove(estacion.Length - 1, 1);
            parameters["desde"] = dpDesde.ToString();
            parameters["hasta"] = dpHasta.ToString(); 
            parameters["modelo"] = modelo;
            parameters["nModelo"] = nModelo;
            parameters["estacion"] = estacion.Replace("#", "");
            parameters["descontadosIng"] = "0";
            parameters["calidad"] = "0";
            parameters["produccion"] = "0";
            Reports.ReportViewer reportViewer = new Reports.ReportViewer(2, parameters);

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
            if (cbModelo.SelectedItem == null)
            {
                MessageBox.Show("Seleccione por lo menos un modelo.");
                return false;
            }

            if (!(cbEstacion.ItemsSource as List<ComboBoxItem>).Any(a => a.IsSelected))
            {
                MessageBox.Show("Seleccione por lo menos una estación.");
                return false;
            }
            return true;
        }

    }
}
