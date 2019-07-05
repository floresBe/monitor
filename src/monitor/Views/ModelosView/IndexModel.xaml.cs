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

namespace monitor.Views.ModelosView
{
    /// <summary>
    /// Interaction logic for IndexModel.xaml
    /// </summary>
    public partial class IndexModel : Page
    {
        private ModeloRepository _modeloRepository;
        public IndexModel()
        {
            InitializeComponent();
            Loaded += Index_Loaded;
        }
        private void Index_Loaded(object sender, RoutedEventArgs e)
        {
            _modeloRepository = new ModeloRepository();
            dataGridModelo.ItemsSource = _modeloRepository.GetModelos();
        }
        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if(dataGridModelo.SelectedItem != null)
            {
                NavigationService.Navigate(new RegisterModel((Modelo)dataGridModelo.SelectedItem));
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridModelo.SelectedItem != null)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("¿Seguro que deseas eliminar el modelo?", "Confirmación", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    if (_modeloRepository.DeleteModelo((Modelo)dataGridModelo.SelectedItem))
                    {
                        dataGridModelo.ItemsSource = null;
                        dataGridModelo.ItemsSource = _modeloRepository.GetModelos();
                    }
                }
            }
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegisterModel());
        }
    }
}
