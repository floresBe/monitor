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
    /// Interaction logic for RunModel.xaml
    /// </summary>
    public partial class RunModel : UserControl
    {
        private ModeloRepository _modeloRepository;

        Modelo modelo;
        string PID;

        System.Windows.Forms.Screen[] screens;

        public RunModel()
        {
            InitializeComponent();
            Loaded += RunModel_Loaded;
        }
        private void RunModel_Loaded(object sender, RoutedEventArgs e)
        {
            _modeloRepository = new ModeloRepository();
            cbModelos.ItemsSource = _modeloRepository.GetModelos();

            screens = System.Windows.Forms.Screen.AllScreens;
        }
        private void BtnIniciar_Click(object sender, RoutedEventArgs e)
        {
            grdIniciarModelo.Visibility = Visibility.Visible;
            btnIniciar.Visibility = Visibility.Collapsed;
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void BtnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPID.Text))
            {
                MessageBox.Show("Ingresar número de PID.");
                return;
            }

            if (cbModelos.SelectedItem == null)
            {
                MessageBox.Show("Seleccione modelo.");
                return;
            }

            PID = txtPID.Text;
            modelo = (Modelo)cbModelos.SelectedItem;

            lblPID.Content = PID;
            lblModelo.Content = modelo.NumeroModelo;

            grdIniciarModelo.Visibility = Visibility.Collapsed;
            StartMonitoringAsync();
        }
        private void EstacionesItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                Estacion selectedEstacion = (Estacion)e.AddedItems[0];

                if (selectedEstacion.Modelo != null)
                {
                    MessageBoxResult boxButton = MessageBox.Show("¿Seguro que desea utilizar una estación que ya esta siendo ocupada?", "Utilizar estación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (boxButton == MessageBoxResult.No)
                    {
                        estacionesItems.SelectedItems.Remove(e.AddedItems[0]);
                    }
                }
            }
        }
        private void BtnIniciarModelo_Click(object sender, RoutedEventArgs e)
        {
            if(estacionesItems.SelectedItems.Count == 0)
            {
                MessageBox.Show("No selecciono ninguna estación.", "Atención");
                return;
            }

            foreach (var est in estacionesItems.SelectedItems)
            {
                Estacion estacion = (Estacion)est;

                //Verificar si la estacion se encuentra activa.
                if (App.estacionesWindows.Any(a => a.Estacion.EstacionId == estacion.EstacionId))
                {
                    //Cerrar y eliminar estacion activa.
                    App.estacionesWindows.Where(a => a.Estacion.EstacionId == estacion.EstacionId).FirstOrDefault().Close();
                    App.estacionesWindows.Remove(App.estacionesWindows.Where(a => a.Estacion.EstacionId == estacion.EstacionId).FirstOrDefault());
                }

                //Buscar monitor indicado.
                var screen = screens.Where(w => w.DeviceName == estacion.Monitor).FirstOrDefault();

                //Crear pantalla de monitoreo.
                Monitoreo monitoreoWindow = new Monitoreo(estacion, modelo, PID);
                monitoreoWindow.Left = screen.WorkingArea.Left;
                monitoreoWindow.Top = screen.WorkingArea.Top;
                monitoreoWindow.Width = screen.Bounds.Width;
                monitoreoWindow.Height = screen.Bounds.Height;
                monitoreoWindow.WindowState = WindowState.Normal;
                monitoreoWindow.Show();
                App.estacionesWindows.Add(monitoreoWindow);

                //Indicar que la estación esta corriendo el modelo seleccionado. 
                App.estaciones.Where(w => w.EstacionId == estacion.EstacionId).FirstOrDefault().Modelo = modelo.NumeroModelo;
                App.estaciones.Where(w => w.EstacionId == estacion.EstacionId).FirstOrDefault().Mensaje = "Modelo Actual:";
            }

            grdEstaciones.Visibility = Visibility.Collapsed;
            grdModeloCorriendo.Visibility = Visibility.Visible;
        }
        private void BtnDetener_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult boxButton = MessageBox.Show("¿Seguro que desea detener el proceso del modelo " + modelo.NumeroModelo + "?", "Detener proceso", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (boxButton == MessageBoxResult.Yes)
            {
                //Buscar estaciones donde el modelo esta corriendo. 
                foreach (var estacion in App.estaciones.Where(w => w.Modelo == modelo.NumeroModelo))
                {
                    estacion.Modelo = null;
                    estacion.Mensaje = "Libre";

                    //Cerrar y eliminar estacion.
                    if (App.estacionesWindows.Any(a => a.Estacion.EstacionId == estacion.EstacionId))
                    {
                        App.estacionesWindows.Where(a => a.Estacion.EstacionId == estacion.EstacionId).FirstOrDefault().Close();
                        App.estacionesWindows.Remove(App.estacionesWindows.Where(a => a.Estacion.EstacionId == estacion.EstacionId).FirstOrDefault());
                    }
                }

                //Reiniciar valores.
                txtPID.Text = null;
                cbModelos.SelectedItem = null;

                estacionesItems.UnselectAll();

                grdModeloCorriendo.Visibility = Visibility.Collapsed;
                grdPiezasTomadas.Visibility = Visibility.Visible;
            }
        }
        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            bool todosCero = true;
            MessageBoxResult boxButton = MessageBox.Show("¿Seguro que desea registrar las piezas tomadas?", "Registrar piezas tomadas", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (boxButton == MessageBoxResult.Yes)
            {
                if (string.IsNullOrEmpty(tbIngenieria.Text))
                {
                    tbIngenieria.Text = "0";
                }
                else
                {
                    todosCero = false;
                }

                if (string.IsNullOrEmpty(tbCalidad.Text))
                {
                    tbCalidad.Text = "0";
                }
                else
                {
                    todosCero = false;
                }

                if (string.IsNullOrEmpty(tbProduccion.Text))
                {
                    tbProduccion.Text = "0";
                }
                else
                {
                    todosCero = false;
                }
                if (todosCero)
                {
                    MessageBox.Show("No es válido realizar registros en cero.", "Información");
                    return;
                }

                RegistrarPiezas();

                grdPiezasTomadas.Visibility = Visibility.Collapsed;
                btnIniciar.Visibility = Visibility.Visible;
            }
        }
        private void BtnVolverInicio_Click(object sender, RoutedEventArgs e)
        {
            lblPiezasTomadas.Visibility = Visibility.Collapsed;
            grdPiezasTomadas.Visibility = Visibility.Collapsed;
            btnIniciar.Visibility = Visibility.Visible;
        } 
        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            grdIniciarModelo.Visibility = Visibility.Collapsed;
            btnIniciar.Visibility = Visibility.Visible;
        }

        private void BtnCancelar2_Click(object sender, RoutedEventArgs e)
        {
            grdEstaciones.Visibility = Visibility.Collapsed;
            grdIniciarModelo.Visibility = Visibility.Visible;
        }
        private void StartMonitoringAsync()
        {
            foreach (var estacion in App.estaciones)
            {
                var screen = screens.Where(w => w.DeviceName == estacion.Monitor).FirstOrDefault();

                if (screen != null)
                {
                    estacion.isRunning = true;
                }
            }
            estacionesItems.ItemsSource = App.estaciones.Where(a => a.isRunning);
            grdEstaciones.Visibility = Visibility.Visible;
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
                    ModeloId = modelo.ModeloId,
                    FechaHora = DateTime.Now
                };

                piezasTomadasRepository.InsertPiezasTomadas(piezasTomadas);

                MessageBox.Show("Piezas registradas con éxito.");

                tbIngenieria.Text = string.Empty;
                tbCalidad.Text = string.Empty;
                tbProduccion.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al registrar piezas. - Error:" + ex.Message);
            }
        }

    }
}
