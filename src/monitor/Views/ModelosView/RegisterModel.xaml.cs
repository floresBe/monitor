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
    /// Interaction logic for RegisterModel.xaml
    /// </summary>
    public partial class RegisterModel : Page
    {
        ModeloRepository _modeloRepository;

        private bool isEdit;
        private Modelo Model;

        public RegisterModel()
        {
            InitializeComponent();
            Loaded += RegisterModel_Loaded;
            isEdit = false;
        }
        public RegisterModel(Modelo modelo)
        {
            InitializeComponent();
            Loaded += RegisterModel_Loaded;
            Model = modelo;
            isEdit = true;
        }
        private void RegisterModel_Loaded(object sender, RoutedEventArgs e)
        {
            _modeloRepository = new ModeloRepository();

            if (isEdit)
            { 
                tbNoModelo.Text = Model.NumeroModelo.ToString();
                tbRouting.Text =  Model.Routing.ToString();
                tbAyudaVisual.Text = Model.RutaAyudaVisual;

                return;
            } 
            
        }

        private void TbNoModelo_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void TbRouting_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TbAyudaVisual_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateFields())
                {
                    if (isEdit)
                    {
                        Model.FechaHora = DateTime.Now;
                        Model.NumeroModelo = tbNoModelo.Text;
                        Model.RutaAyudaVisual = tbAyudaVisual.Text;
                        Model.Routing = Convert.ToDouble(tbRouting.Text);

                        _modeloRepository.UpdateModelo(Model);
                        NavigationService.GoBack();
                        return;
                    }

                    Modelo lastModel = _modeloRepository.GetLastModelo();
                    string modeloId = lastModel.ModeloId + "1";
                    Modelo modelo = new Modelo()
                    {
                        ModeloId = modeloId,
                        FechaHora = DateTime.Now,
                        NumeroModelo = tbNoModelo.Text,
                        Routing = Convert.ToDouble(tbRouting.Text),
                        RutaAyudaVisual = tbAyudaVisual.Text, 
                        Estatus = 1
                    };
                    _modeloRepository.InsertModelo(modelo);
                    NavigationService.GoBack();
                    return;
                }

                throw new Exception("Verifique que todos los campos esten capturados correctamente.");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(tbNoModelo.Text))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(tbRouting.Text))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(tbAyudaVisual.Text))
            {
                return false;
            }
            return true;
        }
    }
}
