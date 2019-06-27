using monitor.Data;
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
    }
}
