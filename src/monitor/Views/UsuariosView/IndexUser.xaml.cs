using monitor.Data;
using monitor.Fingerprint.Views.UsuariosView;
using System.Windows;
using System.Windows.Controls;

namespace monitor.Views.UsuariosView
{
    /// <summary>
    /// Interaction logic for IndexUser.xaml
    /// </summary>
    public partial class IndexUser : Page
    {
        private UsuarioRepository _usuarioRepository;

        public IndexUser()
        {
            InitializeComponent();
            Loaded += Index_Loaded;
        }

        private void Index_Loaded(object sender, RoutedEventArgs e)
        {
            _usuarioRepository = new UsuarioRepository();
            dataGridUsuario.ItemsSource = _usuarioRepository.GetUsuarios();
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegisterUser());
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridUsuario.SelectedItem != null)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("¿Seguro que deseas eliminar al usuario?", "Confirmación", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    if (_usuarioRepository.DeleteUsuario((Usuario)dataGridUsuario.SelectedItem))
                    {
                        dataGridUsuario.ItemsSource = null;
                        dataGridUsuario.ItemsSource = _usuarioRepository.GetUsuarios();
                    }
                }
            }
        }
    }
}
