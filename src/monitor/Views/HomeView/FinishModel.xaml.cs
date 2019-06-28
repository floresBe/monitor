using monitor.Data;
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
    /// Interaction logic for FinishModel.xaml
    /// </summary>
    public partial class FinishModel : Page
    {
        public FinishModel()
        {
            InitializeComponent();
        }

        private void RegistrarPiezas()
        {
            try
            {
                PiezasTomadasRepository piezasTomadasRepository = new PiezasTomadasRepository();
                int ingenieria = int.Parse(tbIngenieria.Text);
                int calidad = int.Parse(tbCalidad.Text);
                int produccion = int.Parse(tbProduccion.Text);

                PiezasTomadas piezasTomadas = new PiezasTomadas()
                {
                    Ingenieria = ingenieria,
                    Calidad = calidad,
                    Produccion = produccion,
                    ModeloId = App.modelo.ModeloId,
                    FechaHora = DateTime.Now
                };

                piezasTomadasRepository.InsertPiezasTomadas(piezasTomadas);

                MessageBox.Show("Piezas registradas con éxito.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al registrar piezas. - Error:" + ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult boxButton = MessageBox.Show("¿Seguro que desea registrar las piezas tomadas?", "Registrar piezas tomadas", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (boxButton == MessageBoxResult.Yes)
            {
                if (string.IsNullOrEmpty(tbIngenieria.Text))
                {
                    tbIngenieria.Text = "0";
                }

                if (string.IsNullOrEmpty(tbCalidad.Text))
                {
                    tbCalidad.Text = "0";
                }

                if (string.IsNullOrEmpty(tbProduccion.Text))
                {
                    tbProduccion.Text = "0";
                }

                RegistrarPiezas();

                StartModel page = new StartModel();
                NavigationService.Navigate(page);
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            StartModel page = new StartModel();
            NavigationService.Navigate(page);
        }
    }
}
