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
    /// Interaction logic for StartModel.xaml
    /// </summary>
    public partial class StartModel : Page
    {
        private ModeloRepository _modeloRepository; 

        public StartModel()
        {
            InitializeComponent();
            Loaded += StartModel_Loaded;
        } 
        private void StartModel_Loaded(object sender, RoutedEventArgs e)
        {
            _modeloRepository = new ModeloRepository(); 
            cbModelos.ItemsSource = _modeloRepository.GetModelos(); 
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //returns the character that match with the expression
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(txtPID.Text))
            {
                MessageBox.Show("Ingresar número de PID.");
                return;
            }

            if(cbModelos.SelectedItem == null)
            {
                MessageBox.Show("Seleccione modelo.");
                return;
            }

            //App.PID = txtPID.Text;
            //App.modelo = (Modelo) cbModelos.SelectedItem;
 
            //App.stopModelPage = new StopModel();
            //NavigationService.Navigate(App.stopModelPage);
        } 
       
    }
}
