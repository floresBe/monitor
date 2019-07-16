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

namespace monitor.Views.EstacionesView
{
    /// <summary>
    /// Interaction logic for IndexEstacion.xaml
    /// </summary>
    public partial class IndexEstacion : Page
    {
        EstacionRepository _estacionRepository;

        public IndexEstacion()
        {
            InitializeComponent();
            Loaded += IndexEstacion_Loaded;
        }
        private void IndexEstacion_Loaded(object sender, RoutedEventArgs e)
        {
            _estacionRepository = new EstacionRepository();
            dataGridEstacion.ItemsSource = _estacionRepository.GetEstaciones();
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridEstacion.SelectedItem != null)
            {
                NavigationService.Navigate(new RegisterEstacion((Estacion)dataGridEstacion.SelectedItem));
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridEstacion.SelectedItem != null)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("¿Seguro que deseas eliminar la estación?", "Confirmación", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    if (_estacionRepository.DeleteEstacion((Estacion)dataGridEstacion.SelectedItem))
                    {
                        dataGridEstacion.ItemsSource = null;
                        dataGridEstacion.ItemsSource = _estacionRepository.GetEstaciones();
                    }
                }
            }
        }
        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegisterEstacion());

        }
    }
}
