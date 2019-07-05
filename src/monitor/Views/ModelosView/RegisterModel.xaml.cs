using monitor.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
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
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\d+(\.(\d{0,2})?)?");
            bool match = !regex.IsMatch(tbRouting.Text + e.Text);
            e.Handled = match;
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        { 
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    tbAyudaVisual.Text = fbd.SelectedPath + @"\"; 
                 }
            }
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
                    string modeloId = GetModeloId();

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
                System.Windows.MessageBox.Show(ex.Message);
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
            try
            {
                Convert.ToDouble(tbRouting.Text);
            }
            catch(Exception)
            {
                throw new Exception("El valor de routing no es válido.");
            }
            return true;
        }

        private char GetLetter(char lastId)
        {
            char id = ' ';

            if (lastId == 'Z')
                id = 'A';
             else
                id = (char)(((int)lastId) + 1);

            return id;
         
        }

        private string GetModeloId()
        {
            string modeloId = "";
            Modelo lastModel = _modeloRepository.GetLastModelo();
            char lastId = lastModel.ModeloId[lastModel.ModeloId.Length - 1];
            char newId = GetLetter(lastId);

            if (lastId == 'Z')
            {
                for (int i = 0; i < lastModel.ModeloId.Length + 1; i++)
                {
                    modeloId += newId;
                }
            }
            else
            {
                for (int i = 0; i < lastModel.ModeloId.Length; i++)
                {
                    modeloId += newId;
                }
            }
            return modeloId;
        }
    }
}
